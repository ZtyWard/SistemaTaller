using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IPuestoService
{
    Task<IEnumerable<PuestoDto>> ObtenerTodosAsync();

    Task<IEnumerable<PuestoDto>> ObtenerActivosAsync();

    Task<PuestoDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(PuestoGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        PuestoGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}