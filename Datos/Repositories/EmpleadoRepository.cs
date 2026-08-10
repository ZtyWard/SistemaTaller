using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class EmpleadoRepository
    : Repository<Empleado>, IEmpleadoRepository
{
    public EmpleadoRepository(SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<Empleado?> ObtenerCompletoAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Puesto)
            .Include(x => x.Especialidad)
            .Include(x => x.Recepciones)
            .Include(x => x.Diagnosticos)
            .FirstOrDefaultAsync(
                x => x.IdEmpleado == id);
    }

    public async Task<IEnumerable<Empleado>>
        ObtenerActivosAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Puesto)
            .Include(x => x.Especialidad)
            .Where(x => x.Activo)
            .OrderBy(x => x.Nombre)
            .ThenBy(x => x.Apellido1)
            .ToListAsync();
    }

    public async Task<IEnumerable<Empleado>>
        ObtenerPorPuestoAsync(int idPuesto)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Puesto)
            .Include(x => x.Especialidad)
            .Where(x => x.IdPuesto == idPuesto)
            .OrderBy(x => x.Nombre)
            .ThenBy(x => x.Apellido1)
            .ToListAsync();
    }

    public async Task<IEnumerable<Empleado>>
        ObtenerPorEspecialidadAsync(int idEspecialidad)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Puesto)
            .Include(x => x.Especialidad)
            .Where(x => x.IdEspecialidad == idEspecialidad)
            .OrderBy(x => x.Nombre)
            .ThenBy(x => x.Apellido1)
            .ToListAsync();
    }
}