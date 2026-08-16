using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class AuditoriaRepository
    : Repository<Auditoria>, IAuditoriaRepository
{
    public AuditoriaRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Auditoria>> ObtenerAsync(
        string? usuarioId = null,
        string? modulo = null,
        string? accion = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null,
        string? registroId = null)
    {
        IQueryable<Auditoria> consulta =
            _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(usuarioId))
        {
            consulta = consulta.Where(x =>
                x.UsuarioId == usuarioId);
        }

        if (!string.IsNullOrWhiteSpace(modulo))
        {
            consulta = consulta.Where(x =>
                x.Modulo == modulo);
        }

        if (!string.IsNullOrWhiteSpace(accion))
        {
            consulta = consulta.Where(x =>
                x.Accion == accion);
        }

        if (fechaDesde.HasValue)
        {
            var desde = fechaDesde.Value.Date;

            consulta = consulta.Where(x =>
                x.Fecha >= desde);
        }

        if (fechaHasta.HasValue)
        {
            var hasta = fechaHasta.Value.Date.AddDays(1);

            consulta = consulta.Where(x =>
                x.Fecha < hasta);
        }

        if (!string.IsNullOrWhiteSpace(registroId))
        {
            consulta = consulta.Where(x =>
                x.RegistroId == registroId);
        }

        return await consulta
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.IdAuditoria)
            .ToListAsync();
    }
}