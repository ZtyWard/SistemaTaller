using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _repository;

    private readonly IProductoRepository
        _productoRepository;

    public VentaService(
        IVentaRepository repository,
        IProductoRepository productoRepository)
    {
        _repository =
            repository;

        _productoRepository =
            productoRepository;
    }

    // =====================================================
    // OBTENER TODOS
    // =====================================================

    public async Task<IEnumerable<VentaDto>>
        ObtenerTodosAsync()
    {
        var ventas =
            await _repository
                .ObtenerTodosAsync();

        return ventas.Select(MapearDto);
    }

    // =====================================================
    // POR CLIENTE
    // =====================================================

    public async Task<IEnumerable<VentaDto>>
        ObtenerPorClienteAsync(
            int idCliente)
    {
        var ventas =
            await _repository
                .ObtenerPorClienteAsync(
                    idCliente);

        return ventas.Select(MapearDto);
    }

    // =====================================================
    // POR ESTADO
    // =====================================================

    public async Task<IEnumerable<VentaDto>>
        ObtenerPorEstadoAsync(
            string estado)
    {
        var ventas =
            await _repository
                .ObtenerPorEstadoAsync(
                    estado);

        return ventas.Select(MapearDto);
    }

    // =====================================================
    // RECIENTES
    // =====================================================

    public async Task<IEnumerable<VentaDto>>
        ObtenerRecientesAsync(
            int cantidad = 10)
    {
        var ventas =
            await _repository
                .ObtenerRecientesAsync(
                    cantidad);

        return ventas.Select(MapearDto);
    }

    // =====================================================
    // POR ID
    // =====================================================

    public async Task<VentaDto?>
        ObtenerPorIdAsync(
            int id)
    {
        var venta =
            await _repository
                .ObtenerPorIdConDetallesAsync(
                    id);

        return venta == null
            ? null
            : MapearDto(venta);
    }

    // =====================================================
    // CREAR VENTA
    // =====================================================

    public async Task CrearAsync(
        VentaGuardarDto dto)
    {
        await ValidarVentaAsync(dto);

        var calculo =
            CalcularTotales(dto.Detalles);

        var venta = new Venta
        {
            NumeroVenta =
                string.IsNullOrWhiteSpace(
                    dto.NumeroVenta)
                    ? GenerarNumeroVenta()
                    : dto.NumeroVenta.Trim(),

            IdCliente =
                dto.IdCliente,

            IdVendedor =
                dto.IdVendedor,

            IdCajero =
                dto.IdCajero,

            FechaVenta =
                dto.FechaVenta == default
                    ? DateTime.Now
                    : dto.FechaVenta,

            Subtotal =
                calculo.Subtotal,

            Impuesto =
                calculo.Impuesto,

            Descuento =
                calculo.Descuento,

            Total =
                calculo.Total,

            FormaPago =
                dto.FormaPago?.Trim(),

            Estado =
                NormalizarEstado(dto.Estado),

            UsuarioId =
                dto.UsuarioId
        };

        foreach (
            var detalleDto
            in dto.Detalles)
        {
            var subtotal =
                (detalleDto.Cantidad *
                 detalleDto.PrecioUnitario)
                + detalleDto.Impuesto
                - detalleDto.Descuento;

            venta.Detalles.Add(
                new DetalleVenta
                {
                    IdProducto =
                        detalleDto.IdProducto,

                    Cantidad =
                        detalleDto.Cantidad,

                    PrecioUnitario =
                        detalleDto.PrecioUnitario,

                    Impuesto =
                        detalleDto.Impuesto,

                    Descuento =
                        detalleDto.Descuento,

                    Subtotal =
                        subtotal
                });
        }

        await _repository
            .AgregarAsync(venta);

        await _repository
            .GuardarCambiosAsync();
    }

    // =====================================================
    // ACTUALIZAR
    // =====================================================

    public async Task<bool>
        ActualizarAsync(
            int id,
            VentaGuardarDto dto)
    {
        await ValidarVentaAsync(dto);

        var venta =
            await _repository
                .ObtenerPorIdConDetallesAsync(
                    id);

        if (venta == null)
            return false;

        if (venta.Estado == "Completada")
        {
            throw new InvalidOperationException(
                "No se puede modificar una venta completada.");
        }

        if (venta.Estado == "Cancelada")
        {
            throw new InvalidOperationException(
                "No se puede modificar una venta cancelada.");
        }

        var calculo =
            CalcularTotales(dto.Detalles);

        venta.NumeroVenta =
            dto.NumeroVenta.Trim();

        venta.IdCliente =
            dto.IdCliente;

        venta.IdVendedor =
            dto.IdVendedor;

        venta.IdCajero =
            dto.IdCajero;

        venta.FechaVenta =
            dto.FechaVenta;

        venta.Subtotal =
            calculo.Subtotal;

        venta.Impuesto =
            calculo.Impuesto;

        venta.Descuento =
            calculo.Descuento;

        venta.Total =
            calculo.Total;

        venta.FormaPago =
            dto.FormaPago?.Trim();

        venta.Estado =
            NormalizarEstado(dto.Estado);

        venta.UsuarioId =
            dto.UsuarioId;

        // =================================================
        // REEMPLAZAR DETALLES
        // =================================================

        venta.Detalles.Clear();

        foreach (
            var detalleDto
            in dto.Detalles)
        {
            var subtotal =
                (detalleDto.Cantidad *
                 detalleDto.PrecioUnitario)
                + detalleDto.Impuesto
                - detalleDto.Descuento;

            venta.Detalles.Add(
                new DetalleVenta
                {
                    IdProducto =
                        detalleDto.IdProducto,

                    Cantidad =
                        detalleDto.Cantidad,

                    PrecioUnitario =
                        detalleDto.PrecioUnitario,

                    Impuesto =
                        detalleDto.Impuesto,

                    Descuento =
                        detalleDto.Descuento,

                    Subtotal =
                        subtotal
                });
        }

        _repository.Actualizar(
            venta);

        await _repository
            .GuardarCambiosAsync();

        return true;
    }

    // =====================================================
    // CAMBIAR ESTADO
    // =====================================================

    public async Task<bool>
        CambiarEstadoAsync(
            int id,
            string estado)
    {
        var nuevoEstado =
            NormalizarEstado(estado);

        var venta =
            await _repository
                .ObtenerPorIdConDetallesAsync(
                    id);

        if (venta == null)
            return false;

        // =================================================
        // COMPLETAR
        // =================================================

        if (nuevoEstado == "Completada")
        {
            if (venta.Estado ==
                "Completada")
            {
                throw new InvalidOperationException(
                    "La venta ya está completada.");
            }

            await _repository
                .CompletarVentaAsync(id);

            return true;
        }

        // =================================================
        // CANCELAR
        // =================================================

        if (nuevoEstado == "Cancelada")
        {
            if (venta.Estado ==
                "Completada")
            {
                throw new InvalidOperationException(
                    "No se puede cancelar una venta completada.");
            }
        }

        // =================================================
        // PENDIENTE / CANCELADA
        // =================================================

        venta.Estado =
            nuevoEstado;

        _repository.Actualizar(
            venta);

        await _repository
            .GuardarCambiosAsync();

        return true;
    }

    // =====================================================
    // VALIDAR VENTA
    // =====================================================

    private async Task
        ValidarVentaAsync(
            VentaGuardarDto dto)
    {
        if (dto.IdVendedor <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar un vendedor.");
        }

        if (dto.Detalles == null ||
            dto.Detalles.Count == 0)
        {
            throw new ArgumentException(
                "La venta debe tener al menos un detalle.");
        }

        NormalizarEstado(
            dto.Estado);

        foreach (
            var detalle
            in dto.Detalles)
        {
            if (detalle.IdProducto <= 0)
            {
                throw new ArgumentException(
                    "Todos los detalles deben tener un producto.");
            }

            if (detalle.Cantidad <= 0)
            {
                throw new ArgumentException(
                    "La cantidad debe ser mayor que cero.");
            }

            if (detalle.PrecioUnitario < 0)
            {
                throw new ArgumentException(
                    "El precio unitario no puede ser negativo.");
            }

            if (detalle.Impuesto < 0)
            {
                throw new ArgumentException(
                    "El impuesto no puede ser negativo.");
            }

            if (detalle.Descuento < 0)
            {
                throw new ArgumentException(
                    "El descuento no puede ser negativo.");
            }

            var producto =
                await _productoRepository
                    .ObtenerPorIdAsync(
                        detalle.IdProducto);

            if (producto == null)
            {
                throw new ArgumentException(
                    $"El producto {detalle.IdProducto} no existe.");
            }

            if (!producto.Activo)
            {
                throw new ArgumentException(
                    $"El producto '{producto.Nombre}' está inactivo.");
            }
        }
    }

    // =====================================================
    // CALCULAR TOTALES
    // =====================================================

    private static (
        decimal Subtotal,
        decimal Impuesto,
        decimal Descuento,
        decimal Total)
        CalcularTotales(
            IEnumerable<DetalleVentaGuardarDto>
                detalles)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal descuento = 0;

        foreach (var detalle in detalles)
        {
            subtotal +=
                detalle.Cantidad *
                detalle.PrecioUnitario;

            impuesto +=
                detalle.Impuesto;

            descuento +=
                detalle.Descuento;
        }

        var total =
            subtotal +
            impuesto -
            descuento;

        if (total < 0)
        {
            throw new ArgumentException(
                "El total de la venta no puede ser negativo.");
        }

        return (
            subtotal,
            impuesto,
            descuento,
            total);
    }

    // =====================================================
    // NORMALIZAR ESTADO
    // =====================================================

    private static string
        NormalizarEstado(
            string estado)
    {
        var valor =
            (estado ?? string.Empty)
                .Trim()
                .ToLowerInvariant();

        return valor switch
        {
            "pendiente" =>
                "Pendiente",

            "completada" =>
                "Completada",

            "cancelada" =>
                "Cancelada",

            _ =>
                throw new ArgumentException(
                    "El estado debe ser Pendiente, Completada o Cancelada.")
        };
    }

    // =====================================================
    // GENERAR NUMERO DE VENTA
    // =====================================================

    private static string
        GenerarNumeroVenta()
    {
        return
            $"V-{DateTime.Now:yyyyMMddHHmmssfff}";
    }

    // =====================================================
    // MAPEAR DTO
    // =====================================================

    private static VentaDto
        MapearDto(
            Venta venta)
    {
        return new VentaDto
        {
            IdVenta =
                venta.IdVenta,

            NumeroVenta =
                venta.NumeroVenta,

            IdCliente =
                venta.IdCliente,

            Cliente =
                venta.Cliente == null
                    ? string.Empty
                    : $"{venta.Cliente.Nombre} " +
                      $"{venta.Cliente.Apellido1} " +
                      $"{venta.Cliente.Apellido2}"
                        .Trim(),

            IdVendedor =
                venta.IdVendedor,

            IdCajero =
                venta.IdCajero,

            FechaVenta =
                venta.FechaVenta,

            Subtotal =
                venta.Subtotal,

            Impuesto =
                venta.Impuesto,

            Descuento =
                venta.Descuento,

            Total =
                venta.Total,

            FormaPago =
                venta.FormaPago,

            Estado =
                venta.Estado,

            UsuarioId =
                venta.UsuarioId,

            Detalles =
                venta.Detalles
                    .Select(x =>
                        new DetalleVentaDto
                        {
                            IdDetalleVenta =
                                x.IdDetalleVenta,

                            IdVenta =
                                x.IdVenta,

                            IdProducto =
                                x.IdProducto,

                            Producto =
                                x.Producto?.Nombre
                                ?? string.Empty,

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
        };
    }
}

