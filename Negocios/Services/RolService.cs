using System.Security.Claims;
using Datos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace Negocios.Services;

public class RolService : IRolService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    private static readonly HashSet<string>
        RolesProtegidos =
            new(
                new[]
                {
                    "Administrador",
                    "Recepcionista",
                    "Mecanico",
                    "EncargadoInventario",
                    "Vendedor",
                    "Cajero",
                    "Supervisor"
                },
                StringComparer.OrdinalIgnoreCase);

    public RolService(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IEnumerable<RolAdministracionDto>>
        ObtenerTodosAsync()
    {
        var roles = await _roleManager.Roles
            .OrderBy(r => r.Name)
            .ToListAsync();

        var resultado =
            new List<RolAdministracionDto>();

        foreach (var rol in roles)
        {
            var usuarios =
                await _userManager
                    .GetUsersInRoleAsync(
                        rol.Name ?? string.Empty);

            var permisos =
                await ObtenerPermisosDelRolAsync(rol);

            resultado.Add(
                new RolAdministracionDto
                {
                    Id = rol.Id,
                    Nombre = rol.Name ?? string.Empty,
                    CantidadUsuarios =
                        usuarios.Count,
                    Permisos = permisos
                });
        }

        return resultado;
    }

    public async Task<RolAdministracionDto?>
        ObtenerPorIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        var rol =
            await _roleManager.FindByIdAsync(id);

        if (rol == null)
            return null;

        var usuarios =
            await _userManager
                .GetUsersInRoleAsync(
                    rol.Name ?? string.Empty);

        var permisos =
            await ObtenerPermisosDelRolAsync(rol);

        return new RolAdministracionDto
        {
            Id = rol.Id,
            Nombre = rol.Name ?? string.Empty,
            CantidadUsuarios =
                usuarios.Count,
            Permisos = permisos,
            PermisosDisponibles =
                PermisosCatalogo.Todos.ToList()
        };
    }

    public async Task<IEnumerable<string>>
        ObtenerPermisosDisponiblesAsync()
    {
        return PermisosCatalogo.Todos;
    }

    public async Task<(bool Exitoso, string Mensaje)>
        CrearAsync(
            RolAdministracionDto dto,
            bool administrarPermisos)
    {
        var nombre =
            dto.Nombre?.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            return (
                false,
                "El nombre del rol es obligatorio.");
        }

        var rolExistente =
            await _roleManager
                .FindByNameAsync(nombre);

        if (rolExistente != null)
        {
            return (
                false,
                "Ya existe un rol con ese nombre.");
        }

        var rol =
            new IdentityRole(nombre);

        var resultado =
            await _roleManager.CreateAsync(rol);

        if (!resultado.Succeeded)
        {
            return (
                false,
                ObtenerErrores(resultado));
        }

        // Todo rol nuevo queda marcado como
        // administrable por permisos.
        var marcador =
            await _roleManager.AddClaimAsync(
                rol,
                new Claim(
                    RolClaims.PermisosConfigurados,
                    "true"));

        if (!marcador.Succeeded)
        {
            await _roleManager.DeleteAsync(rol);

            return (
                false,
                ObtenerErrores(marcador));
        }

        if (administrarPermisos)
        {
            var resultadoPermisos =
                await SincronizarPermisosAsync(
                    rol,
                    dto.Permisos);

            if (!resultadoPermisos.Exitoso)
            {
                await _roleManager.DeleteAsync(rol);

                return resultadoPermisos;
            }
        }

        return (
            true,
            "Rol creado correctamente.");
    }

    public async Task<(bool Exitoso, string Mensaje)>
        ActualizarAsync(
            RolAdministracionDto dto,
            bool administrarPermisos)
    {
        if (string.IsNullOrWhiteSpace(dto.Id))
        {
            return (
                false,
                "El rol indicado no es válido.");
        }

        var rol =
            await _roleManager
                .FindByIdAsync(dto.Id);

        if (rol == null)
        {
            return (
                false,
                "El rol no existe.");
        }

        var nombre =
            dto.Nombre?.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            return (
                false,
                "El nombre del rol es obligatorio.");
        }

        var otroRol =
            await _roleManager
                .FindByNameAsync(nombre);

        if (otroRol != null &&
            otroRol.Id != rol.Id)
        {
            return (
                false,
                "Ya existe otro rol con ese nombre.");
        }

        rol.Name = nombre;
        rol.NormalizedName =
            _roleManager.NormalizeKey(nombre);

        var resultadoActualizar =
            await _roleManager.UpdateAsync(rol);

        if (!resultadoActualizar.Succeeded)
        {
            return (
                false,
                ObtenerErrores(
                    resultadoActualizar));
        }

        if (administrarPermisos)
        {
            var resultadoPermisos =
                await SincronizarPermisosAsync(
                    rol,
                    dto.Permisos);

            if (!resultadoPermisos.Exitoso)
            {
                return resultadoPermisos;
            }
        }

        return (
            true,
            "Rol actualizado correctamente.");
    }

    public async Task<(bool Exitoso, string Mensaje)>
        EliminarAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return (
                false,
                "El rol indicado no es válido.");
        }

        var rol =
            await _roleManager
                .FindByIdAsync(id);

        if (rol == null)
        {
            return (
                false,
                "El rol no existe.");
        }

        var nombre =
            rol.Name ?? string.Empty;

        if (RolesProtegidos.Contains(nombre))
        {
            return (
                false,
                "Los roles principales de AXIS no pueden eliminarse.");
        }

        var usuarios =
            await _userManager
                .GetUsersInRoleAsync(nombre);

        if (usuarios.Count > 0)
        {
            return (
                false,
                "No se puede eliminar un rol que tiene usuarios asignados.");
        }

        var resultado =
            await _roleManager.DeleteAsync(rol);

        if (!resultado.Succeeded)
        {
            return (
                false,
                ObtenerErrores(resultado));
        }

        return (
            true,
            "Rol eliminado correctamente.");
    }

    private async Task<List<string>>
        ObtenerPermisosDelRolAsync(
            IdentityRole rol)
    {
        var claims =
            await _roleManager.GetClaimsAsync(rol);

        return claims
            .Where(c =>
                c.Type == RolClaims.Permiso)
            .Select(c => c.Value)
            .Where(p =>
                PermisosCatalogo.Todos.Contains(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p)
            .ToList();
    }

    private async Task<(bool Exitoso, string Mensaje)>
        SincronizarPermisosAsync(
            IdentityRole rol,
            IEnumerable<string>? permisos)
    {
        var permisosValidos =
            (permisos ?? Enumerable.Empty<string>())
                .Where(p =>
                    PermisosCatalogo.Todos.Contains(p))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

        var claims =
            await _roleManager.GetClaimsAsync(rol);

        var claimsPermiso =
            claims
                .Where(c =>
                    c.Type == RolClaims.Permiso)
                .ToList();

        foreach (var claim in claimsPermiso)
        {
            var resultadoEliminar =
                await _roleManager.RemoveClaimAsync(
                    rol,
                    claim);

            if (!resultadoEliminar.Succeeded)
            {
                return (
                    false,
                    ObtenerErrores(
                        resultadoEliminar));
            }
        }

        foreach (var permiso in permisosValidos)
        {
            var resultadoAgregar =
                await _roleManager.AddClaimAsync(
                    rol,
                    new Claim(
                        RolClaims.Permiso,
                        permiso));

            if (!resultadoAgregar.Succeeded)
            {
                return (
                    false,
                    ObtenerErrores(
                        resultadoAgregar));
            }
        }

        var tieneMarcador =
            claims.Any(c =>
                c.Type ==
                RolClaims.PermisosConfigurados);

        if (!tieneMarcador)
        {
            var resultadoMarcador =
                await _roleManager.AddClaimAsync(
                    rol,
                    new Claim(
                        RolClaims.PermisosConfigurados,
                        "true"));

            if (!resultadoMarcador.Succeeded)
            {
                return (
                    false,
                    ObtenerErrores(
                        resultadoMarcador));
            }
        }

        return (
            true,
            "Permisos actualizados correctamente.");
    }

    private static string ObtenerErrores(
        IdentityResult resultado)
    {
        var errores =
            resultado.Errors
                .Select(e => e.Description)
                .Where(e =>
                    !string.IsNullOrWhiteSpace(e))
                .ToList();

        if (errores.Count == 0)
        {
            return
                "No fue posible completar la operación.";
        }

        return string.Join(
            " ",
            errores);
    }
}