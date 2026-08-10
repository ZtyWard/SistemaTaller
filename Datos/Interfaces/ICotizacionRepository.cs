using Datos.Models;

namespace Datos.Interfaces;

public interface ICotizacionRepository : IRepository<Cotizacion>
{
    Task<Cotizacion?> ObtenerCompletaAsync(int id);

    Task<IEnumerable<Cotizacion>> ObtenerPorDiagnosticoAsync(
        int idDiagnostico);

    Task<IEnumerable<Cotizacion>> ObtenerPendientesAsync();
}