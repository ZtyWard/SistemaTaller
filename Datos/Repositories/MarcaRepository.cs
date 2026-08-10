using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class MarcaRepository : Repository<Marca>, IMarcaRepository
{
    public MarcaRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Marca>> ObtenerActivasAsync()
    {
        return await _context.Marcas
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}