using Datos.Models;

namespace Datos.Interfaces;

public interface IDiagnosticoRepository : IRepository<Diagnostico>
{
    Task<Diagnostico?> ObtenerCompletoAsync(int id);

    Task<IEnumerable<Diagnostico>> ObtenerPorRecepcionAsync(
        int idRecepcion);
}