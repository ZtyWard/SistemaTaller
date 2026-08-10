using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class VehiculoRepository : Repository<Vehiculo>, IVehiculoRepository
{
    public VehiculoRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<Vehiculo?> ObtenerPorPlacaAsync(string placa)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Placa == placa);
    }

    public async Task<Vehiculo?> ObtenerCompletoAsync(int idVehiculo)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Marca)
            .Include(x => x.Modelo)
            .Include(x => x.TipoVehiculo)
            .Include(x => x.TipoCombustible)
            .Include(x => x.Recepciones)
            .FirstOrDefaultAsync(x => x.IdVehiculo == idVehiculo);
    }

    public async Task<IEnumerable<Vehiculo>> ObtenerPorClienteAsync(int idCliente)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Marca)
            .Include(x => x.Modelo)
            .Include(x => x.TipoVehiculo)
            .Include(x => x.TipoCombustible)
            .Where(x => x.IdCliente == idCliente)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehiculo>> ObtenerActivosAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.Activo)
            .Include(x => x.Marca)
            .Include(x => x.Modelo)
            .ToListAsync();
    }
}