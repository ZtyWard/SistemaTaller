using Datos.Models;

namespace Datos.Interfaces;

public interface IRecepcionRepository : IRepository<Recepcion>
{
    Task<Recepcion?> ObtenerCompletaAsync(int idRecepcion);

    Task<IEnumerable<Recepcion>> ObtenerPorVehiculoAsync(int idVehiculo);

    Task<IEnumerable<Recepcion>> ObtenerAbiertasAsync();
}