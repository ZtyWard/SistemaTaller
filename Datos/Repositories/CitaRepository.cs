using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class CitaRepository
    : Repository<Cita>,
      ICitaRepository
{
    public CitaRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Cita>>
        ObtenerAgendaAsync(
            DateTime? fechaInicio,
            DateTime? fechaFin)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Vehiculo)
            .Include(x => x.Servicio)
            .Include(x => x.Empleado)
            .AsQueryable();

        if (fechaInicio.HasValue)
        {
            query = query.Where(x =>
                x.FechaFin >= fechaInicio.Value);
        }

        if (fechaFin.HasValue)
        {
            query = query.Where(x =>
                x.FechaInicio <= fechaFin.Value);
        }

        return await query
            .OrderBy(x => x.FechaInicio)
            .ToListAsync();
    }

    public async Task<Cita?>
        ObtenerCompletaAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Vehiculo)
            .Include(x => x.Servicio)
            .Include(x => x.Empleado)
            .FirstOrDefaultAsync(
                x => x.IdCita == id);
    }

    public async Task<bool>
        ExisteConflictoAsync(
            DateTime fechaInicio,
            DateTime fechaFin,
            int? idEmpleado,
            string? area,
            int? idCitaExcluir = null)
    {
        var areaNormalizada =
            string.IsNullOrWhiteSpace(area)
                ? null
                : area.Trim();

        var query = _dbSet
            .AsNoTracking()
            .Where(x =>
                x.Estado != "Cancelada" &&
                x.FechaInicio < fechaFin &&
                x.FechaFin > fechaInicio);

        if (idCitaExcluir.HasValue)
        {
            query = query.Where(x =>
                x.IdCita != idCitaExcluir.Value);
        }

        if (idEmpleado.HasValue)
        {
            var conflictoEmpleado =
                await query.AnyAsync(x =>
                    x.IdEmpleado == idEmpleado.Value);

            if (conflictoEmpleado)
                return true;
        }

        if (!string.IsNullOrWhiteSpace(areaNormalizada))
        {
            var conflictoArea =
                await query.AnyAsync(x =>
                    x.Area != null &&
                    x.Area == areaNormalizada);

            if (conflictoArea)
                return true;
        }

        return false;
    }
}