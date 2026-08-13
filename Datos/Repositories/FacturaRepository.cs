using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class FacturaRepository
    : Repository<Factura>, IFacturaRepository
{
    public FacturaRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Factura>>
        ObtenerRecientesAsync(int cantidad)
    {
        if (cantidad <= 0)
            cantidad = 10;

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Pagos)
            .OrderByDescending(x => x.FechaEmision)
            .Take(cantidad)
            .ToListAsync();
    }

    public async Task<Factura?>
        ObtenerPorIdConRelacionesAsync(
            int idFactura)
    {
        return await _dbSet
            .Include(x => x.Cliente)
            .Include(x => x.OrdenTrabajo)
            .Include(x => x.Venta)
            .Include(x => x.Pagos)
            .FirstOrDefaultAsync(x =>
                x.IdFactura == idFactura);
    }

    public async Task<Factura?>
        ObtenerPorNumeroAsync(
            string numeroFactura)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x =>
                x.NumeroFactura == numeroFactura);
    }

    public async Task<IEnumerable<Factura>>
        ObtenerPendientesAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Pagos)
            .Where(x =>
                x.Estado == "Pendiente" ||
                x.Estado == "Parcialmente pagada")
            .OrderByDescending(x => x.FechaEmision)
            .ToListAsync();
    }

    public async Task AnularAsync(
        int idFactura)
    {
        var factura =
            await _dbSet.FirstOrDefaultAsync(x =>
                x.IdFactura == idFactura);

        if (factura == null)
        {
            throw new InvalidOperationException(
                "La factura no existe.");
        }

        if (factura.Estado == "Pagada")
        {
            throw new InvalidOperationException(
                "No se puede anular una factura pagada.");
        }

        factura.Estado = "Anulada";

        _context.Facturas.Update(factura);

        await _context.SaveChangesAsync();
    }
}