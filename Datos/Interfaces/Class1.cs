using Datos.Models;

namespace Datos.Interfaces;

public interface IProductoRepository : IRepository<Producto>
{
    Task<IEnumerable<Producto>> ObtenerActivosAsync();

    Task<IEnumerable<Producto>> ObtenerStockBajoAsync();

    Task<Producto?> ObtenerPorCodigoAsync(
        string codigo);

    Task<Producto?> ObtenerPorCodigoBarrasAsync(
        string codigoBarras);

    Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(
        int idCategoriaProducto);
}