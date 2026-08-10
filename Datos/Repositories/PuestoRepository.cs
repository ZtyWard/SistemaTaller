using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class PuestoRepository : Repository<Puesto>, IPuestoRepository
{
    public PuestoRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Puesto>> ObtenerActivosAsync()
    {
        return await _context.Puestos
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}