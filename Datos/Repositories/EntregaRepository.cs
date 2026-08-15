using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class EntregaRepository
    : Repository<Entrega>,
      IEntregaRepository
{
    public EntregaRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    // =====================================================
    // OBTENER COMPLETA
    // =====================================================

    public async Task<Entrega?>
        ObtenerCompletaAsync(
            int idEntrega)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.OrdenTrabajo)
                .ThenInclude(x => x!.Cotizacion)
                    .ThenInclude(x => x!.Diagnostico)
                        .ThenInclude(x => x!.Recepcion)
                            .ThenInclude(x => x!.Vehiculo)
                                .ThenInclude(x => x!.Cliente)
            .FirstOrDefaultAsync(x =>
                x.IdEntrega == idEntrega);
    }

    // =====================================================
    // OBTENER POR ORDEN DE TRABAJO
    // =====================================================

    public async Task<Entrega?>
        ObtenerPorOrdenTrabajoAsync(
            int idOrdenTrabajo)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.OrdenTrabajo)
                .ThenInclude(x => x!.Cotizacion)
                    .ThenInclude(x => x!.Diagnostico)
                        .ThenInclude(x => x!.Recepcion)
                            .ThenInclude(x => x!.Vehiculo)
                                .ThenInclude(x => x!.Cliente)
            .FirstOrDefaultAsync(x =>
                x.IdOrdenTrabajo ==
                idOrdenTrabajo);
    }

    // =====================================================
    // VALIDAR SI YA EXISTE
    // =====================================================

    public async Task<bool>
        ExisteParaOrdenTrabajoAsync(
            int idOrdenTrabajo)
    {
        return await _dbSet
            .AnyAsync(x =>
                x.IdOrdenTrabajo ==
                idOrdenTrabajo);
    }
}