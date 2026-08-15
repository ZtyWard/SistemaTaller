using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioAdministracionDto>> ObtenerTodosAsync();

    Task<UsuarioAdministracionDto?> ObtenerPorIdAsync(
        string id);

    Task<IEnumerable<string>> ObtenerRolesDisponiblesAsync();

    Task<(bool Exitoso, string Mensaje)> CrearAsync(
        UsuarioGuardarDto dto);

    Task<(bool Exitoso, string Mensaje)> ActualizarAsync(
        string id,
        UsuarioGuardarDto dto,
        bool actualizarRoles);

    Task<(bool Exitoso, string Mensaje)> CambiarEstadoAsync(
        string id);
}