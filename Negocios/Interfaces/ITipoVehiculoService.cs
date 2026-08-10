using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface ITipoVehiculoService
{
    Task<IEnumerable<TipoVehiculoDto>> ObtenerTodosAsync();

    Task<IEnumerable<TipoVehiculoDto>> ObtenerActivasAsync();

    Task<TipoVehiculoDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(TipoVehiculoGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        TipoVehiculoGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}