using Datos.Models;

namespace Datos.Interfaces;

public interface IGarantiaRepository
    : IRepository<Garantia>
{
    Task<Garantia?> ObtenerCompletaAsync(
        int idGarantia);

    Task<IEnumerable<Garantia>>
        ObtenerVigentesAsync();

    Task<IEnumerable<Garantia>>
        ObtenerPorOrdenTrabajoAsync(
            int idOrdenTrabajo);

    Task<IEnumerable<Garantia>>
        ObtenerPorVentaAsync(
            int idVenta);

    Task<IEnumerable<Garantia>>
        ObtenerPorVencerAsync(
            int dias);
}