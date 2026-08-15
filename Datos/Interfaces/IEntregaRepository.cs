using Datos.Models;

namespace Datos.Interfaces;

public interface IEntregaRepository
    : IRepository<Entrega>
{
    Task<Entrega?> ObtenerCompletaAsync(
        int idEntrega);

    Task<Entrega?> ObtenerPorOrdenTrabajoAsync(
        int idOrdenTrabajo);

    Task<bool> ExisteParaOrdenTrabajoAsync(
        int idOrdenTrabajo);
}