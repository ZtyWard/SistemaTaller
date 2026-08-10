using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class EmpleadoService : IEmpleadoService
{
    private readonly IEmpleadoRepository _repository;

    public EmpleadoService(
        IEmpleadoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmpleadoDto>>
        ObtenerTodosAsync()
    {
        var empleados =
            await _repository.ObtenerTodosAsync();

        return empleados.Select(MapearDto);
    }

    public async Task<IEnumerable<EmpleadoDto>>
        ObtenerActivosAsync()
    {
        var empleados =
            await _repository.ObtenerActivosAsync();

        return empleados.Select(MapearDto);
    }

    public async Task<IEnumerable<EmpleadoDto>>
        ObtenerPorPuestoAsync(int idPuesto)
    {
        var empleados =
            await _repository.ObtenerPorPuestoAsync(idPuesto);

        return empleados.Select(MapearDto);
    }

    public async Task<IEnumerable<EmpleadoDto>>
        ObtenerPorEspecialidadAsync(
            int idEspecialidad)
    {
        var empleados =
            await _repository
                .ObtenerPorEspecialidadAsync(idEspecialidad);

        return empleados.Select(MapearDto);
    }

    public async Task<EmpleadoDto?> ObtenerPorIdAsync(int id)
    {
        var empleado =
            await _repository.ObtenerCompletoAsync(id);

        return empleado == null
            ? null
            : MapearDto(empleado);
    }

    public async Task CrearAsync(
        EmpleadoGuardarDto dto)
    {
        Validar(dto);

        var empleado = new Empleado
        {
            Cedula = dto.Cedula.Trim(),
            Nombre = dto.Nombre.Trim(),
            Apellido1 = dto.Apellido1.Trim(),
            Apellido2 = dto.Apellido2?.Trim(),
            Telefono = dto.Telefono?.Trim(),
            Correo = dto.Correo?.Trim(),
            IdPuesto = dto.IdPuesto,
            IdEspecialidad = dto.IdEspecialidad,
            Salario = dto.Salario,
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(empleado);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        EmpleadoGuardarDto dto)
    {
        Validar(dto);

        var empleado =
            await _repository.ObtenerPorIdAsync(id);

        if (empleado == null)
            return false;

        empleado.Cedula = dto.Cedula.Trim();
        empleado.Nombre = dto.Nombre.Trim();
        empleado.Apellido1 = dto.Apellido1.Trim();
        empleado.Apellido2 = dto.Apellido2?.Trim();
        empleado.Telefono = dto.Telefono?.Trim();
        empleado.Correo = dto.Correo?.Trim();
        empleado.IdPuesto = dto.IdPuesto;
        empleado.IdEspecialidad = dto.IdEspecialidad;
        empleado.Salario = dto.Salario;
        empleado.Activo = dto.Activo;

        _repository.Actualizar(empleado);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var empleado =
            await _repository.ObtenerPorIdAsync(id);

        if (empleado == null)
            return false;

        empleado.Activo = activo;

        _repository.Actualizar(empleado);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(
        EmpleadoGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Cedula))
            throw new ArgumentException(
                "La cédula es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            throw new ArgumentException(
                "El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(dto.Apellido1))
            throw new ArgumentException(
                "El primer apellido es obligatorio.");

        if (dto.IdPuesto <= 0)
            throw new ArgumentException(
                "Debe seleccionar un puesto.");

        if (dto.IdEspecialidad <= 0)
            throw new ArgumentException(
                "Debe seleccionar una especialidad.");

        if (dto.Salario < 0)
            throw new ArgumentException(
                "El salario no puede ser negativo.");
    }

    private static EmpleadoDto MapearDto(
        Empleado empleado)
    {
        return new EmpleadoDto
        {
            IdEmpleado = empleado.IdEmpleado,

            Cedula = empleado.Cedula,

            NombreCompleto =
                $"{empleado.Nombre} " +
                $"{empleado.Apellido1}" +
                $"{(string.IsNullOrWhiteSpace(empleado.Apellido2)
                    ? ""
                    : $" {empleado.Apellido2}")}",

            Telefono = empleado.Telefono,

            Correo = empleado.Correo,

            IdPuesto = empleado.IdPuesto,

            Puesto =
                empleado.Puesto?.Nombre ?? string.Empty,

            IdEspecialidad = empleado.IdEspecialidad,

            Especialidad =
                empleado.Especialidad?.Nombre ?? string.Empty,

            Salario = empleado.Salario,

            Activo = empleado.Activo
        };
    }
}