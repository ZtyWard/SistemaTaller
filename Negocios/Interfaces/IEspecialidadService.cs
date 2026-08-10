using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IEspecialidadService
{
    Task<IEnumerable<EspecialidadDto>> ObtenerTodosAsync();

    Task<IEnumerable<EspecialidadDto>> ObtenerActivasAsync();

    Task<EspecialidadDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(EspecialidadGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        EspecialidadGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}