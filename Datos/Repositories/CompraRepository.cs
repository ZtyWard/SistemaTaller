using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class CompraRepository
    : Repository<Compra>, ICompraRepository
{
    public CompraRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Compra>>
        ObtenerPorProveedorAsync(
            int idProveedor)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Proveedor)
            .Where(x => x.IdProveedor == idProveedor)
            .OrderByDescending(x => x.FechaCompra)
            .ToListAsync();
    }

    public async Task<IEnumerable<Compra>>
        ObtenerPorEstadoAsync(
            string estado)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Proveedor)
            .Where(x => x.Estado == estado)
            .OrderByDescending(x => x.FechaCompra)
            .ToListAsync();
    }

    public async Task<IEnumerable<Compra>>
        ObtenerRecientesAsync(
            int cantidad)
    {
        if (cantidad <= 0)
            cantidad = 10;

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Proveedor)
            .OrderByDescending(x => x.FechaCompra)
            .Take(cantidad)
            .ToListAsync();
    }
}