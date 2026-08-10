using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class EspecialidadRepository
    : Repository<Especialidad>, IEspecialidadRepository
{
    public EspecialidadRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Especialidad>> ObtenerActivasAsync()
    {
        return await _context.Especialidades
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}