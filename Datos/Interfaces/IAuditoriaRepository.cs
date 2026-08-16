using Datos.Models;

namespace Datos.Interfaces;

public interface IAuditoriaRepository
{
    Task<IEnumerable<Auditoria>> ObtenerAsync(
        string? usuarioId = null,
        string? modulo = null,
        string? accion = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null,
        string? registroId = null);
}