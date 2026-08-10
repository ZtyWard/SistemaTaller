using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class TipoCombustibleRepository
    : Repository<TipoCombustible>, ITipoCombustibleRepository
{
    public TipoCombustibleRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<TipoCombustible>> ObtenerActivasAsync()
    {
        return await _context.TiposCombustible
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}