using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class CotizacionRepository
    : Repository<Cotizacion>, ICotizacionRepository
{
    public CotizacionRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<Cotizacion?> ObtenerCompletaAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Diagnostico)
                .ThenInclude(x => x!.Recepcion)
                    .ThenInclude(x => x!.Vehiculo)
            .Include(x => x.OrdenesTrabajo)
            .FirstOrDefaultAsync(
                x => x.IdCotizacion == id);
    }

    public async Task<IEnumerable<Cotizacion>>
        ObtenerPorDiagnosticoAsync(int idDiagnostico)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.OrdenesTrabajo)
            .Where(x => x.IdDiagnostico == idDiagnostico)
            .OrderByDescending(x => x.Fecha)
            .ToListAsync();
    }

    public async Task<IEnumerable<Cotizacion>>
        ObtenerPendientesAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Diagnostico)
                .ThenInclude(x => x!.Recepcion)
                    .ThenInclude(x => x!.Vehiculo)
            .Where(x => x.Estado == "Pendiente")
            .OrderByDescending(x => x.Fecha)
            .ToListAsync();
    }
}