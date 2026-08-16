using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class ReportesRepository : IReportesRepository
{
    private readonly SistemaTallerDbContext _context;

    public ReportesRepository(
        SistemaTallerDbContext context)
    {
        _context = context;
    }

    // =====================================================
    // RESUMEN
    // =====================================================

    public async Task<ReporteResumenData>
        ObtenerResumenAsync(
            DateTime desde,
            DateTime hasta)
    {
        var resultado =
            await _context.Database
                .SqlQuery<ReporteResumenData>($"""
                    SELECT
                        CAST(
                            ISNULL(
                                (
                                    SELECT SUM(v.Total)
                                    FROM dbo.Venta v
                                    WHERE v.FechaVenta >= {desde}
                                      AND v.FechaVenta < {hasta}
                                      AND v.Estado <> 'Anulada'
                                ),
                                0
                            )
                            AS decimal(18,2)
                        ) AS Ingresos,

                        CAST(
                            ISNULL(
                                (
                                    SELECT SUM(c.Total)
                                    FROM dbo.Compra c
                                    WHERE c.FechaCompra >= {desde}
                                      AND c.FechaCompra < {hasta}
                                      AND c.Estado <> 'Anulada'
                                ),
                                0
                            )
                            AS decimal(18,2)
                        ) AS Compras,

                        CAST(
                            ISNULL(
                                (
                                    SELECT SUM(
                                        v.Total -
                                        ISNULL(costos.Costo, 0)
                                    )
                                    FROM dbo.Venta v

                                    OUTER APPLY
                                    (
                                        SELECT
                                            SUM(
                                                dv.Cantidad *
                                                p.PrecioCompra
                                            ) AS Costo
                                        FROM dbo.DetalleVenta dv
                                        INNER JOIN dbo.Producto p
                                            ON p.IdProducto =
                                               dv.IdProducto
                                        WHERE dv.IdVenta =
                                              v.IdVenta
                                    ) costos

                                    WHERE v.FechaVenta >= {desde}
                                      AND v.FechaVenta < {hasta}
                                      AND v.Estado <> 'Anulada'
                                ),
                                0
                            )
                            AS decimal(18,2)
                        ) AS Utilidad,

                        (
                            SELECT COUNT(*)
                            FROM dbo.Factura f
                            WHERE f.Estado IN
                            (
                                'Pendiente',
                                'Parcialmente pagada'
                            )
                        ) AS FacturasPendientes,

                        (
                            SELECT COUNT(*)
                            FROM dbo.OrdenTrabajo o
                            WHERE o.FechaInicio >= {desde}
                              AND o.FechaInicio < {hasta}
                              AND o.Estado NOT IN
                              (
                                  'Finalizada',
                                  'Cancelada'
                              )
                        ) AS OrdenesPendientes,

                        (
                            SELECT COUNT(*)
                            FROM dbo.OrdenTrabajo o
                            WHERE o.FechaInicio >= {desde}
                              AND o.FechaInicio < {hasta}
                              AND o.Estado NOT IN
                              (
                                  'Finalizada',
                                  'Cancelada'
                              )
                              AND o.FechaInicio <
                                  DATEADD(
                                      DAY,
                                      -7,
                                      {hasta}
                                  )
                        ) AS OrdenesAtrasadas,

                        (
                            SELECT COUNT(*)
                            FROM dbo.OrdenTrabajo o
                            WHERE o.FechaInicio >= {desde}
                              AND o.FechaInicio < {hasta}
                              AND o.Estado = 'Finalizada'
                        ) AS OrdenesFinalizadas,

                        (
                            SELECT COUNT(*)
                            FROM dbo.OrdenTrabajo o
                            WHERE o.FechaInicio >= {desde}
                              AND o.FechaInicio < {hasta}
                        ) AS TrabajosPeriodo
                    """)
                .SingleAsync();

        return resultado;
    }

    // =====================================================
    // VENTAS POR MES
    // =====================================================

    public async Task<IEnumerable<ReporteVentaMesData>>
        ObtenerVentasPorMesAsync(
            DateTime desde,
            DateTime hasta)
    {
        return await _context.Database
            .SqlQuery<ReporteVentaMesData>($"""
                SELECT
                    FORMAT(
                        v.FechaVenta,
                        'yyyy-MM'
                    ) AS Mes,

                    CAST(
                        ISNULL(
                            SUM(v.Total),
                            0
                        )
                        AS decimal(18,2)
                    ) AS Total,

                    COUNT(*) AS Cantidad

                FROM dbo.Venta v

                WHERE v.FechaVenta >= {desde}
                  AND v.FechaVenta < {hasta}
                  AND v.Estado <> 'Anulada'

                GROUP BY
                    FORMAT(
                        v.FechaVenta,
                        'yyyy-MM'
                    )

                ORDER BY
                    Mes
                """)
            .ToListAsync();
    }

    // =====================================================
    // ÓRDENES POR ESTADO
    // =====================================================

    public async Task<IEnumerable<ReporteOrdenEstadoData>>
        ObtenerOrdenesPorEstadoAsync(
            DateTime desde,
            DateTime hasta)
    {
        return await _context.Database
            .SqlQuery<ReporteOrdenEstadoData>($"""
                SELECT
                    ISNULL(
                        o.Estado,
                        'Sin estado'
                    ) AS Estado,

                    COUNT(*) AS Cantidad

                FROM dbo.OrdenTrabajo o

                WHERE o.FechaInicio >= {desde}
                  AND o.FechaInicio < {hasta}

                GROUP BY
                    o.Estado

                ORDER BY
                    Cantidad DESC
                """)
            .ToListAsync();
    }

    // =====================================================
    // PRODUCTOS MÁS VENDIDOS
    // =====================================================

    public async Task<IEnumerable<ReporteProductoVendidoData>>
        ObtenerProductosMasVendidosAsync(
            DateTime desde,
            DateTime hasta)
    {
        return await _context.Database
            .SqlQuery<ReporteProductoVendidoData>($"""
                SELECT TOP 10

                    p.Nombre AS Nombre,

                    SUM(
                        dv.Cantidad
                    ) AS Cantidad,

                    CAST(
                        ISNULL(
                            SUM(dv.Subtotal),
                            0
                        )
                        AS decimal(18,2)
                    ) AS Total

                FROM dbo.DetalleVenta dv

                INNER JOIN dbo.Venta v
                    ON v.IdVenta = dv.IdVenta

                INNER JOIN dbo.Producto p
                    ON p.IdProducto = dv.IdProducto

                WHERE v.FechaVenta >= {desde}
                  AND v.FechaVenta < {hasta}
                  AND v.Estado <> 'Anulada'

                GROUP BY
                    p.Nombre

                ORDER BY
                    Cantidad DESC
                """)
            .ToListAsync();
    }

    // =====================================================
    // TRABAJOS POR MECÁNICO
    // =====================================================

    public async Task<IEnumerable<ReporteMecanicoData>>
        ObtenerTrabajosPorMecanicoAsync(
            DateTime desde,
            DateTime hasta)
    {
        return await _context.Database
            .SqlQuery<ReporteMecanicoData>($"""
                SELECT

                    CONCAT(
                        e.Nombre,
                        ' ',
                        e.Apellido1,
                        CASE
                            WHEN e.Apellido2 IS NULL
                            THEN ''
                            ELSE ' ' + e.Apellido2
                        END
                    ) AS Mecanico,

                    COUNT(
                        DISTINCT ote.IdOrdenTrabajo
                    ) AS Trabajos,

                    CAST(
                        ISNULL(
                            SUM(
                                ote.HorasTrabajadas
                            ),
                            0
                        )
                        AS decimal(18,2)
                    ) AS Horas

                FROM dbo.OrdenTrabajoEmpleado ote

                INNER JOIN dbo.OrdenTrabajo o
                    ON o.IdOrdenTrabajo =
                       ote.IdOrdenTrabajo

                INNER JOIN dbo.Empleado e
                    ON e.IdEmpleado =
                       ote.IdEmpleado

                WHERE o.FechaInicio >= {desde}
                  AND o.FechaInicio < {hasta}

                GROUP BY
                    e.Nombre,
                    e.Apellido1,
                    e.Apellido2

                ORDER BY
                    Trabajos DESC
                """)
            .ToListAsync();
    }

    // =====================================================
    // SERVICIOS MÁS SOLICITADOS
    // =====================================================

    public async Task<IEnumerable<ReporteServicioData>>
        ObtenerServiciosMasSolicitadosAsync(
            DateTime desde,
            DateTime hasta)
    {
        return await _context.Database
            .SqlQuery<ReporteServicioData>($"""
                SELECT TOP 10

                    s.Nombre AS Servicio,

                    SUM(
                        ots.Cantidad
                    ) AS Cantidad

                FROM dbo.OrdenTrabajoServicio ots

                INNER JOIN dbo.OrdenTrabajo o
                    ON o.IdOrdenTrabajo =
                       ots.IdOrdenTrabajo

                INNER JOIN dbo.Servicio s
                    ON s.IdServicio =
                       ots.IdServicio

                WHERE o.FechaInicio >= {desde}
                  AND o.FechaInicio < {hasta}
                  AND o.Estado <> 'Cancelada'

                GROUP BY
                    s.Nombre

                ORDER BY
                    Cantidad DESC
                """)
            .ToListAsync();
    }

    // =====================================================
    // COMPRAS POR MES
    // =====================================================

    public async Task<IEnumerable<ReporteCompraMesData>>
        ObtenerComprasPorMesAsync(
            DateTime desde,
            DateTime hasta)
    {
        return await _context.Database
            .SqlQuery<ReporteCompraMesData>($"""
                SELECT

                    FORMAT(
                        c.FechaCompra,
                        'yyyy-MM'
                    ) AS Mes,

                    CAST(
                        ISNULL(
                            SUM(c.Total),
                            0
                        )
                        AS decimal(18,2)
                    ) AS Total,

                    COUNT(*) AS Cantidad

                FROM dbo.Compra c

                WHERE c.FechaCompra >= {desde}
                  AND c.FechaCompra < {hasta}
                  AND c.Estado <> 'Anulada'

                GROUP BY
                    FORMAT(
                        c.FechaCompra,
                        'yyyy-MM'
                    )

                ORDER BY
                    Mes
                """)
            .ToListAsync();
    }

    // =====================================================
    // STOCK BAJO
    // =====================================================

    public async Task<IEnumerable<ReporteStockData>>
        ObtenerStockBajoAsync()
    {
        return await _context.Database
            .SqlQuery<ReporteStockData>($"""
                SELECT TOP 20

                    p.Codigo AS Codigo,

                    p.Nombre AS Nombre,

                    p.Stock AS Stock,

                    p.StockMinimo AS StockMinimo

                FROM dbo.Producto p

                WHERE p.Activo = 1
                  AND p.Stock <= p.StockMinimo

                ORDER BY
                    p.Stock ASC,
                    p.Nombre ASC
                """)
            .ToListAsync();
    }

    // =====================================================
    // FACTURAS PENDIENTES
    // =====================================================

    public async Task<IEnumerable<ReporteFacturaPendienteData>>
        ObtenerFacturasPendientesAsync()
    {
        return await _context.Database
            .SqlQuery<ReporteFacturaPendienteData>($"""
                SELECT TOP 50

                    IdFactura,

                    NumeroFactura,

                    FechaEmision,

                    Cliente,

                    Total,

                    TotalPagado,

                    SaldoPendiente,

                    Estado

                FROM dbo.vw_FacturasPendientes

                ORDER BY
                    FechaEmision DESC
                """)
            .ToListAsync();
    }

    // =====================================================
    // HISTORIAL DE VEHÍCULOS
    // =====================================================

    public async Task<IEnumerable<ReporteVehiculoHistorialData>>
        ObtenerHistorialVehiculosAsync(
            string? placa)
    {
        return await _context.Database
            .SqlQuery<ReporteVehiculoHistorialData>($"""
                SELECT TOP 100

                    v.Placa AS Placa,

                    CONCAT(
                        c.Nombre,
                        ' ',
                        c.Apellido1,
                        CASE
                            WHEN c.Apellido2 IS NULL
                            THEN ''
                            ELSE ' ' + c.Apellido2
                        END
                    ) AS Cliente,

                    o.FechaInicio AS Fecha,

                    o.IdOrdenTrabajo AS IdOrdenTrabajo,

                    o.Estado AS Estado,

                    o.Observaciones AS Observaciones

                FROM dbo.OrdenTrabajo o

                INNER JOIN dbo.Vehiculo v
                    ON v.IdVehiculo =
                       o.IdVehiculo

                INNER JOIN dbo.Cliente c
                    ON c.IdCliente =
                       v.IdCliente

                WHERE
                    (
                        {placa} IS NULL
                        OR
                        v.Placa LIKE
                            '%' + {placa} + '%'
                    )

                ORDER BY
                    o.FechaInicio DESC
                """)
            .ToListAsync();
    }
}