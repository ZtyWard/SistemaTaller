using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class GarantiaRepository
    : Repository<Garantia>,
      IGarantiaRepository
{
    public GarantiaRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    // =====================================================
    // OBTENER COMPLETA
    // =====================================================

    public async Task<Garantia?>
        ObtenerCompletaAsync(
            int idGarantia)
    {
        return await _dbSet
            .AsNoTracking()

            .Include(x => x.OrdenTrabajo)

            .Include(x => x.Venta)

            .Include(x => x.Producto)

            .Include(x => x.Servicio)

            .FirstOrDefaultAsync(x =>
                x.IdGarantia == idGarantia);
    }

    // =====================================================
    // GARANTÍAS VIGENTES
    // =====================================================

    public async Task<IEnumerable<Garantia>>
        ObtenerVigentesAsync()
    {
        return await _dbSet
            .AsNoTracking()

            .Include(x => x.Producto)

            .Include(x => x.Servicio)

            .Where(x =>
                x.Estado == "Vigente" &&
                x.FechaVencimiento >=
                    DateTime.UtcNow.Date)

            .OrderBy(x =>
                x.FechaVencimiento)

            .ToListAsync();
    }

    // =====================================================
    // POR ORDEN DE TRABAJO
    // =====================================================

    public async Task<IEnumerable<Garantia>>
        ObtenerPorOrdenTrabajoAsync(
            int idOrdenTrabajo)
    {
        return await _dbSet
            .AsNoTracking()

            .Include(x => x.Producto)

            .Include(x => x.Servicio)

            .Where(x =>
                x.IdOrdenTrabajo ==
                idOrdenTrabajo)

            .OrderByDescending(x =>
                x.FechaInicio)

            .ToListAsync();
    }

    // =====================================================
    // POR VENTA
    // =====================================================

    public async Task<IEnumerable<Garantia>>
        ObtenerPorVentaAsync(
            int idVenta)
    {
        return await _dbSet
            .AsNoTracking()

            .Include(x => x.Producto)

            .Include(x => x.Servicio)

            .Where(x =>
                x.IdVenta ==
                idVenta)

            .OrderByDescending(x =>
                x.FechaInicio)

            .ToListAsync();
    }

    // =====================================================
    // POR VENCER
    // =====================================================

    public async Task<IEnumerable<Garantia>>
        ObtenerPorVencerAsync(
            int dias)
    {
        if (dias < 0)
            dias = 0;

        var hoy =
            DateTime.UtcNow.Date;

        var limite =
            hoy.AddDays(dias);

        return await _dbSet
            .AsNoTracking()

            .Include(x => x.Producto)

            .Include(x => x.Servicio)

            .Where(x =>
                x.Estado == "Vigente" &&
                x.FechaVencimiento >= hoy &&
                x.FechaVencimiento <= limite)

            .OrderBy(x =>
                x.FechaVencimiento)

            .ToListAsync();
    }
}