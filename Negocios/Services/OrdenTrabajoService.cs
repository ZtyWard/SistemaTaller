using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class OrdenTrabajoService : IOrdenTrabajoService
{
    private readonly IOrdenTrabajoRepository _repository;

    public OrdenTrabajoService(
        IOrdenTrabajoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerTodasAsync()
    {
        var ordenes = await _repository.ObtenerTodosAsync();

        return ordenes.Select(MapearDto);
    }

    public async Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerAbiertasAsync()
    {
        var ordenes =
            await _repository.ObtenerAbiertasAsync();

        return ordenes.Select(MapearDto);
    }

    public async Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerPorEstadoAsync(string estado)
    {
        var ordenes =
            await _repository.ObtenerPorEstadoAsync(estado);

        return ordenes.Select(MapearDto);
    }

    public async Task<OrdenTrabajoDto?> ObtenerPorIdAsync(int id)
    {
        var orden =
            await _repository.ObtenerCompletaAsync(id);

        return orden == null
            ? null
            : MapearDto(orden);
    }

    public async Task CrearAsync(
        OrdenTrabajoGuardarDto dto)
    {
        var orden = new OrdenTrabajo
        {
            IdCotizacion = dto.IdCotizacion,
            FechaInicio = DateTime.UtcNow,
            Estado = "Abierta",
            Observaciones = dto.Observaciones
        };

        await _repository.AgregarAsync(orden);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        OrdenTrabajoGuardarDto dto)
    {
        var orden =
            await _repository.ObtenerPorIdAsync(id);

        if (orden == null)
            return false;

        orden.IdCotizacion = dto.IdCotizacion;
        orden.Observaciones = dto.Observaciones;

        _repository.Actualizar(orden);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
            throw new ArgumentException(
                "El estado es obligatorio.");

        var orden =
            await _repository.ObtenerPorIdAsync(id);

        if (orden == null)
            return false;

        orden.Estado = estado;

        if (estado.Equals(
            "Finalizada",
            StringComparison.OrdinalIgnoreCase))
        {
            orden.FechaFin = DateTime.UtcNow;
        }

        _repository.Actualizar(orden);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> FinalizarAsync(int id)
    {
        var orden =
            await _repository.ObtenerPorIdAsync(id);

        if (orden == null)
            return false;

        orden.Estado = "Finalizada";
        orden.FechaFin = DateTime.UtcNow;

        _repository.Actualizar(orden);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static OrdenTrabajoDto MapearDto(
        OrdenTrabajo orden)
    {
        var vehiculo =
            orden.Cotizacion?
                .Diagnostico?
                .Recepcion?
                .Vehiculo;

        var cliente = vehiculo?.Cliente;

        return new OrdenTrabajoDto
        {
            IdOrdenTrabajo = orden.IdOrdenTrabajo,
            IdCotizacion = orden.IdCotizacion,

            Placa = vehiculo?.Placa ?? string.Empty,

            ClienteNombre = cliente != null
                ? $"{cliente.Nombre} {cliente.Apellido1}"
                : string.Empty,

            FechaInicio = orden.FechaInicio,
            FechaFin = orden.FechaFin,
            Estado = orden.Estado,
            Observaciones = orden.Observaciones,

            TotalCotizacion =
                orden.Cotizacion?.Total ?? 0
        };
    }
}