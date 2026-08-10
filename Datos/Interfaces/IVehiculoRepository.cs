using Datos.Models;

namespace Datos.Interfaces;

public interface IVehiculoRepository : IRepository<Vehiculo>
{
    Task<Vehiculo?> ObtenerPorPlacaAsync(string placa);

    Task<Vehiculo?> ObtenerCompletoAsync(int idVehiculo);

    Task<IEnumerable<Vehiculo>> ObtenerPorClienteAsync(int idCliente);

    Task<IEnumerable<Vehiculo>> ObtenerActivosAsync();
}