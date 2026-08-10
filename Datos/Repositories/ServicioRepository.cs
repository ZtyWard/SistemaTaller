using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class ServicioRepository
    : Repository<Servicio>, IServicioRepository
{
    public ServicioRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Servicio>>
        ObtenerActivosAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}