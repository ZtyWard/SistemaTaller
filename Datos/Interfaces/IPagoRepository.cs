using Datos.Models;

namespace Datos.Interfaces;

public interface IPagoRepository
    : IRepository<Pago>
{
    Task<IEnumerable<Pago>>
        ObtenerRecientesAsync(int cantidad);

    Task<Pago?>
        ObtenerPorIdConFacturaAsync(int idPago);

    Task<IEnumerable<Pago>>
        ObtenerPorFacturaAsync(int idFactura);

    Task<Pago>
        RegistrarConProcedimientoAsync(
            int idFactura,
            decimal monto,
            string formaPago,
            string? numeroReferencia,
            string? usuarioId,
            string? observaciones);
}