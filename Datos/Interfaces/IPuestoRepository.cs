using Datos.Models;

namespace Datos.Interfaces;

public interface IPuestoRepository : IRepository<Puesto>
{
    Task<IEnumerable<Puesto>> ObtenerActivosAsync();
}