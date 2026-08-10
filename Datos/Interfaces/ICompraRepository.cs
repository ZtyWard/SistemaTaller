using Datos.Models;

namespace Datos.Interfaces;

public interface ICompraRepository : IRepository<Compra>
{
    Task<IEnumerable<Compra>> ObtenerPorProveedorAsync(
        int idProveedor);

    Task<IEnumerable<Compra>> ObtenerPorEstadoAsync(
        string estado);

    Task<IEnumerable<Compra>> ObtenerRecientesAsync(
        int cantidad);
}