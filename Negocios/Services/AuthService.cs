using Datos.Models;
using Microsoft.AspNetCore.Identity;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> ValidarUsuarioAsync(
        LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Usuario) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return null;
        }

        ApplicationUser? usuario = null;

        if (dto.Usuario.Contains("@"))
        {
            usuario = await _userManager
                .FindByEmailAsync(dto.Usuario.Trim());
        }

        if (usuario == null)
        {
            usuario = await _userManager
                .FindByNameAsync(dto.Usuario.Trim());
        }

        if (usuario == null || !usuario.Activo)
        {
            return null;
        }

        var passwordCorrecta =
            await _userManager.CheckPasswordAsync(
                usuario,
                dto.Password);

        if (!passwordCorrecta)
        {
            return null;
        }

        return usuario;
    }
}