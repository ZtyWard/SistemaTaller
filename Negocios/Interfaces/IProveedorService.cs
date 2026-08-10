using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IProveedorService
{
    Task<IEnumerable<ProveedorDto>>
        ObtenerTodosAsync();

    Task<IEnumerable<ProveedorDto>>
        ObtenerActivosAsync();

    Task<ProveedorDto?>
        ObtenerPorIdAsync(int id);

    Task CrearAsync(
        ProveedorGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        ProveedorGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}