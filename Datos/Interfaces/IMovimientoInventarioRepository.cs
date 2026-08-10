using Datos.Models;

namespace Datos.Interfaces;

public interface IMovimientoInventarioRepository
    : IRepository<MovimientoInventario>
{
    Task<IEnumerable<MovimientoInventario>>
        ObtenerPorProductoAsync(int idProducto);

    Task<IEnumerable<MovimientoInventario>>
        ObtenerPorTipoAsync(string tipoMovimiento);

    Task<IEnumerable<MovimientoInventario>>
        ObtenerRecientesAsync(int cantidad);
}