using Datos.Models;

namespace Datos.Interfaces;

public interface IReportesRepository
{
    Task<ReporteResumenData>
        ObtenerResumenAsync(
            DateTime desde,
            DateTime hasta);

    Task<IEnumerable<ReporteVentaMesData>>
        ObtenerVentasPorMesAsync(
            DateTime desde,
            DateTime hasta);

    Task<IEnumerable<ReporteOrdenEstadoData>>
        ObtenerOrdenesPorEstadoAsync(
            DateTime desde,
            DateTime hasta);

    Task<IEnumerable<ReporteProductoVendidoData>>
        ObtenerProductosMasVendidosAsync(
            DateTime desde,
            DateTime hasta);

    Task<IEnumerable<ReporteMecanicoData>>
        ObtenerTrabajosPorMecanicoAsync(
            DateTime desde,
            DateTime hasta);

    Task<IEnumerable<ReporteServicioData>>
        ObtenerServiciosMasSolicitadosAsync(
            DateTime desde,
            DateTime hasta);

    Task<IEnumerable<ReporteCompraMesData>>
        ObtenerComprasPorMesAsync(
            DateTime desde,
            DateTime hasta);

    Task<IEnumerable<ReporteStockData>>
        ObtenerStockBajoAsync();

    Task<IEnumerable<ReporteFacturaPendienteData>>
        ObtenerFacturasPendientesAsync();

    Task<IEnumerable<ReporteVehiculoHistorialData>>
        ObtenerHistorialVehiculosAsync(
            string? placa);
}