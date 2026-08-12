using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class MovimientoInventarioRepository
    : Repository<MovimientoInventario>,
      IMovimientoInventarioRepository
{
    public MovimientoInventarioRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<MovimientoInventario>>
        ObtenerPorProductoAsync(int idProducto)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Producto)
            .Where(x => x.IdProducto == idProducto)
            .OrderByDescending(x => x.FechaMovimiento)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovimientoInventario>>
        ObtenerPorTipoAsync(string tipoMovimiento)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Producto)
            .Where(x =>
                x.TipoMovimiento == tipoMovimiento)
            .OrderByDescending(x => x.FechaMovimiento)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovimientoInventario>>
        ObtenerRecientesAsync(int cantidad)
    {
        if (cantidad <= 0)
            cantidad = 10;

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Producto)
            .OrderByDescending(x => x.FechaMovimiento)
            .Take(cantidad)
            .ToListAsync();
    }

    // =====================================================
    // REGISTRAR MOVIMIENTO MEDIANTE STORED PROCEDURE
    // =====================================================

    public async Task RegistrarMovimientoAsync(
        int idProducto,
        string tipoMovimiento,
        int cantidad,
        string? observacion)
    {
        await _context.Database
            .ExecuteSqlInterpolatedAsync($@"
                EXEC dbo.sp_RegistrarMovimientoInventario
                    @IdProducto = {idProducto},
                    @TipoMovimiento = {tipoMovimiento},
                    @Cantidad = {cantidad},
                    @Observacion = {observacion}");
    }
}