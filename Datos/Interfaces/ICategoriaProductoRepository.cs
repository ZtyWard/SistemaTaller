using Datos.Models;

namespace Datos.Interfaces;

public interface ICategoriaProductoRepository
    : IRepository<CategoriaProducto>
{
    Task<IEnumerable<CategoriaProducto>>
        ObtenerActivasAsync();
}