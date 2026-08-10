using Datos.Models;

namespace Datos.Interfaces;

public interface IModeloRepository : IRepository<Modelo>
{
    Task<IEnumerable<Modelo>> ObtenerActivasAsync();
}