using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class CompraRepository
    : Repository<Compra>, ICompraRepository
{
    public CompraRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    // =====================================================
    // POR PROVEEDOR
    // =====================================================

    public async Task<IEnumerable<Compra>>
        ObtenerPorProveedorAsync(
            int idProveedor)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Proveedor)
            .Where(x =>
                x.IdProveedor == idProveedor)
            .OrderByDescending(x =>
                x.FechaCompra)
            .ToListAsync();
    }

    // =====================================================
    // POR ESTADO
    // =====================================================

    public async Task<IEnumerable<Compra>>
        ObtenerPorEstadoAsync(
            string estado)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Proveedor)
            .Where(x =>
                x.Estado == estado)
            .OrderByDescending(x =>
                x.FechaCompra)
            .ToListAsync();
    }

    // =====================================================
    // RECIENTES
    // =====================================================

    public async Task<IEnumerable<Compra>>
        ObtenerRecientesAsync(
            int cantidad)
    {
        if (cantidad <= 0)
            cantidad = 10;

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Proveedor)
            .OrderByDescending(x =>
                x.FechaCompra)
            .Take(cantidad)
            .ToListAsync();
    }

    // =====================================================
    // COMPRA + DETALLES + PRODUCTOS
    // =====================================================

    public async Task<Compra?>
        ObtenerPorIdConDetallesAsync(
            int idCompra)
    {
        return await _dbSet
            .Include(x => x.Proveedor)
            .Include(x => x.Detalles)
                .ThenInclude(x => x.Producto)
            .FirstOrDefaultAsync(x =>
                x.IdCompra == idCompra);
    }

    // =====================================================
    // COMPLETAR COMPRA
    // =====================================================

    public async Task CompletarCompraAsync(
        int idCompra)
    {
        await using var transaction =
            await _context.Database
                .BeginTransactionAsync();

        try
        {
            var compra =
                await _dbSet
                    .Include(x => x.Detalles)
                    .FirstOrDefaultAsync(x =>
                        x.IdCompra == idCompra);

            if (compra == null)
            {
                throw new InvalidOperationException(
                    "La compra no existe.");
            }

            if (compra.Estado == "Completada")
            {
                throw new InvalidOperationException(
                    "La compra ya fue completada.");
            }

            if (compra.Estado == "Cancelada")
            {
                throw new InvalidOperationException(
                    "No se puede completar una compra cancelada.");
            }

            if (!compra.Detalles.Any())
            {
                throw new InvalidOperationException(
                    "La compra no tiene detalles.");
            }

            // =============================================
            // CADA DETALLE GENERA UNA ENTRADA
            // =============================================

            foreach (var detalle in compra.Detalles)
            {
                var observacion =
                    $"Entrada por compra #{compra.IdCompra}";

                await _context.Database
                    .ExecuteSqlInterpolatedAsync($@"
                        EXEC dbo.sp_RegistrarMovimientoInventario
                            @IdProducto = {detalle.IdProducto},
                            @TipoMovimiento = {"Entrada"},
                            @Cantidad = {detalle.Cantidad},
                            @Observacion = {observacion}");
            }

            // =============================================
            // CAMBIAR ESTADO
            // =============================================

            compra.Estado = "Completada";

            _context.Compras.Update(compra);

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