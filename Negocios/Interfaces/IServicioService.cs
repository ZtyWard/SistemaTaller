using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IServicioService
{
    Task<IEnumerable<ServicioDto>> ObtenerTodosAsync();

    Task<IEnumerable<ServicioDto>> ObtenerActivosAsync();

    Task<ServicioDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(ServicioGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        ServicioGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}