using Datos.Models;

namespace Datos.Interfaces;

public interface IFacturaRepository
    : IRepository<Factura>
{
    Task<IEnumerable<Factura>>
        ObtenerRecientesAsync(int cantidad);

    Task<Factura?>
        ObtenerPorIdConRelacionesAsync(int idFactura);

    Task<Factura?>
        ObtenerPorNumeroAsync(string numeroFactura);

    Task<IEnumerable<Factura>>
        ObtenerPendientesAsync();

    Task AnularAsync(int idFactura);
}