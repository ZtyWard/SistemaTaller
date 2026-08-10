using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class TipoVehiculoRepository
    : Repository<TipoVehiculo>, ITipoVehiculoRepository
{
    public TipoVehiculoRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<TipoVehiculo>> ObtenerActivasAsync()
    {
        return await _context.TiposVehiculo
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}