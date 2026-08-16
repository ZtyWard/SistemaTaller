using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IAuditoriaService
{
    Task<IEnumerable<AuditoriaDto>> ObtenerAsync(
        string? usuarioId = null,
        string? modulo = null,
        string? accion = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null,
        string? registroId = null);
}