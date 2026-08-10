using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class ProveedorRepository
    : Repository<Proveedor>, IProveedorRepository
{
    public ProveedorRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Proveedor>>
        ObtenerActivosAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<Proveedor?>
        ObtenerPorCedulaJuridicaAsync(
            string cedulaJuridica)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.CedulaJuridica ==
                     cedulaJuridica);
    }
}