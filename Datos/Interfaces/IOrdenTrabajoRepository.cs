using Datos.Models;

namespace Datos.Interfaces;

public interface IOrdenTrabajoRepository : IRepository<OrdenTrabajo>
{
    Task<OrdenTrabajo?> ObtenerCompletaAsync(int id);

    Task<IEnumerable<OrdenTrabajo>> ObtenerPorEstadoAsync(
        string estado);

    Task<IEnumerable<OrdenTrabajo>> ObtenerAbiertasAsync();
}