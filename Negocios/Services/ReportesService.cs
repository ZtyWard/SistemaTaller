using Datos.Interfaces;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class ReportesService : IReportesService
{
    private readonly IReportesRepository _repository;

    public ReportesService(
        IReportesRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReportesDto> ObtenerAsync(
        DateTime? desde,
        DateTime? hasta,
        string? placa)
    {
        // =====================================================
        // FECHAS
        // =====================================================

        var fechaDesde =
            (desde ?? DateTime.Today.AddMonths(-6))
            .Date;

        var fechaHasta =
            (hasta ?? DateTime.Today)
            .Date;

        if (fechaDesde > fechaHasta)
        {
            throw new ArgumentException(
                "La fecha desde no puede ser mayor que la fecha hasta.");
        }

        // Fecha final exclusiva para SQL.
        var fechaHastaExclusiva =
            fechaHasta.AddDays(1);

        // =====================================================
        // RESUMEN
        // =====================================================

        var resumen =
            await _repository.ObtenerResumenAsync(
                fechaDesde,
                fechaHastaExclusiva);

        // =====================================================
        // VENTAS
        // =====================================================

        var ventas =
            await _repository.ObtenerVentasPorMesAsync(
                fechaDesde,
                fechaHastaExclusiva);

        // =====================================================
        // ÓRDENES POR ESTADO
        // =====================================================

        var ordenes =
            await _repository.ObtenerOrdenesPorEstadoAsync(
                fechaDesde,
                fechaHastaExclusiva);

        // =====================================================
        // PRODUCTOS MÁS VENDIDOS
        // =====================================================

        var productos =
            await _repository.ObtenerProductosMasVendidosAsync(
                fechaDesde,
                fechaHastaExclusiva);

        // =====================================================
        // TRABAJOS POR MECÁNICO
        // =====================================================

        var mecanicos =
            await _repository.ObtenerTrabajosPorMecanicoAsync(
                fechaDesde,
                fechaHastaExclusiva);

        // =====================================================
        // SERVICIOS MÁS SOLICITADOS
        // =====================================================

        var servicios =
            await _repository.ObtenerServiciosMasSolicitadosAsync(
                fechaDesde,
                fechaHastaExclusiva);

        // =====================================================
        // COMPRAS
        // =====================================================

        var compras =
            await _repository.ObtenerComprasPorMesAsync(
                fechaDesde,
                fechaHastaExclusiva);

        // =====================================================
        // STOCK BAJO
        // =====================================================

        var stock =
            await _repository.ObtenerStockBajoAsync();

        // =====================================================
        // FACTURAS PENDIENTES
        // =====================================================

        var facturas =
            await _repository.ObtenerFacturasPendientesAsync();

        // =====================================================
        // HISTORIAL DE VEHÍCULO
        // =====================================================

        var historial =
            await _repository.ObtenerHistorialVehiculosAsync(
                string.IsNullOrWhiteSpace(placa)
                    ? null
                    : placa.Trim());

        // =====================================================
        // CONSTRUIR DTO
        // =====================================================

        return new ReportesDto
        {
            Desde = fechaDesde,

            Hasta = fechaHasta,

            Placa =
                string.IsNullOrWhiteSpace(placa)
                    ? null
                    : placa.Trim(),

            // =================================================
            // KPIs
            // =================================================

            Ingresos =
                resumen.Ingresos,

            Compras =
                resumen.Compras,

            Utilidad =
                resumen.Utilidad,

            FacturasPendientes =
                resumen.FacturasPendientes,

            OrdenesPendientes =
                resumen.OrdenesPendientes,

            OrdenesAtrasadas =
                resumen.OrdenesAtrasadas,

            OrdenesFinalizadas =
                resumen.OrdenesFinalizadas,

            TrabajosPeriodo =
                resumen.TrabajosPeriodo,

            // =================================================
            // VENTAS
            // =================================================

            VentasPorMes =
                ventas
                    .Select(x =>
                        new ReporteVentaMesDto
                        {
                            Mes =
                                x.Mes,

                            Total =
                                x.Total,

                            Cantidad =
                                x.Cantidad
                        })
                    .ToList(),

            // =================================================
            // ÓRDENES
            // =================================================

            OrdenesPorEstado =
                ordenes
                    .Select(x =>
                        new ReporteOrdenEstadoDto
                        {
                            Estado =
                                x.Estado,

                            Cantidad =
                                x.Cantidad
                        })
                    .ToList(),

            // =================================================
            // PRODUCTOS
            // =================================================

            ProductosMasVendidos =
                productos
                    .Select(x =>
                        new ReporteProductoVendidoDto
                        {
                            Nombre =
                                x.Nombre,

                            Cantidad =
                                x.Cantidad,

                            Total =
                                x.Total
                        })
                    .ToList(),

            // =================================================
            // MECÁNICOS
            // =================================================

            TrabajosPorMecanico =
                mecanicos
                    .Select(x =>
                        new ReporteMecanicoDto
                        {
                            Mecanico =
                                x.Mecanico,

                            Trabajos =
                                x.Trabajos,

                            Horas =
                                x.Horas
                        })
                    .ToList(),

            // =================================================
            // SERVICIOS
            // =================================================

            ServiciosMasSolicitados =
                servicios
                    .Select(x =>
                        new ReporteServicioDto
                        {
                            Servicio =
                                x.Servicio,

                            Cantidad =
                                x.Cantidad
                        })
                    .ToList(),

            // =================================================
            // COMPRAS
            // =================================================

            ComprasPorMes =
                compras
                    .Select(x =>
                        new ReporteCompraMesDto
                        {
                            Mes =
                                x.Mes,

                            Total =
                                x.Total,

                            Cantidad =
                                x.Cantidad
                        })
                    .ToList(),

            // =================================================
            // STOCK
            // =================================================

            StockBajo =
                stock
                    .Select(x =>
                        new ReporteStockDto
                        {
                            Codigo =
                                x.Codigo,

                            Nombre =
                                x.Nombre,

                            Stock =
                                x.Stock,

                            StockMinimo =
                                x.StockMinimo
                        })
                    .ToList(),

            // =================================================
            // FACTURAS
            // =================================================

            FacturasPendientesLista =
                facturas
                    .Select(x =>
                        new ReporteFacturaPendienteDto
                        {
                            IdFactura =
                                x.IdFactura,

                            NumeroFactura =
                                x.NumeroFactura,

                            FechaEmision =
                                x.FechaEmision,

                            Cliente =
                                x.Cliente,

                            Total =
                                x.Total,

                            TotalPagado =
                                x.TotalPagado,

                            SaldoPendiente =
                                x.SaldoPendiente,

                            Estado =
                                x.Estado
                        })
                    .ToList(),

            // =================================================
            // HISTORIAL
            // =================================================

            HistorialVehiculos =
                historial
                    .Select(x =>
                        new ReporteVehiculoHistorialDto
                        {
                            Placa =
                                x.Placa,

                            Cliente =
                                x.Cliente,

                            Fecha =
                                x.Fecha,

                            IdOrdenTrabajo =
                                x.IdOrdenTrabajo,

                            Estado =
                                x.Estado,

                            Observaciones =
                                x.Observaciones
                        })
                    .ToList()
        };
    }
}