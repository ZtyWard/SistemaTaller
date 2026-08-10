using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface ITipoCombustibleService
{
    Task<IEnumerable<TipoCombustibleDto>> ObtenerTodosAsync();

    Task<IEnumerable<TipoCombustibleDto>> ObtenerActivasAsync();

    Task<TipoCombustibleDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(TipoCombustibleGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        TipoCombustibleGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}