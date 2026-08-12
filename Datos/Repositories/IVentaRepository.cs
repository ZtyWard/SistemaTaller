using Datos.Models;

namespace Datos.Interfaces;

public interface IVentaRepository : IRepository<Venta>
{
    Task<IEnumerable<Venta>> ObtenerPorClienteAsync(
        int idCliente);

    Task<IEnumerable<Venta>> ObtenerPorEstadoAsync(
        string estado);

    Task<IEnumerable<Venta>> ObtenerRecientesAsync(
        int cantidad);

    Task<Venta?> ObtenerPorIdConDetallesAsync(
        int idVenta);

    Task CompletarVentaAsync(
        int idVenta);
}