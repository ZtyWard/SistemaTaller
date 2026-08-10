using Datos.Models;

namespace Datos.Interfaces;

public interface ITipoCombustibleRepository : IRepository<TipoCombustible>
{
    Task<IEnumerable<TipoCombustible>> ObtenerActivasAsync();
}