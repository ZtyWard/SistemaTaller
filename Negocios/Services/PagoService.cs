using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class PagoService
    : IPagoService
{
    private readonly IPagoRepository _repository;
    private readonly IFacturaRepository _facturaRepository;

    public PagoService(
        IPagoRepository repository,
        IFacturaRepository facturaRepository)
    {
        _repository = repository;
        _facturaRepository = facturaRepository;
    }

    public async Task<IEnumerable<PagoDto>>
        ObtenerRecientesAsync(
            int cantidad = 20)
    {
        var pagos =
            await _repository
                .ObtenerRecientesAsync(cantidad);

        return pagos.Select(MapearDto);
    }

    public async Task<IEnumerable<PagoDto>>
        ObtenerPorFacturaAsync(
            int idFactura)
    {
        var factura =
            await _facturaRepository
                .ObtenerPorIdConRelacionesAsync(idFactura);

        if (factura == null)
        {
            throw new ArgumentException(
                "La factura no existe.");
        }

        var pagos =
            await _repository
                .ObtenerPorFacturaAsync(idFactura);

        return pagos.Select(p =>
            MapearDto(
                p,
                factura));
    }

    public async Task<PagoDto?>
        ObtenerPorIdAsync(
            int idPago)
    {
        var pago =
            await _repository
                .ObtenerPorIdConFacturaAsync(idPago);

        if (pago == null)
            return null;

        var factura =
            pago.Factura;

        if (factura == null)
            return MapearDto(pago);

        return MapearDto(
            pago,
            factura);
    }

    public async Task<PagoDto>
        RegistrarAsync(
            PagoGuardarDto dto,
            string? usuarioId)
    {
        if (dto.Monto <= 0)
        {
            throw new ArgumentException(
                "El monto del pago debe ser mayor que cero.");
        }

        if (string.IsNullOrWhiteSpace(
                dto.FormaPago))
        {
            throw new ArgumentException(
                "La forma de pago es obligatoria.");
        }

        var factura =
            await _facturaRepository
                .ObtenerPorIdConRelacionesAsync(
                    dto.IdFactura);

        if (factura == null)
        {
            throw new ArgumentException(
                "La factura seleccionada no existe.");
        }

        if (factura.Estado == "Anulada")
        {
            throw new InvalidOperationException(
                "No se puede registrar un pago sobre una factura anulada.");
        }

        if (factura.Estado == "Pagada")
        {
            throw new InvalidOperationException(
                "La factura ya está completamente pagada.");
        }

        var totalPagado =
            factura.Pagos?
                .Sum(x => x.Monto) ?? 0m;

        var saldo =
            factura.Total - totalPagado;

        if (saldo <= 0)
        {
            throw new InvalidOperationException(
                "La factura no tiene saldo pendiente.");
        }

        if (dto.Monto > saldo)
        {
            throw new InvalidOperationException(
                $"El pago no puede superar el saldo pendiente de {saldo:C2}.");
        }

        var pago =
            await _repository
                .RegistrarConProcedimientoAsync(
                    dto.IdFactura,
                    dto.Monto,
                    dto.FormaPago.Trim(),
                    string.IsNullOrWhiteSpace(
                        dto.NumeroReferencia)
                        ? null
                        : dto.NumeroReferencia.Trim(),
                    usuarioId,
                    string.IsNullOrWhiteSpace(
                        dto.Observaciones)
                        ? null
                        : dto.Observaciones.Trim());

        var facturaActualizada =
            await _facturaRepository
                .ObtenerPorIdConRelacionesAsync(
                    dto.IdFactura);

        if (facturaActualizada != null)
        {
            return MapearDto(
                pago,
                facturaActualizada);
        }

        return MapearDto(pago);
    }

    private static PagoDto
        MapearDto(
            Pago pago)
    {
        if (pago.Factura == null)
            return MapearDto(
                pago,
                null);

        return MapearDto(
            pago,
            pago.Factura);
    }

    private static PagoDto
        MapearDto(
            Pago pago,
            Factura? factura)
    {
        var totalPagado =
            factura?.Pagos?
                .Sum(x => x.Monto)
            ?? pago.Monto;

        var totalFactura =
            factura?.Total ?? 0m;

        var saldo =
            Math.Max(
                0m,
                totalFactura - totalPagado);

        var cliente =
            factura?.Cliente == null
                ? null
                : $"{factura.Cliente.Nombre} " +
                  $"{factura.Cliente.Apellido1} " +
                  $"{factura.Cliente.Apellido2}"
                    .Trim();

        return new PagoDto
        {
            IdPago =
                pago.IdPago,

            IdFactura =
                pago.IdFactura,

            NumeroFactura =
                factura?.NumeroFactura
                ?? string.Empty,

            Cliente =
                cliente,

            Monto =
                pago.Monto,

            FormaPago =
                pago.FormaPago,

            NumeroReferencia =
                pago.NumeroReferencia,

            FechaPago =
                pago.FechaPago,

            UsuarioId =
                pago.UsuarioId,

            Observaciones =
                pago.Observaciones,

            TotalFactura =
                totalFactura,

            TotalPagado =
                totalPagado,

            SaldoPendiente =
                saldo,

            EstadoFactura =
                factura?.Estado
                ?? string.Empty
        };
    }
}