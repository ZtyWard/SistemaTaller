using Datos.Models;

namespace Datos.Interfaces;

public interface IEspecialidadRepository : IRepository<Especialidad>
{
    Task<IEnumerable<Especialidad>> ObtenerActivasAsync();
}