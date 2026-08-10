using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class DiagnosticoRepository
    : Repository<Diagnostico>, IDiagnosticoRepository
{
    public DiagnosticoRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<Diagnostico?> ObtenerCompletoAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Recepcion)
                .ThenInclude(x => x!.Vehiculo)
            .Include(x => x.Empleado)
            .Include(x => x.Cotizaciones)
            .FirstOrDefaultAsync(
                x => x.IdDiagnostico == id);
    }

    public async Task<IEnumerable<Diagnostico>>
        ObtenerPorRecepcionAsync(int idRecepcion)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Empleado)
            .Where(x => x.IdRecepcion == idRecepcion)
            .OrderByDescending(x => x.FechaDiagnostico)
            .ToListAsync();
    }
}