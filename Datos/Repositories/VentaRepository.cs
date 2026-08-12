using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class VentaRepository
    : Repository<Venta>, IVentaRepository
{
    public VentaRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    // =====================================================
    // POR CLIENTE
    // =====================================================

    public async Task<IEnumerable<Venta>>
        ObtenerPorClienteAsync(
            int idCliente)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Where(x =>
                x.IdCliente == idCliente)
            .OrderByDescending(x =>
                x.FechaVenta)
            .ToListAsync();
    }

    // =====================================================
    // POR ESTADO
    // =====================================================

    public async Task<IEnumerable<Venta>>
        ObtenerPorEstadoAsync(
            string estado)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Where(x =>
                x.Estado == estado)
            .OrderByDescending(x =>
                x.FechaVenta)
            .ToListAsync();
    }

    // =====================================================
    // RECIENTES
    // =====================================================

    public async Task<IEnumerable<Venta>>
        ObtenerRecientesAsync(
            int cantidad)
    {
        if (cantidad <= 0)
            cantidad = 10;

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cliente)
            .OrderByDescending(x =>
                x.FechaVenta)
            .Take(cantidad)
            .ToListAsync();
    }

    // =====================================================
    // VENTA + DETALLES + PRODUCTOS
    // =====================================================

    public async Task<Venta?>
        ObtenerPorIdConDetallesAsync(
            int idVenta)
    {
        return await _dbSet
            .Include(x => x.Cliente)
            .Include(x => x.Detalles)
                .ThenInclude(x => x.Producto)
            .FirstOrDefaultAsync(x =>
                x.IdVenta == idVenta);
    }

    // =====================================================
    // COMPLETAR VENTA
    // =====================================================

    public async Task CompletarVentaAsync(
        int idVenta)
    {
        await using var transaction =
            await _context.Database
                .BeginTransactionAsync();

        try
        {
            var venta =
                await _dbSet
                    .Include(x => x.Detalles)
                    .FirstOrDefaultAsync(x =>
                        x.IdVenta == idVenta);

            if (venta == null)
            {
                throw new InvalidOperationException(
                    "La venta no existe.");
            }

            if (venta.Estado == "Completada")
            {
                throw new InvalidOperationException(
                    "La venta ya fue completada.");
            }

            if (venta.Estado == "Cancelada")
            {
                throw new InvalidOperationException(
                    "No se puede completar una venta cancelada.");
            }

            if (!venta.Detalles.Any())
            {
                throw new InvalidOperationException(
                    "La venta no tiene detalles.");
            }

            // =============================================
            // VALIDAR STOCK ANTES DE HACER MOVIMIENTOS
            // =============================================

            foreach (var detalle in venta.Detalles)
            {
                var producto =
                    await _context.Productos
                        .FirstOrDefaultAsync(x =>
                            x.IdProducto ==
                            detalle.IdProducto);

                if (producto == null)
                {
                    throw new InvalidOperationException(
                        $"El producto {detalle.IdProducto} no existe.");
                }

                if (!producto.Activo)
                {
                    throw new InvalidOperationException(
                        $"El producto '{producto.Nombre}' está inactivo.");
                }

                if (detalle.Cantidad <= 0)
                {
                    throw new InvalidOperationException(
                        "La cantidad de un detalle debe ser mayor que cero.");
                }

                if (producto.Stock < detalle.Cantidad)
                {
                    throw new InvalidOperationException(
                        $"Stock insuficiente para '{producto.Nombre}'. " +
                        $"Disponible: {producto.Stock}. " +
                        $"Solicitado: {detalle.Cantidad}.");
                }
            }

            // =============================================
            // CADA DETALLE GENERA UNA SALIDA
            // =============================================

            foreach (var detalle in venta.Detalles)
            {
                var observacion =
                    $"Salida por venta #{venta.IdVenta}";

                await _context.Database
                    .ExecuteSqlInterpolatedAsync($@"
                        EXEC dbo.sp_RegistrarMovimientoInventario
                            @IdProducto = {detalle.IdProducto},
                            @TipoMovimiento = {"Salida"},
                            @Cantidad = {detalle.Cantidad},
                            @Observacion = {observacion}");
            }

            // =============================================
            // CAMBIAR ESTADO
            // =============================================

            venta.Estado = "Completada";

            _context.Ventas.Update(venta);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}