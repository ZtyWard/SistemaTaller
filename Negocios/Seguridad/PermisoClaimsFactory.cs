using System.Security.Claims;
using Datos.Models;
using Microsoft.AspNetCore.Identity;

namespace Negocios.Seguridad;

public class PermisoClaimsFactory
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermisoClaimsFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<Claim>> ObtenerClaimsAsync(
        ApplicationUser usuario)
    {
        var claims =
            new List<Claim>();

        var roles =
            await _userManager.GetRolesAsync(usuario);

        var permisosPorRol =
            PermisosPorRol.Obtener();

        var permisos =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        foreach (var nombreRol in roles)
        {
            var rol =
                await _roleManager
                    .FindByNameAsync(nombreRol);

            if (rol == null)
                continue;

            var claimsRol =
                await _roleManager
                    .GetClaimsAsync(rol);

            var rolConfigurado =
                claimsRol.Any(c =>
                    c.Type ==
                    RolClaims.PermisosConfigurados);

            if (rolConfigurado)
            {
                foreach (
                    var claim in claimsRol.Where(c =>
                        c.Type == RolClaims.Permiso))
                {
                    if (!string.IsNullOrWhiteSpace(
                        claim.Value))
                    {
                        permisos.Add(
                            claim.Value);
                    }
                }

                continue;
            }

            // Compatibilidad con roles existentes
            // que todavía no hayan sido migrados
            // a Role Claims.

            if (!permisosPorRol.TryGetValue(
                    nombreRol,
                    out var permisosRol))
            {
                continue;
            }

            foreach (var permiso in permisosRol)
            {
                permisos.Add(permiso);
            }
        }

        foreach (var permiso in permisos)
        {
            claims.Add(
                new Claim(
                    "Permiso",
                    permiso));
        }

        return claims;
    }
}