using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<Cliente?> ObtenerPorCedulaAsync(string cedula)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Cedula == cedula);
    }

    public async Task<Cliente?> ObtenerConVehiculosAsync(int idCliente)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Vehiculos)
            .FirstOrDefaultAsync(x => x.IdCliente == idCliente);
    }

    public async Task<IEnumerable<Cliente>> ObtenerActivosAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.Activo)
            .ToListAsync();
    }
}