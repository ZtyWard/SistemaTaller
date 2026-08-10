using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class ServicioService : IServicioService
{
    private readonly IServicioRepository _repository;

    public ServicioService(
        IServicioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ServicioDto>>
        ObtenerTodosAsync()
    {
        var servicios =
            await _repository.ObtenerTodosAsync();

        return servicios.Select(MapearDto);
    }

    public async Task<IEnumerable<ServicioDto>>
        ObtenerActivosAsync()
    {
        var servicios =
            await _repository.ObtenerActivosAsync();

        return servicios.Select(MapearDto);
    }

    public async Task<ServicioDto?> ObtenerPorIdAsync(
        int id)
    {
        var servicio =
            await _repository.ObtenerPorIdAsync(id);

        return servicio == null
            ? null
            : MapearDto(servicio);
    }

    public async Task CrearAsync(
        ServicioGuardarDto dto)
    {
        Validar(dto);

        var servicio = new Servicio
        {
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            Precio = dto.Precio,
            DuracionEstimada = dto.DuracionEstimada,
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(servicio);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ServicioGuardarDto dto)
    {
        Validar(dto);

        var servicio =
            await _repository.ObtenerPorIdAsync(id);

        if (servicio == null)
            return false;

        servicio.Nombre = dto.Nombre.Trim();
        servicio.Descripcion = dto.Descripcion?.Trim();
        servicio.Precio = dto.Precio;
        servicio.DuracionEstimada = dto.DuracionEstimada;
        servicio.Activo = dto.Activo;

        _repository.Actualizar(servicio);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var servicio =
            await _repository.ObtenerPorIdAsync(id);

        if (servicio == null)
            return false;

        servicio.Activo = activo;

        _repository.Actualizar(servicio);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(
        ServicioGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new ArgumentException(
                "El nombre del servicio es obligatorio.");

        if (dto.Precio < 0)
            throw new ArgumentException(
                "El precio no puede ser negativo.");

        if (dto.DuracionEstimada < 0)
            throw new ArgumentException(
                "La duración no puede ser negativa.");
    }

    private static ServicioDto MapearDto(
        Servicio servicio)
    {
        return new ServicioDto
        {
            IdServicio = servicio.IdServicio,
            Nombre = servicio.Nombre,
            Descripcion = servicio.Descripcion,
            Precio = servicio.Precio,
            DuracionEstimada = servicio.DuracionEstimada,
            Activo = servicio.Activo
        };
    }
}