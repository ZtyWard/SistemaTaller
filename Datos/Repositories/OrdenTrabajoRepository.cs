using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class OrdenTrabajoRepository
    : Repository<OrdenTrabajo>, IOrdenTrabajoRepository
{
    public OrdenTrabajoRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<OrdenTrabajo?> ObtenerCompletaAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cotizacion)
                .ThenInclude(x => x!.Diagnostico)
                    .ThenInclude(x => x!.Recepcion)
                        .ThenInclude(x => x!.Vehiculo)
                            .ThenInclude(x => x!.Cliente)
            .FirstOrDefaultAsync(
                x => x.IdOrdenTrabajo == id);
    }

    public async Task<IEnumerable<OrdenTrabajo>>
        ObtenerPorEstadoAsync(string estado)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cotizacion)
                .ThenInclude(x => x!.Diagnostico)
                    .ThenInclude(x => x!.Recepcion)
                        .ThenInclude(x => x!.Vehiculo)
            .Where(x => x.Estado == estado)
            .OrderByDescending(x => x.FechaInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrdenTrabajo>>
        ObtenerAbiertasAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cotizacion)
                .ThenInclude(x => x!.Diagnostico)
                    .ThenInclude(x => x!.Recepcion)
                        .ThenInclude(x => x!.Vehiculo)
                            .ThenInclude(x => x!.Cliente)
            .Where(x => x.Estado != "Finalizada")
            .OrderByDescending(x => x.FechaInicio)
            .ToListAsync();
    }
}