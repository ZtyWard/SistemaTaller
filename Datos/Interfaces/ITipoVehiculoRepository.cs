using Datos.Models;

namespace Datos.Interfaces;

public interface ITipoVehiculoRepository : IRepository<TipoVehiculo>
{
    Task<IEnumerable<TipoVehiculo>> ObtenerActivasAsync();
}