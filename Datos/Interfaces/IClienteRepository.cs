using Datos.Models;

namespace Datos.Interfaces;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<Cliente?> ObtenerPorCedulaAsync(string cedula);

    Task<Cliente?> ObtenerConVehiculosAsync(int idCliente);

    Task<IEnumerable<Cliente>> ObtenerActivosAsync();
}