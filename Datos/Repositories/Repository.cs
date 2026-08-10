using Datos.Context;
using Datos.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Datos.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly SistemaTallerDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(SistemaTallerDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> ObtenerTodosAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> BuscarAsync(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
    }

    public async Task AgregarAsync(T entidad)
    {
        await _dbSet.AddAsync(entidad);
    }

    public void Actualizar(T entidad)
    {
        _dbSet.Update(entidad);
    }

    public void Eliminar(T entidad)
    {
        _dbSet.Remove(entidad);
    }

    public async Task<bool> ExisteAsync(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<int> GuardarCambiosAsync()
    {
        return await _context.SaveChangesAsync();
    }
}