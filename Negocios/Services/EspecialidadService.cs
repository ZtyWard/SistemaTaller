using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class EspecialidadService : IEspecialidadService
{
    private readonly IEspecialidadRepository _repository;

    public EspecialidadService(
        IEspecialidadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EspecialidadDto>>
        ObtenerTodosAsync()
    {
        var especialidades =
            await _repository.ObtenerTodosAsync();

        return especialidades.Select(MapearDto);
    }

    public async Task<IEnumerable<EspecialidadDto>>
        ObtenerActivasAsync()
    {
        var especialidades =
            await _repository.ObtenerActivasAsync();

        return especialidades.Select(MapearDto);
    }

    public async Task<EspecialidadDto?>
        ObtenerPorIdAsync(int id)
    {
        var especialidad =
            await _repository.ObtenerPorIdAsync(id);

        return especialidad == null
            ? null
            : MapearDto(especialidad);
    }

    public async Task CrearAsync(
        EspecialidadGuardarDto dto)
    {
        Validar(dto);

        var especialidad = new Especialidad
        {
            Nombre = dto.Nombre.Trim(),
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(especialidad);

        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        EspecialidadGuardarDto dto)
    {
        Validar(dto);

        var especialidad =
            await _repository.ObtenerPorIdAsync(id);

        if (especialidad == null)
            return false;

        especialidad.Nombre = dto.Nombre.Trim();
        especialidad.Activo = dto.Activo;

        _repository.Actualizar(especialidad);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var especialidad =
            await _repository.ObtenerPorIdAsync(id);

        if (especialidad == null)
            return false;

        especialidad.Activo = activo;

        _repository.Actualizar(especialidad);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(
        EspecialidadGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre de la especialidad es obligatorio.");
        }
    }

    private static EspecialidadDto MapearDto(
        Especialidad especialidad)
    {
        return new EspecialidadDto
        {
            IdEspecialidad = especialidad.IdEspecialidad,
            Nombre = especialidad.Nombre,
            Activo = especialidad.Activo
        };
    }
}