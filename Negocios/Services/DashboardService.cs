using Datos.Interfaces;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class DashboardService : IDashboardService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IOrdenTrabajoRepository _ordenTrabajoRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IFacturaRepository _facturaRepository;
    private readonly IVentaRepository _ventaRepository;

    public DashboardService(
        IClienteRepository clienteRepository,
        IVehiculoRepository vehiculoRepository,
        IOrdenTrabajoRepository ordenTrabajoRepository,
        IProductoRepository productoRepository,
        IFacturaRepository facturaRepository,
        IVentaRepository ventaRepository)
    {
        _clienteRepository = clienteRepository;
        _vehiculoRepository = vehiculoRepository;
        _ordenTrabajoRepository = ordenTrabajoRepository;
        _productoRepository = productoRepository;
        _facturaRepository = facturaRepository;
        _ventaRepository = ventaRepository;
    }

    public async Task<DashboardDto> ObtenerDashboardAsync(
        string rolUsuario)
    {
        var clientes =
            await _clienteRepository.ObtenerTodosAsync();

        var vehiculos =
            await _vehiculoRepository.ObtenerTodosAsync();

        var ordenes =
            await _ordenTrabajoRepository.ObtenerTodosAsync();

        var productos =
            await _productoRepository.ObtenerTodosAsync();

        var facturas =
            await _facturaRepository.ObtenerTodosAsync();

        var ventas =
            await _ventaRepository.ObtenerTodosAsync();

        var ahora = DateTime.Now;

        var inicioMes =
            new DateTime(
                ahora.Year,
                ahora.Month,
                1);

        var inicioPeriodo =
            inicioMes.AddMonths(-5);

        var clientesActivos =
            clientes.Count(c => c.Activo);

        var vehiculosActivos =
            vehiculos.Count(v => v.Activo);

        var ordenesAbiertas =
            ordenes.Count(o =>
                !string.Equals(
                    o.Estado,
                    "Finalizada",
                    StringComparison.OrdinalIgnoreCase)
                &&
                !string.Equals(
                    o.Estado,
                    "Cancelada",
                    StringComparison.OrdinalIgnoreCase));

        var productosStockBajo =
            productos.Count(p =>
                p.Activo &&
                p.Stock <= p.StockMinimo);

        var facturasPendientes =
            facturas.Count(f =>
                string.Equals(
                    f.Estado,
                    "Pendiente",
                    StringComparison.OrdinalIgnoreCase));

        var ventasMes =
            ventas
                .Where(v =>
                    v.FechaVenta >= inicioMes &&
                    v.FechaVenta < inicioMes.AddMonths(1) &&
                    !string.Equals(
                        v.Estado,
                        "Anulada",
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

        var ventasMesActual =
            ventasMes.Sum(v => v.Total);

        var ventasMesActualCantidad =
            ventasMes.Count;

        var ventasPorMes =
            new List<DashboardVentaMesDto>();

        for (int i = 0; i < 6; i++)
        {
            var fecha =
                inicioPeriodo.AddMonths(i);

            var siguienteMes =
                fecha.AddMonths(1);

            var ventasDelMes =
                ventas
                    .Where(v =>
                        v.FechaVenta >= fecha &&
                        v.FechaVenta < siguienteMes &&
                        !string.Equals(
                            v.Estado,
                            "Anulada",
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();

            ventasPorMes.Add(
                new DashboardVentaMesDto
                {
                    Mes = fecha.ToString("MMM yyyy"),
                    Total = ventasDelMes.Sum(v => v.Total),
                    Cantidad = ventasDelMes.Count
                });
        }

        var ordenesPorEstado =
            ordenes
                .GroupBy(o =>
                    string.IsNullOrWhiteSpace(o.Estado)
                        ? "Sin estado"
                        : o.Estado)
                .OrderByDescending(g => g.Count())
                .Select(g =>
                    new DashboardOrdenEstadoDto
                    {
                        Estado = g.Key,
                        Cantidad = g.Count()
                    })
                .ToList();

        var productosStock =
            productos
                .Where(p =>
                    p.Activo &&
                    p.Stock <= p.StockMinimo)
                .OrderBy(p => p.Stock)
                .ThenBy(p => p.Nombre)
                .Take(8)
                .Select(p =>
                    new DashboardProductoStockDto
                    {
                        Codigo = p.Codigo,
                        Nombre = p.Nombre,
                        Stock = p.Stock,
                        StockMinimo = p.StockMinimo
                    })
                .ToList();

        return new DashboardDto
        {
            ClientesActivos = clientesActivos,
            VehiculosActivos = vehiculosActivos,
            OrdenesAbiertas = ordenesAbiertas,
            ProductosStockBajo = productosStockBajo,
            FacturasPendientes = facturasPendientes,
            VentasMesActual = ventasMesActual,
            VentasMesActualCantidad =
                ventasMesActualCantidad,
            RolUsuario = rolUsuario,
            VentasPorMes = ventasPorMes,
            OrdenesPorEstado = ordenesPorEstado,
            ProductosStock = productosStock
        };
    }
}