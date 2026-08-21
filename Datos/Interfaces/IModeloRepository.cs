using Datos.Models;

namespace Datos.Interfaces;

public interface IModeloRepository : IRepository<Modelo>
{
    Task<IEnumerable<Modelo>> ObtenerActivasAsync();

    Task<IEnumerable<Modelo>> ObtenerActivasPorMarcaAsync(
        int idMarca);
}