using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IMovimientoInventarioService
{
    Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerTodosAsync();

    Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerPorProductoAsync(
            int idProducto);

    Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerPorTipoAsync(
            string tipoMovimiento);

    Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerRecientesAsync(
            int cantidad = 10);

    Task<MovimientoInventarioDto?>
        ObtenerPorIdAsync(int id);

    Task CrearAsync(
        MovimientoInventarioGuardarDto dto);
}