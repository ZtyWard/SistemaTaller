using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class FacturaService
    : IFacturaService
{
    private readonly IFacturaRepository _repository;

    public FacturaService(
        IFacturaRepository repository)
    {
        _repository = repository;
    }

    // =====================================================
    // OBTENER TODAS
    // =====================================================

    public async Task<IEnumerable<FacturaDto>>
        ObtenerTodosAsync()
    {
        var facturas =
            await _repository.ObtenerTodosAsync();

        return facturas
            .Select(MapearDto);
    }

    // =====================================================
    // OBTENER PENDIENTES
    // =====================================================

    public async Task<IEnumerable<FacturaDto>>
        ObtenerPendientesAsync()
    {
        var facturas =
            await _repository
                .ObtenerPendientesAsync();

        return facturas
            .Select(MapearDto);
    }

    // =====================================================
    // OBTENER POR ID
    // =====================================================

    public async Task<FacturaDto?>
        ObtenerPorIdAsync(int id)
    {
        var factura =
            await _repository
                .ObtenerPorIdConRelacionesAsync(id);

        return factura == null
            ? null
            : MapearDto(factura);
    }

    // =====================================================
    // CREAR
    // =====================================================

    public async Task CrearAsync(
        FacturaGuardarDto dto)
    {
        Validar(dto);

        var existente =
            await _repository
                .ObtenerPorNumeroAsync(
                    dto.NumeroFactura.Trim());

        if (existente != null)
        {
            throw new InvalidOperationException(
                "Ya existe una factura con ese número.");
        }

        var factura = new Factura
        {
            NumeroFactura =
                dto.NumeroFactura.Trim(),

            IdCliente =
                dto.IdCliente,

            IdOrdenTrabajo =
                dto.IdOrdenTrabajo,

            IdVenta =
                dto.IdVenta,

            FechaEmision =
                dto.FechaEmision,

            Subtotal =
                dto.Subtotal,

            Impuesto =
                dto.Impuesto,

            Descuento =
                dto.Descuento,

            Total =
                dto.Total,

            Estado =
                string.IsNullOrWhiteSpace(dto.Estado)
                    ? "Pendiente"
                    : dto.Estado.Trim()
        };

        await _repository.AgregarAsync(
            factura);

        await _repository
            .GuardarCambiosAsync();
    }

    // =====================================================
    // ACTUALIZAR
    // =====================================================

    public async Task<bool> ActualizarAsync(
        int id,
        FacturaGuardarDto dto)
    {
        Validar(dto);

        var factura =
            await _repository
                .ObtenerPorIdAsync(id);

        if (factura == null)
            return false;

        var existente =
            await _repository
                .ObtenerPorNumeroAsync(
                    dto.NumeroFactura.Trim());

        if (existente != null &&
            existente.IdFactura != id)
        {
            throw new InvalidOperationException(
                "Ya existe otra factura con ese número.");
        }

        if (factura.Estado == "Pagada")
        {
            throw new InvalidOperationException(
                "No se puede editar una factura pagada.");
        }

        factura.NumeroFactura =
            dto.NumeroFactura.Trim();

        factura.IdCliente =
            dto.IdCliente;

        factura.IdOrdenTrabajo =
            dto.IdOrdenTrabajo;

        factura.IdVenta =
            dto.IdVenta;

        factura.FechaEmision =
            dto.FechaEmision;

        factura.Subtotal =
            dto.Subtotal;

        factura.Impuesto =
            dto.Impuesto;

        factura.Descuento =
            dto.Descuento;

        factura.Total =
            dto.Total;

        factura.Estado =
            dto.Estado;

        _repository.Actualizar(
            factura);

        await _repository
            .GuardarCambiosAsync();

        return true;
    }

    // =====================================================
    // ANULAR
    // =====================================================

    public async Task<bool> AnularAsync(
        int id)
    {
        var factura =
            await _repository
                .ObtenerPorIdAsync(id);

        if (factura == null)
            return false;

        await _repository
            .AnularAsync(id);

        return true;
    }

    // =====================================================
    // VALIDACIONES
    // =====================================================

    private static void Validar(
        FacturaGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(
                dto.NumeroFactura))
        {
            throw new ArgumentException(
                "El número de factura es obligatorio.");
        }

        if (dto.Subtotal < 0 ||
            dto.Impuesto < 0 ||
            dto.Descuento < 0 ||
            dto.Total < 0)
        {
            throw new ArgumentException(
                "Los montos no pueden ser negativos.");
        }

        var totalCalculado =
            dto.Subtotal +
            dto.Impuesto -
            dto.Descuento;

        if (Math.Abs(
                totalCalculado - dto.Total) > 0.01m)
        {
            throw new ArgumentException(
                "El total no coincide con subtotal + impuesto - descuento.");
        }
    }

    // =====================================================
    // MAPEAR DTO
    // =====================================================

    private static FacturaDto
        MapearDto(Factura factura)
    {
        var totalPagado =
            factura.Pagos?
                .Sum(x => x.Monto) ?? 0m;

        return new FacturaDto
        {
            IdFactura =
                factura.IdFactura,

            NumeroFactura =
                factura.NumeroFactura,

            IdCliente =
                factura.IdCliente,

            Cliente =
                factura.Cliente == null
                    ? null
                    : $"{factura.Cliente.Nombre} " +
                      $"{factura.Cliente.Apellido1} " +
                      $"{factura.Cliente.Apellido2}".Trim(),

            IdOrdenTrabajo =
                factura.IdOrdenTrabajo,

            IdVenta =
                factura.IdVenta,

            FechaEmision =
                factura.FechaEmision,

            Subtotal =
                factura.Subtotal,

            Impuesto =
                factura.Impuesto,

            Descuento =
                factura.Descuento,

            Total =
                factura.Total,

            Estado =
                factura.Estado,

            TotalPagado =
                totalPagado,

            SaldoPendiente =
                Math.Max(
                    0m,
                    factura.Total - totalPagado),

            // =================================================
            // DETALLES DE LA VENTA
            // =================================================

            DetallesVenta =
                factura.Venta?.Detalles?
                    .Select(x => new DetalleVentaDto
                    {
                        IdDetalleVenta =
                            x.IdDetalleVenta,

                        IdVenta =
                            x.IdVenta,

                        IdProducto =
                            x.IdProducto,

                        Producto =
                            x.Producto?.Nombre
                            ?? "Producto",

                        Cantidad =
                            x.Cantidad,

                        PrecioUnitario =
                            x.PrecioUnitario,

                        Impuesto =
                            x.Impuesto,

                        Descuento =
                            x.Descuento,

                        Subtotal =
                            x.Subtotal
                    })
                    .ToList()
                ?? new List<DetalleVentaDto>()
        };
    }
}