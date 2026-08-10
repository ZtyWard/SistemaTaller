using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class ProductoRepository
    : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Producto>>
        ObtenerActivosAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.CategoriaProducto)
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Producto>>
        ObtenerStockBajoAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.CategoriaProducto)
            .Where(x =>
                x.Activo &&
                x.Stock <= x.StockMinimo)
            .OrderBy(x => x.Stock)
            .ToListAsync();
    }

    public async Task<Producto?>
        ObtenerPorCodigoAsync(string codigo)
    {
        return await _dbSet
            .Include(x => x.CategoriaProducto)
            .FirstOrDefaultAsync(
                x => x.Codigo == codigo);
    }

    public async Task<IEnumerable<Producto>>
        ObtenerPorCategoriaAsync(
            int idCategoriaProducto)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.CategoriaProducto)
            .Where(x =>
                x.IdCategoriaProducto ==
                idCategoriaProducto)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }
}