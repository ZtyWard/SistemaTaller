using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class ModeloRepository : Repository<Modelo>, IModeloRepository
{
    public ModeloRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Modelo>> ObtenerTodosAsync()
    {
        return await _context.Modelos
            .AsNoTracking()
            .Include(x => x.Marca)
            .OrderBy(x => x.Marca!.Nombre)
            .ThenBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Modelo>> ObtenerActivasAsync()
    {
        return await _context.Modelos
            .AsNoTracking()
            .Include(x => x.Marca)
            .Where(x => x.Activo)
            .OrderBy(x => x.Marca!.Nombre)
            .ThenBy(x => x.Nombre)
            .ToListAsync();
    }

    public async Task<Modelo?> ObtenerPorIdAsync(int id)
    {
        return await _context.Modelos
            .Include(x => x.Marca)
            .FirstOrDefaultAsync(x => x.IdModelo == id);
    }
}