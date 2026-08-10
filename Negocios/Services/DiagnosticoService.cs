using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class DiagnosticoService : IDiagnosticoService
{
    private readonly IDiagnosticoRepository _repository;

    public DiagnosticoService(IDiagnosticoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DiagnosticoDto>> ObtenerTodosAsync()
    {
        var diagnosticos = await _repository.ObtenerTodosAsync();

        return diagnosticos.Select(MapearDto);
    }

    public async Task<DiagnosticoDto?> ObtenerPorIdAsync(int id)
    {
        var diagnostico =
            await _repository.ObtenerCompletoAsync(id);

        return diagnostico == null
            ? null
            : MapearDto(diagnostico);
    }

    public async Task<IEnumerable<DiagnosticoDto>>
        ObtenerPorRecepcionAsync(int idRecepcion)
    {
        var diagnosticos =
            await _repository.ObtenerPorRecepcionAsync(idRecepcion);

        return diagnosticos.Select(MapearDto);
    }

    public async Task CrearAsync(DiagnosticoGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            throw new ArgumentException(
                "La descripción del diagnóstico es obligatoria.");
        }

        var diagnostico = new Diagnostico
        {
            IdRecepcion = dto.IdRecepcion,
            IdEmpleado = dto.IdEmpleado,
            Descripcion = dto.Descripcion,
            FechaDiagnostico = DateTime.UtcNow
        };

        await _repository.AgregarAsync(diagnostico);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        DiagnosticoGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Descripcion))
        {
            throw new ArgumentException(
                "La descripción del diagnóstico es obligatoria.");
        }

        var diagnostico =
            await _repository.ObtenerPorIdAsync(id);

        if (diagnostico == null)
            return false;

        diagnostico.IdRecepcion = dto.IdRecepcion;
        diagnostico.IdEmpleado = dto.IdEmpleado;
        diagnostico.Descripcion = dto.Descripcion;

        _repository.Actualizar(diagnostico);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static DiagnosticoDto MapearDto(
        Diagnostico diagnostico)
    {
        return new DiagnosticoDto
        {
            IdDiagnostico = diagnostico.IdDiagnostico,

            IdRecepcion = diagnostico.IdRecepcion,

            Placa = diagnostico.Recepcion?
                .Vehiculo?.Placa ?? string.Empty,

            IdEmpleado = diagnostico.IdEmpleado,

            EmpleadoNombre = diagnostico.Empleado != null
                ? $"{diagnostico.Empleado.Nombre} " +
                  $"{diagnostico.Empleado.Apellido1}"
                : string.Empty,

            Descripcion = diagnostico.Descripcion,

            FechaDiagnostico =
                diagnostico.FechaDiagnostico,

            EstadoRecepcion =
                diagnostico.Recepcion?.Estado ?? string.Empty
        };
    }
}