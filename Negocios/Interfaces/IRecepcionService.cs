using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IRecepcionService
{
    Task<IEnumerable<RecepcionDto>> ObtenerTodasAsync();

    Task<IEnumerable<RecepcionDto>> ObtenerAbiertasAsync();

    Task<IEnumerable<RecepcionDto>> ObtenerPorVehiculoAsync(int idVehiculo);

    Task<RecepcionDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(RecepcionGuardarDto dto);

    Task<bool> ActualizarEstadoAsync(int id, string estado);
}