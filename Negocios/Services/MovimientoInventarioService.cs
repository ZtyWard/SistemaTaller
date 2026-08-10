using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class MovimientoInventarioService
    : IMovimientoInventarioService
{
    private readonly IMovimientoInventarioRepository
        _movimientoRepository;

    private readonly IProductoRepository
        _productoRepository;

    public MovimientoInventarioService(
        IMovimientoInventarioRepository movimientoRepository,
        IProductoRepository productoRepository)
    {
        _movimientoRepository =
            movimientoRepository;

        _productoRepository =
            productoRepository;
    }

    public async Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerTodosAsync()
    {
        var movimientos =
            await _movimientoRepository
                .ObtenerTodosAsync();

        return movimientos.Select(MapearDto);
    }

    public async Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerPorProductoAsync(
            int idProducto)
    {
        var movimientos =
            await _movimientoRepository
                .ObtenerPorProductoAsync(
                    idProducto);

        return movimientos.Select(MapearDto);
    }

    public async Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerPorTipoAsync(
            string tipoMovimiento)
    {
        var movimientos =
            await _movimientoRepository
                .ObtenerPorTipoAsync(
                    tipoMovimiento);

        return movimientos.Select(MapearDto);
    }

    public async Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerRecientesAsync(
            int cantidad = 10)
    {
        var movimientos =
            await _movimientoRepository
                .ObtenerRecientesAsync(
                    cantidad);

        return movimientos.Select(MapearDto);
    }

    public async Task<MovimientoInventarioDto?>
        ObtenerPorIdAsync(int id)
    {
        var movimiento =
            await _movimientoRepository
                .ObtenerPorIdAsync(id);

        return movimiento == null
            ? null
            : MapearDto(movimiento);
    }

    public async Task CrearAsync(
        MovimientoInventarioGuardarDto dto)
    {
        Validar(dto);

        var producto =
            await _productoRepository
                .ObtenerPorIdAsync(
                    dto.IdProducto);

        if (producto == null)
        {
            throw new ArgumentException(
                "El producto indicado no existe.");
        }

        int nuevoStock =
            CalcularNuevoStock(
                producto.Stock,
                dto.TipoMovimiento,
                dto.Cantidad);

        producto.Stock = nuevoStock;

        _productoRepository.Actualizar(
            producto);

        var movimiento =
            new MovimientoInventario
            {
                IdProducto =
                    dto.IdProducto,

                TipoMovimiento =
                    NormalizarTipo(
                        dto.TipoMovimiento),

                Cantidad =
                    dto.Cantidad,

                FechaMovimiento =
                    DateTime.Now,

                Observacion =
                    dto.Observacion?.Trim(),

                Producto =
                    null
            };

        await _movimientoRepository
            .AgregarAsync(movimiento);

        await _productoRepository
            .GuardarCambiosAsync();
    }

    private static int CalcularNuevoStock(
        int stockActual,
        string tipoMovimiento,
        int cantidad)
    {
        var tipo =
            NormalizarTipo(tipoMovimiento);

        return tipo switch
        {
            "Entrada" =>
                stockActual + cantidad,

            "Salida" =>
                stockActual - cantidad
                < 0
                    ? throw new ArgumentException(
                        "No hay suficiente stock para realizar la salida.")
                    : stockActual - cantidad,

            "Ajuste" =>
                cantidad,

            _ =>
                throw new ArgumentException(
                    "Tipo de movimiento no válido.")
        };
    }

    private static void Validar(
        MovimientoInventarioGuardarDto dto)
    {
        if (dto.IdProducto <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar un producto.");
        }

        if (string.IsNullOrWhiteSpace(
                dto.TipoMovimiento))
        {
            throw new ArgumentException(
                "El tipo de movimiento es obligatorio.");
        }

        if (dto.Cantidad <= 0)
        {
            throw new ArgumentException(
                "La cantidad debe ser mayor que cero.");
        }

        NormalizarTipo(
            dto.TipoMovimiento);
    }

    private static string NormalizarTipo(
        string tipoMovimiento)
    {
        var tipo =
            tipoMovimiento.Trim()
                .ToLowerInvariant();

        return tipo switch
        {
            "entrada" => "Entrada",

            "salida" => "Salida",

            "ajuste" => "Ajuste",

            _ => throw new ArgumentException(
                "El tipo de movimiento debe ser Entrada, Salida o Ajuste.")
        };
    }

    private static MovimientoInventarioDto
        MapearDto(
            MovimientoInventario movimiento)
    {
        return new MovimientoInventarioDto
        {
            IdMovimiento =
                movimiento.IdMovimiento,

            IdProducto =
                movimiento.IdProducto,

            Producto =
                movimiento.Producto?.Nombre
                ?? string.Empty,

            TipoMovimiento =
                movimiento.TipoMovimiento,

            Cantidad =
                movimiento.Cantidad,

            FechaMovimiento =
                movimiento.FechaMovimiento,

            Observacion =
                movimiento.Observacion
        };
    }
}