using Datos.Models;

namespace Datos.Interfaces;

public interface IServicioRepository : IRepository<Servicio>
{
    Task<IEnumerable<Servicio>> ObtenerActivosAsync();
}