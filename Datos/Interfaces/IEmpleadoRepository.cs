using Datos.Models;

namespace Datos.Interfaces;

public interface IEmpleadoRepository : IRepository<Empleado>
{
    Task<Empleado?> ObtenerCompletoAsync(int id);

    Task<IEnumerable<Empleado>> ObtenerActivosAsync();

    Task<IEnumerable<Empleado>> ObtenerPorPuestoAsync(
        int idPuesto);

    Task<IEnumerable<Empleado>> ObtenerPorEspecialidadAsync(
        int idEspecialidad);
}