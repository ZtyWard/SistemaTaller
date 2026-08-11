using System.Security.Claims;
using Datos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Negocios.Seguridad;

public class PermisoClaimsFactory
{
    private readonly UserManager<ApplicationUser> _userManager;

    public PermisoClaimsFactory(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<Claim>> ObtenerClaimsAsync(
        ApplicationUser usuario)
    {
        var claims = new List<Claim>();

        var roles =
            await _userManager.GetRolesAsync(usuario);

        var permisosPorRol =
            PermisosPorRol.Obtener();

        var permisos =
            new HashSet<string>();

        foreach (var rol in roles)
        {
            if (!permisosPorRol.TryGetValue(
                    rol,
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