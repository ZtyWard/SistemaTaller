using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IModeloService
{
    Task<IEnumerable<ModeloDto>> ObtenerTodosAsync();

    Task<IEnumerable<ModeloDto>> ObtenerActivasAsync();

    Task<ModeloDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(ModeloGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        ModeloGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}