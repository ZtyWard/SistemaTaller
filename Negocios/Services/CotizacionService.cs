using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class CotizacionService : ICotizacionService
{
    private readonly ICotizacionRepository _repository;

    public CotizacionService(ICotizacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CotizacionDto>> ObtenerTodasAsync()
    {
        var cotizaciones = await _repository.ObtenerTodosAsync();

        return cotizaciones.Select(MapearDto);
    }

    public async Task<IEnumerable<CotizacionDto>>
        ObtenerPendientesAsync()
    {
        var cotizaciones =
            await _repository.ObtenerPendientesAsync();

        return cotizaciones.Select(MapearDto);
    }

    public async Task<IEnumerable<CotizacionDto>>
        ObtenerPorDiagnosticoAsync(int idDiagnostico)
    {
        var cotizaciones =
            await _repository.ObtenerPorDiagnosticoAsync(idDiagnostico);

        return cotizaciones.Select(MapearDto);
    }

    public async Task<CotizacionDto?> ObtenerPorIdAsync(int id)
    {
        var cotizacion =
            await _repository.ObtenerCompletaAsync(id);

        return cotizacion == null
            ? null
            : MapearDto(cotizacion);
    }

    public async Task CrearAsync(CotizacionGuardarDto dto)
    {
        if (dto.Total < 0)
        {
            throw new ArgumentException(
                "El total de la cotización no puede ser negativo.");
        }

        var cotizacion = new Cotizacion
        {
            IdDiagnostico = dto.IdDiagnostico,
            Fecha = DateTime.UtcNow,
            Total = dto.Total,
            Estado = string.IsNullOrWhiteSpace(dto.Estado)
                ? "Pendiente"
                : dto.Estado
        };

        await _repository.AgregarAsync(cotizacion);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        CotizacionGuardarDto dto)
    {
        if (dto.Total < 0)
        {
            throw new ArgumentException(
                "El total de la cotización no puede ser negativo.");
        }

        var cotizacion =
            await _repository.ObtenerPorIdAsync(id);

        if (cotizacion == null)
            return false;

        cotizacion.IdDiagnostico = dto.IdDiagnostico;
        cotizacion.Total = dto.Total;
        cotizacion.Estado = string.IsNullOrWhiteSpace(dto.Estado)
            ? cotizacion.Estado
            : dto.Estado;

        _repository.Actualizar(cotizacion);

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

        var cotizacion =
            await _repository.ObtenerPorIdAsync(id);

        if (cotizacion == null)
            return false;

        cotizacion.Estado = estado;

        _repository.Actualizar(cotizacion);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static CotizacionDto MapearDto(
        Cotizacion cotizacion)
    {
        return new CotizacionDto
        {
            IdCotizacion = cotizacion.IdCotizacion,

            IdDiagnostico = cotizacion.IdDiagnostico,

            Placa = cotizacion.Diagnostico?
                .Recepcion?
                .Vehiculo?
                .Placa ?? string.Empty,

            Fecha = cotizacion.Fecha,

            Total = cotizacion.Total,

            Estado = cotizacion.Estado,

            TieneOrdenTrabajo =
                cotizacion.OrdenesTrabajo != null &&
                cotizacion.OrdenesTrabajo.Any()
        };
    }
}