using System.Linq.Expressions;

namespace Datos.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> ObtenerPorIdAsync(int id);

    Task<IEnumerable<T>> ObtenerTodosAsync();

    Task<IEnumerable<T>> BuscarAsync(
        Expression<Func<T, bool>> predicate);

    Task AgregarAsync(T entidad);

    void Actualizar(T entidad);

    void Eliminar(T entidad);

    Task<bool> ExisteAsync(
        Expression<Func<T, bool>> predicate);

    Task<int> GuardarCambiosAsync();
}