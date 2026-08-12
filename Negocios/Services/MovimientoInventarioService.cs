using Datos.Interfaces;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class MovimientoInventarioService
    : IMovimientoInventarioService
{
    private readonly IMovimientoInventarioRepository
        _movimientoRepository;

    public MovimientoInventarioService(
        IMovimientoInventarioRepository movimientoRepository)
    {
        _movimientoRepository =
            movimientoRepository;
    }

    // =====================================================
    // OBTENER TODOS
    // =====================================================

    public async Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerTodosAsync()
    {
        var movimientos =
            await _movimientoRepository
                .ObtenerTodosAsync();

        return movimientos.Select(MapearDto);
    }

    // =====================================================
    // OBTENER POR PRODUCTO
    // =====================================================

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

    // =====================================================
    // OBTENER POR TIPO
    // =====================================================

    public async Task<IEnumerable<MovimientoInventarioDto>>
        ObtenerPorTipoAsync(
            string tipoMovimiento)
    {
        var movimientos =
            await _movimientoRepository
                .ObtenerPorTipoAsync(
                    NormalizarTipo(tipoMovimiento));

        return movimientos.Select(MapearDto);
    }

    // =====================================================
    // OBTENER RECIENTES
    // =====================================================

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

    // =====================================================
    // OBTENER POR ID
    // =====================================================

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

    // =====================================================
    // CREAR MOVIMIENTO
    // =====================================================

    public async Task CrearAsync(
        MovimientoInventarioGuardarDto dto)
    {
        Validar(dto);

        var tipo =
            NormalizarTipo(
                dto.TipoMovimiento);

        try
        {
            await _movimientoRepository
                .RegistrarMovimientoAsync(
                    dto.IdProducto,
                    tipo,
                    dto.Cantidad,
                    dto.Observacion?.Trim());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                ObtenerMensajeError(ex),
                ex);
        }
    }

    // =====================================================
    // VALIDACIONES
    // =====================================================

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

    // =====================================================
    // NORMALIZAR TIPO
    // =====================================================

    private static string NormalizarTipo(
        string tipoMovimiento)
    {
        var tipo =
            tipoMovimiento
                .Trim()
                .ToLowerInvariant();

        return tipo switch
        {
            "entrada" =>
                "Entrada",

            "salida" =>
                "Salida",

            "ajuste" =>
                "Ajuste",

            _ =>
                throw new ArgumentException(
                    "El tipo de movimiento debe ser Entrada, Salida o Ajuste.")
        };
    }

    // =====================================================
    // MAPEAR DTO
    // =====================================================

    private static MovimientoInventarioDto
        MapearDto(
            Datos.Models.MovimientoInventario movimiento)
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

    // =====================================================
    // EXTRAER MENSAJE DE SQL SERVER
    // =====================================================

    private static string ObtenerMensajeError(
        Exception ex)
    {
        var actual = ex;

        while (actual.InnerException != null)
        {
            actual = actual.InnerException;
        }

        return actual.Message;
    }
}