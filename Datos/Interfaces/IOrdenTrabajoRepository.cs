using Datos.Models;

namespace Datos.Interfaces;

public interface IOrdenTrabajoRepository : IRepository<OrdenTrabajo>
{
    Task<OrdenTrabajo?> ObtenerCompletaAsync(int id);

    Task<IEnumerable<OrdenTrabajo>> ObtenerPorEstadoAsync(
        string estado);

    Task<IEnumerable<OrdenTrabajo>> ObtenerAbiertasAsync();

    Task CambiarEstadoAsync(
        int idOrdenTrabajo,
        string nuevoEstado,
        string usuarioId,
        string? observaciones);

    Task CrearConUsuarioAsync(
        OrdenTrabajo orden,
        string usuarioId);

    Task<bool> ActualizarConUsuarioAsync(
        int idOrdenTrabajo,
        int idCotizacion,
        string? observaciones,
        string usuarioId);
}