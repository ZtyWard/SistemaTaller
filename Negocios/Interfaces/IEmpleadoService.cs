using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IEmpleadoService
{
    Task<IEnumerable<EmpleadoDto>> ObtenerTodosAsync();

    Task<IEnumerable<EmpleadoDto>> ObtenerActivosAsync();

    Task<IEnumerable<EmpleadoDto>> ObtenerPorPuestoAsync(
        int idPuesto);

    Task<IEnumerable<EmpleadoDto>> ObtenerPorEspecialidadAsync(
        int idEspecialidad);

    Task<EmpleadoDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(EmpleadoGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        EmpleadoGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}