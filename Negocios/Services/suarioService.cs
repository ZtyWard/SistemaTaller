using Datos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsuarioService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<UsuarioAdministracionDto>>
        ObtenerTodosAsync()
    {
        var usuarios = await _userManager.Users
            .OrderBy(u => u.NombreCompleto)
            .ToListAsync();

        var resultado = new List<UsuarioAdministracionDto>();

        foreach (var usuario in usuarios)
        {
            var roles = await _userManager.GetRolesAsync(usuario);

            resultado.Add(MapearDto(usuario, roles));
        }

        return resultado;
    }

    public async Task<UsuarioAdministracionDto?>
        ObtenerPorIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        var usuario = await _userManager.FindByIdAsync(id);

        if (usuario == null)
            return null;

        var roles = await _userManager.GetRolesAsync(usuario);

        return MapearDto(usuario, roles);
    }

    public async Task<IEnumerable<string>>
        ObtenerRolesDisponiblesAsync()
    {
        return await _roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => r.Name!)
            .ToListAsync();
    }

    public async Task<(bool Exitoso, string Mensaje)>
        CrearAsync(UsuarioGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Usuario))
            return (false, "El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return (false, "El correo electrónico es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.NombreCompleto))
            return (false, "El nombre completo es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            return (false, "La contraseña es obligatoria al crear un usuario.");

        if (dto.Password != dto.ConfirmarPassword)
            return (false, "Las contraseñas no coinciden.");

        var usuarioExistente =
            await _userManager.FindByNameAsync(dto.Usuario.Trim());

        if (usuarioExistente != null)
            return (false, "Ya existe un usuario con ese nombre.");

        var correoExistente =
            await _userManager.FindByEmailAsync(dto.Email.Trim());

        if (correoExistente != null)
            return (false, "Ya existe un usuario con ese correo electrónico.");

        var usuario = new ApplicationUser
        {
            UserName = dto.Usuario.Trim(),
            Email = dto.Email.Trim(),
            NombreCompleto = dto.NombreCompleto.Trim(),
            Activo = dto.Activo,
            EmailConfirmed = true
        };

        var resultadoCrear =
            await _userManager.CreateAsync(
                usuario,
                dto.Password);

        if (!resultadoCrear.Succeeded)
        {
            return (
                false,
                ObtenerErrores(resultadoCrear));
        }

        var rolesValidos = await ObtenerRolesValidosAsync(dto.Roles);

        if (rolesValidos.Count > 0)
        {
            var resultadoRoles =
                await _userManager.AddToRolesAsync(
                    usuario,
                    rolesValidos);

            if (!resultadoRoles.Succeeded)
            {
                await _userManager.DeleteAsync(usuario);

                return (
                    false,
                    ObtenerErrores(resultadoRoles));
            }
        }

        return (
            true,
            "Usuario creado correctamente.");
    }

    public async Task<(bool Exitoso, string Mensaje)>
        ActualizarAsync(
            string id,
            UsuarioGuardarDto dto,
            bool actualizarRoles)
    {
        if (string.IsNullOrWhiteSpace(id))
            return (false, "El usuario indicado no es válido.");

        var usuario =
            await _userManager.FindByIdAsync(id);

        if (usuario == null)
            return (false, "El usuario no existe.");

        if (string.IsNullOrWhiteSpace(dto.Usuario))
            return (false, "El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return (false, "El correo electrónico es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.NombreCompleto))
            return (false, "El nombre completo es obligatorio.");

        var otroUsuario =
            await _userManager.FindByNameAsync(
                dto.Usuario.Trim());

        if (otroUsuario != null &&
            otroUsuario.Id != usuario.Id)
        {
            return (
                false,
                "Ya existe otro usuario con ese nombre.");
        }

        var otroCorreo =
            await _userManager.FindByEmailAsync(
                dto.Email.Trim());

        if (otroCorreo != null &&
            otroCorreo.Id != usuario.Id)
        {
            return (
                false,
                "Ya existe otro usuario con ese correo electrónico.");
        }

        usuario.UserName = dto.Usuario.Trim();
        usuario.Email = dto.Email.Trim();
        usuario.NombreCompleto = dto.NombreCompleto.Trim();
        usuario.Activo = dto.Activo;
        usuario.EmailConfirmed = true;

        var resultadoActualizar =
            await _userManager.UpdateAsync(usuario);

        if (!resultadoActualizar.Succeeded)
        {
            return (
                false,
                ObtenerErrores(resultadoActualizar));
        }

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            if (dto.Password != dto.ConfirmarPassword)
            {
                return (
                    false,
                    "Las contraseñas no coinciden.");
            }

            var token =
                await _userManager
                    .GeneratePasswordResetTokenAsync(usuario);

            var resultadoPassword =
                await _userManager.ResetPasswordAsync(
                    usuario,
                    token,
                    dto.Password);

            if (!resultadoPassword.Succeeded)
            {
                return (
                    false,
                    ObtenerErrores(resultadoPassword));
            }
        }

        if (actualizarRoles)
        {
            var rolesActuales =
                await _userManager.GetRolesAsync(usuario);

            if (rolesActuales.Count > 0)
            {
                var resultadoEliminarRoles =
                    await _userManager.RemoveFromRolesAsync(
                        usuario,
                        rolesActuales);

                if (!resultadoEliminarRoles.Succeeded)
                {
                    return (
                        false,
                        ObtenerErrores(resultadoEliminarRoles));
                }
            }

            var rolesValidos =
                await ObtenerRolesValidosAsync(dto.Roles);

            if (rolesValidos.Count > 0)
            {
                var resultadoAgregarRoles =
                    await _userManager.AddToRolesAsync(
                        usuario,
                        rolesValidos);

                if (!resultadoAgregarRoles.Succeeded)
                {
                    return (
                        false,
                        ObtenerErrores(resultadoAgregarRoles));
                }
            }
        }

        return (
            true,
            "Usuario actualizado correctamente.");
    }

    public async Task<(bool Exitoso, string Mensaje)>
        CambiarEstadoAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return (false, "El usuario indicado no es válido.");

        var usuario =
            await _userManager.FindByIdAsync(id);

        if (usuario == null)
            return (false, "El usuario no existe.");

        usuario.Activo = !usuario.Activo;

        var resultado =
            await _userManager.UpdateAsync(usuario);

        if (!resultado.Succeeded)
        {
            return (
                false,
                ObtenerErrores(resultado));
        }

        var estado =
            usuario.Activo
                ? "activado"
                : "desactivado";

        return (
            true,
            $"Usuario {estado} correctamente.");
    }

    private async Task<List<string>>
        ObtenerRolesValidosAsync(
            IEnumerable<string>? roles)
    {
        if (roles == null)
            return new List<string>();

        var rolesSolicitados =
            roles
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

        var rolesDisponibles =
            await _roleManager.Roles
                .Select(r => r.Name!)
                .ToListAsync();

        return rolesSolicitados
            .Where(r =>
                rolesDisponibles.Any(
                    rd => string.Equals(
                        rd,
                        r,
                        StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    private static UsuarioAdministracionDto
        MapearDto(
            ApplicationUser usuario,
            IEnumerable<string> roles)
    {
        return new UsuarioAdministracionDto
        {
            Id = usuario.Id,
            Usuario = usuario.UserName ?? string.Empty,
            Email = usuario.Email ?? string.Empty,
            NombreCompleto = usuario.NombreCompleto,
            Activo = usuario.Activo,
            Roles = roles.ToList()
        };
    }

    private static string
        ObtenerErrores(
            IdentityResult resultado)
    {
        var errores =
            resultado.Errors
                .Select(e => e.Description)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToList();

        if (errores.Count == 0)
            return "No fue posible completar la operación.";

        return string.Join(" ", errores);
    }
}