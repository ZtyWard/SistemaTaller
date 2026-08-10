using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class CategoriaProductoRepository
    : Repository<CategoriaProducto>,
      ICategoriaProductoRepository
{
    public CategoriaProductoRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<CategoriaProducto>>
        ObtenerActivasAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}