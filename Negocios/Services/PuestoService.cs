using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class PuestoService : IPuestoService
{
    private readonly IPuestoRepository _repository;

    public PuestoService(IPuestoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PuestoDto>> ObtenerTodosAsync()
    {
        var puestos = await _repository.ObtenerTodosAsync();

        return puestos.Select(MapearDto);
    }

    public async Task<IEnumerable<PuestoDto>> ObtenerActivosAsync()
    {
        var puestos = await _repository.ObtenerActivosAsync();

        return puestos.Select(MapearDto);
    }

    public async Task<PuestoDto?> ObtenerPorIdAsync(int id)
    {
        var puesto = await _repository.ObtenerPorIdAsync(id);

        return puesto == null
            ? null
            : MapearDto(puesto);
    }

    public async Task CrearAsync(PuestoGuardarDto dto)
    {
        Validar(dto);

        var puesto = new Puesto
        {
            Nombre = dto.Nombre.Trim(),
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(puesto);

        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        PuestoGuardarDto dto)
    {
        Validar(dto);

        var puesto = await _repository.ObtenerPorIdAsync(id);

        if (puesto == null)
            return false;

        puesto.Nombre = dto.Nombre.Trim();
        puesto.Activo = dto.Activo;

        _repository.Actualizar(puesto);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var puesto = await _repository.ObtenerPorIdAsync(id);

        if (puesto == null)
            return false;

        puesto.Activo = activo;

        _repository.Actualizar(puesto);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(PuestoGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre del puesto es obligatorio.");
        }
    }

    private static PuestoDto MapearDto(Puesto puesto)
    {
        return new PuestoDto
        {
            IdPuesto = puesto.IdPuesto,
            Nombre = puesto.Nombre,
            Activo = puesto.Activo
        };
    }
}