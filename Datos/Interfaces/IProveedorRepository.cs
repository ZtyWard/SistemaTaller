using Datos.Models;

namespace Datos.Interfaces;

public interface IProveedorRepository : IRepository<Proveedor>
{
    Task<IEnumerable<Proveedor>> ObtenerActivosAsync();

    Task<Proveedor?> ObtenerPorCedulaJuridicaAsync(
        string cedulaJuridica);
}