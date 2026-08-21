using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>>
        ObtenerTodosAsync();


    Task<IEnumerable<ProductoDto>>
        ObtenerActivosAsync();


    Task<IEnumerable<ProductoDto>>
        ObtenerStockBajoAsync();


    Task<IEnumerable<ProductoDto>>
        ObtenerPorCategoriaAsync(
            int idCategoriaProducto);


    Task<ProductoDto?>
        ObtenerPorIdAsync(
            int id);


    Task<ProductoDto?>
        ObtenerPorCodigoAsync(
            string codigo);


    Task<ProductoDto?>
        ObtenerPorCodigoBarrasAsync(
            string codigoBarras);


    Task CrearAsync(
        ProductoGuardarDto dto);


    Task<bool>
        ActualizarAsync(
            int id,
            ProductoGuardarDto dto);


    Task<bool>
        CambiarEstadoAsync(
            int id,
            bool activo);
}