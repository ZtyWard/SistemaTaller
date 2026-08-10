using Datos.Models;

namespace Datos.Interfaces;

public interface IMarcaRepository : IRepository<Marca>
{
    Task<IEnumerable<Marca>> ObtenerActivasAsync();
}