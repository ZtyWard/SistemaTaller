using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Microsoft.EntityFrameworkCore;

namespace Datos.Repositories;

public class PagoRepository
    : Repository<Pago>, IPagoRepository
{
    public PagoRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Pago>>
        ObtenerRecientesAsync(int cantidad)
    {
        if (cantidad <= 0)
            cantidad = 20;

        return await _dbSet
            .AsNoTracking()
            .Include(x => x.Factura)
                .ThenInclude(x => x!.Cliente)
            .OrderByDescending(x => x.FechaPago)
            .Take(cantidad)
            .ToListAsync();
    }

    public async Task<Pago?>
        ObtenerPorIdConFacturaAsync(
            int idPago)
    {
        return await _dbSet
            .Include(x => x.Factura)
                .ThenInclude(x => x!.Cliente)
            .FirstOrDefaultAsync(x =>
                x.IdPago == idPago);
    }

    public async Task<IEnumerable<Pago>>
        ObtenerPorFacturaAsync(
            int idFactura)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x =>
                x.IdFactura == idFactura)
            .OrderByDescending(x => x.FechaPago)
            .ToListAsync();
    }

    public async Task<Pago>
        RegistrarConProcedimientoAsync(
            int idFactura,
            decimal monto,
            string formaPago,
            string? numeroReferencia,
            string? usuarioId,
            string? observaciones)
    {
        await _context.Database
            .ExecuteSqlInterpolatedAsync($@"
                EXEC dbo.sp_RegistrarPago
                    @IdFactura = {idFactura},
                    @Monto = {monto},
                    @FormaPago = {formaPago},
                    @NumeroReferencia = {numeroReferencia},
                    @UsuarioId = {usuarioId},
                    @Observaciones = {observaciones}");

        var pago = await _dbSet
            .Include(x => x.Factura)
                .ThenInclude(x => x!.Cliente)
            .Where(x =>
                x.IdFactura == idFactura)
            .OrderByDescending(x => x.IdPago)
            .FirstOrDefaultAsync();

        if (pago == null)
        {
            throw new InvalidOperationException(
                "El pago fue registrado, pero no se pudo recuperar el registro generado.");
        }

        return pago;
    }
}