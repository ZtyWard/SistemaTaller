using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IMarcaService
{
    Task<IEnumerable<MarcaDto>> ObtenerTodosAsync();

    Task<IEnumerable<MarcaDto>> ObtenerActivasAsync();

    Task<MarcaDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(MarcaGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        MarcaGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}