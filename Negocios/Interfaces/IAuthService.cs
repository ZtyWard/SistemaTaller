using Datos.Models;
using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IAuthService
{
    Task<ApplicationUser?> ValidarUsuarioAsync(
        LoginDto dto);
}