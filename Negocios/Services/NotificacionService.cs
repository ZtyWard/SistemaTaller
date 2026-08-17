using Datos.Interfaces;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class NotificacionService : INotificacionService
{
    private readonly ICotizacionRepository _cotizacionRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly ICitaRepository _citaRepository;
    private readonly IFacturaRepository _facturaRepository;
    private readonly IGarantiaRepository _garantiaRepository;
    private readonly IOrdenTrabajoRepository _ordenTrabajoRepository;
    private readonly IEntregaRepository _entregaRepository;

    public NotificacionService(
        ICotizacionRepository cotizacionRepository,
        IProductoRepository productoRepository,
        ICitaRepository citaRepository,
        IFacturaRepository facturaRepository,
        IGarantiaRepository garantiaRepository,
        IOrdenTrabajoRepository ordenTrabajoRepository,
        IEntregaRepository entregaRepository)
    {
        _cotizacionRepository = cotizacionRepository;
        _productoRepository = productoRepository;
        _citaRepository = citaRepository;
        _facturaRepository = facturaRepository;
        _garantiaRepository = garantiaRepository;
        _ordenTrabajoRepository = ordenTrabajoRepository;
        _entregaRepository = entregaRepository;
    }

    public async Task<IEnumerable<NotificacionDto>> ObtenerTodasAsync()
    {
        var notificaciones = new List<NotificacionDto>();

        var ahora = DateTime.Now;

        // =====================================================
        // 1. COTIZACIONES PENDIENTES
        // =====================================================

        var cotizacionesPendientes =
            await _cotizacionRepository.ObtenerPendientesAsync();

        if (cotizacionesPendientes.Any())
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "Cotizaciones",
                Titulo = "Cotizaciones pendientes",
                Mensaje =
                    $"Hay {cotizacionesPendientes.Count()} cotización(es) pendientes de aprobación.",
                Severidad = "warning",
                Icono = "▤",
                Fecha = ahora,
                Url = "/Cotizacion"
            });
        }

        // =====================================================
        // 2. VEHÍCULOS LISTOS PARA ENTREGA
        // =====================================================

        var ordenesFinalizadas =
            await _ordenTrabajoRepository
                .ObtenerPorEstadoAsync("Finalizada");

        var vehiculosListos = 0;

        foreach (var orden in ordenesFinalizadas)
        {
            var entrega =
                await _entregaRepository
                    .ExisteParaOrdenTrabajoAsync(
                        orden.IdOrdenTrabajo);

            if (!entrega)
            {
                vehiculosListos++;
            }
        }

        if (vehiculosListos > 0)
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "Entregas",
                Titulo = "Vehículos listos para entrega",
                Mensaje =
                    $"Hay {vehiculosListos} vehículo(s) con la orden finalizada y pendientes de entrega.",
                Severidad = "info",
                Icono = "↗",
                Fecha = ahora,
                Url = "/Entrega"
            });
        }

        // =====================================================
        // 3. PRODUCTOS BAJO MÍNIMO
        // =====================================================

        var productosStockBajo =
            await _productoRepository
                .ObtenerStockBajoAsync();

        // Los repuestos se muestran en su propia alerta
        // para evitar duplicar la misma notificación.
        var repuestosStockBajo =
            productosStockBajo
                .Where(x =>
                    x.CategoriaProducto != null &&
                    x.CategoriaProducto.Nombre
                        .Contains(
                            "repuesto",
                            StringComparison.OrdinalIgnoreCase))
                .ToList();

        var productosGeneralesStockBajo =
            productosStockBajo
                .Where(x =>
                    x.CategoriaProducto == null ||
                    !x.CategoriaProducto.Nombre
                        .Contains(
                            "repuesto",
                            StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (productosGeneralesStockBajo.Any())
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "Inventario",
                Titulo = "Stock bajo",
                Mensaje =
                    $"{productosGeneralesStockBajo.Count} producto(s) están en su mínimo o por debajo.",
                Severidad = "danger",
                Icono = "◇",
                Fecha = ahora,
                Url = "/Producto"
            });
        }

        // =====================================================
        // 4. CITAS PRÓXIMAS
        // =====================================================

        var limiteCitas =
            ahora.AddDays(2);

        var citasProximas =
            await _citaRepository
                .ObtenerAgendaAsync(
                    ahora,
                    limiteCitas);

        var citasValidas =
            citasProximas
                .Where(x =>
                    !string.Equals(
                        x.Estado,
                        "Cancelada",
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (citasValidas.Any())
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "Citas",
                Titulo = "Citas próximas",
                Mensaje =
                    $"Hay {citasValidas.Count} cita(s) programadas durante las próximas 48 horas.",
                Severidad = "info",
                Icono = "◷",
                Fecha = ahora,
                Url = "/Cita"
            });
        }

        // =====================================================
        // 5. FACTURAS PENDIENTES
        // =====================================================

        var facturasPendientes =
            (await _facturaRepository
                .ObtenerPendientesAsync())
            .ToList();

        if (facturasPendientes.Any())
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "Facturacion",
                Titulo = "Facturas pendientes",
                Mensaje =
                    $"Hay {facturasPendientes.Count} factura(s) pendientes o parcialmente pagadas.",
                Severidad = "warning",
                Icono = "▤",
                Fecha = ahora,
                Url = "/Factura"
            });
        }

        // =====================================================
        // 6. SALDOS PENDIENTES
        // =====================================================

        var saldoPendiente =
            facturasPendientes.Sum(f =>
                Math.Max(
                    0,
                    f.Total -
                    f.Pagos.Sum(p => p.Monto)));

        if (saldoPendiente > 0.01m)
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "Saldos",
                Titulo = "Saldos pendientes",
                Mensaje =
                    $"El sistema registra saldos pendientes por ₡{saldoPendiente:N2}.",
                Severidad = "danger",
                Icono = "◉",
                Fecha = ahora,
                Url = "/Pago"
            });
        }

        // =====================================================
        // 7. GARANTÍAS PRÓXIMAS A VENCER
        // =====================================================

        var garantiasPorVencer =
            await _garantiaRepository
                .ObtenerPorVencerAsync(7);

        if (garantiasPorVencer.Any())
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "Garantias",
                Titulo = "Garantías próximas a vencer",
                Mensaje =
                    $"Hay {garantiasPorVencer.Count()} garantía(s) que vencen durante los próximos 7 días.",
                Severidad = "warning",
                Icono = "◇",
                Fecha = ahora,
                Url = "/Garantia"
            });
        }

        // =====================================================
        // 8. ÓRDENES DE TRABAJO ATRASADAS
        // =====================================================

        var ordenesAbiertas =
            await _ordenTrabajoRepository
                .ObtenerAbiertasAsync();

        var ordenesAtrasadas =
            ordenesAbiertas
                .Where(x =>
                    x.FechaFin.HasValue &&
                    x.FechaFin.Value < ahora)
                .ToList();

        if (ordenesAtrasadas.Any())
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "OrdenesAtrasadas",
                Titulo = "Órdenes de trabajo atrasadas",
                Mensaje =
                    $"Hay {ordenesAtrasadas.Count} orden(es) de trabajo fuera de su fecha prevista.",
                Severidad = "danger",
                Icono = "▣",
                Fecha = ahora,
                Url = "/OrdenTrabajo"
            });
        }

        // =====================================================
        // 9. REPUESTOS PENDIENTES
        // =====================================================

        if (repuestosStockBajo.Any())
        {
            notificaciones.Add(new NotificacionDto
            {
                Tipo = "Repuestos",
                Titulo = "Repuestos pendientes de reposición",
                Mensaje =
                    $"Hay {repuestosStockBajo.Count} repuesto(s) con stock insuficiente.",
                Severidad = "warning",
                Icono = "◇",
                Fecha = ahora,
                Url = "/Producto"
            });
        }

        return notificaciones
            .OrderByDescending(x =>
                SeveridadOrden(x.Severidad))
            .ThenByDescending(x => x.Fecha)
            .ToList();
    }

    private static int SeveridadOrden(string severidad)
    {
        return severidad switch
        {
            "danger" => 3,
            "warning" => 2,
            "info" => 1,
            _ => 0
        };
    }
}