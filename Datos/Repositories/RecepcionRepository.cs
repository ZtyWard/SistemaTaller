using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class RecepcionRepository : Repository<Recepcion>, IRecepcionRepository
{
    public RecepcionRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<Recepcion?> ObtenerCompletaAsync(int idRecepcion)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Vehiculo)
                .ThenInclude(x => x!.Cliente)
            .Include(x => x.Vehiculo)
                .ThenInclude(x => x!.Marca)
            .Include(x => x.Vehiculo)
                .ThenInclude(x => x!.Modelo)
            .Include(x => x.Vehiculo)
                .ThenInclude(x => x!.TipoVehiculo)
            .Include(x => x.Vehiculo)
                .ThenInclude(x => x!.TipoCombustible)
            .Include(x => x.Empleado)
            .Include(x => x.Diagnosticos)
            .FirstOrDefaultAsync(x => x.IdRecepcion == idRecepcion);
    }

    public async Task<IEnumerable<Recepcion>> ObtenerPorVehiculoAsync(int idVehiculo)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Empleado)
            .Where(x => x.IdVehiculo == idVehiculo)
            .OrderByDescending(x => x.FechaRecepcion)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recepcion>> ObtenerAbiertasAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Vehiculo)
                .ThenInclude(x => x!.Cliente)
            .Include(x => x.Empleado)
            .Where(x => x.Estado != "Finalizado")
            .OrderByDescending(x => x.FechaRecepcion)
            .ToListAsync();
    }
}