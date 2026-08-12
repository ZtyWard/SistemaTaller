using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;
using System.Data.Common;

namespace Negocios.Services;

public class OrdenTrabajoService : IOrdenTrabajoService
{
    private readonly IOrdenTrabajoRepository _repository;

    public OrdenTrabajoService(
        IOrdenTrabajoRepository repository)
    {
        _repository = repository;
    }

    // =====================================================
    // OBTENER TODAS
    // =====================================================

    public async Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerTodasAsync()
    {
        var ordenes =
            await _repository.ObtenerTodosAsync();

        return ordenes.Select(MapearDto);
    }

    // =====================================================
    // OBTENER ABIERTAS
    // =====================================================

    public async Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerAbiertasAsync()
    {
        var ordenes =
            await _repository.ObtenerAbiertasAsync();

        return ordenes.Select(MapearDto);
    }

    // =====================================================
    // OBTENER POR ESTADO
    // =====================================================

    public async Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerPorEstadoAsync(string estado)
    {
        var ordenes =
            await _repository.ObtenerPorEstadoAsync(
                estado);

        return ordenes.Select(MapearDto);
    }

    // =====================================================
    // OBTENER POR ID
    // =====================================================

    public async Task<OrdenTrabajoDto?>
        ObtenerPorIdAsync(int id)
    {
        var orden =
            await _repository.ObtenerCompletaAsync(id);

        return orden == null
            ? null
            : MapearDto(orden);
    }

    // =====================================================
    // CREAR
    // =====================================================

    public async Task CrearAsync(
        OrdenTrabajoGuardarDto dto,
        string usuarioId)
    {
        if (string.IsNullOrWhiteSpace(usuarioId))
        {
            throw new ArgumentException(
                "El usuario autenticado es obligatorio.");
        }

        if (dto.IdCotizacion <= 0)
        {
            throw new ArgumentException(
                "La cotización es obligatoria.");
        }

        var orden = new OrdenTrabajo
        {
            IdCotizacion = dto.IdCotizacion,

            FechaInicio = DateTime.UtcNow,

            // Estado inicial oficial de la orden.
            Estado = "Registrada",

            Observaciones = dto.Observaciones
        };

        await _repository.CrearConUsuarioAsync(
            orden,
            usuarioId);
    }

    // =====================================================
    // ACTUALIZAR
    // =====================================================

    public async Task<bool> ActualizarAsync(
        int id,
        OrdenTrabajoGuardarDto dto,
        string usuarioId)
    {
        if (string.IsNullOrWhiteSpace(usuarioId))
        {
            throw new ArgumentException(
                "El usuario autenticado es obligatorio.");
        }

        if (dto.IdCotizacion <= 0)
        {
            throw new ArgumentException(
                "La cotización es obligatoria.");
        }

        return await _repository
            .ActualizarConUsuarioAsync(
                id,
                dto.IdCotizacion,
                dto.Observaciones,
                usuarioId);
    }

    // =====================================================
    // CAMBIAR ESTADO
    // =====================================================

    public async Task<bool> CambiarEstadoAsync(
        int id,
        string estado,
        string usuarioId,
        string? observaciones = null)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            throw new ArgumentException(
                "El estado es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(usuarioId))
        {
            throw new ArgumentException(
                "El usuario autenticado es obligatorio.");
        }

        try
        {
            await _repository.CambiarEstadoAsync(
                id,
                estado,
                usuarioId,
                observaciones);
        }
        catch (DbException ex)
        {
            throw new InvalidOperationException(
                ex.Message,
                ex);
        }

        return true;
    }

    // =====================================================
    // FINALIZAR
    // =====================================================

    public async Task<bool> FinalizarAsync(
        int id,
        string usuarioId)
    {
        return await CambiarEstadoAsync(
            id,
            "Finalizada",
            usuarioId);
    }

    // =====================================================
    // MAPEAR DTO
    // =====================================================

    private static OrdenTrabajoDto MapearDto(
        OrdenTrabajo orden)
    {
        var vehiculo =
            orden.Cotizacion?
                .Diagnostico?
                .Recepcion?
                .Vehiculo;

        var cliente =
            vehiculo?.Cliente;

        return new OrdenTrabajoDto
        {
            IdOrdenTrabajo =
                orden.IdOrdenTrabajo,

            IdCotizacion =
                orden.IdCotizacion,

            Placa =
                vehiculo?.Placa ?? string.Empty,

            ClienteNombre =
                cliente != null
                    ? $"{cliente.Nombre} {cliente.Apellido1}"
                    : string.Empty,

            FechaInicio =
                orden.FechaInicio,

            FechaFin =
                orden.FechaFin,

            Estado =
                orden.Estado,

            Observaciones =
                orden.Observaciones,

            TotalCotizacion =
                orden.Cotizacion?.Total ?? 0
        };
    }
}