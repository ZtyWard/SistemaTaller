using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IDiagnosticoService
{
    Task<IEnumerable<DiagnosticoDto>> ObtenerTodosAsync();

    Task<DiagnosticoDto?> ObtenerPorIdAsync(int id);

    Task<IEnumerable<DiagnosticoDto>> ObtenerPorRecepcionAsync(
        int idRecepcion);

    Task CrearAsync(DiagnosticoGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        DiagnosticoGuardarDto dto);
}