using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class RecepcionService : IRecepcionService
{
    private readonly IRecepcionRepository _repository;

    public RecepcionService(IRecepcionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RecepcionDto>> ObtenerTodasAsync()
    {
        var recepciones = await _repository.ObtenerTodosAsync();

        return recepciones.Select(MapearDto);
    }

    public async Task<IEnumerable<RecepcionDto>> ObtenerAbiertasAsync()
    {
        var recepciones = await _repository.ObtenerAbiertasAsync();

        return recepciones.Select(MapearDto);
    }

    public async Task<IEnumerable<RecepcionDto>> ObtenerPorVehiculoAsync(
        int idVehiculo)
    {
        var recepciones =
            await _repository.ObtenerPorVehiculoAsync(idVehiculo);

        return recepciones.Select(MapearDto);
    }

    public async Task<RecepcionDto?> ObtenerPorIdAsync(int id)
    {
        var recepcion =
            await _repository.ObtenerCompletaAsync(id);

        return recepcion == null
            ? null
            : MapearDto(recepcion);
    }

    public async Task CrearAsync(RecepcionGuardarDto dto)
    {
        var recepcion = new Recepcion
        {
            IdVehiculo = dto.IdVehiculo,
            IdEmpleado = dto.IdEmpleado,
            FechaRecepcion = DateTime.UtcNow,
            Kilometraje = dto.Kilometraje,
            NivelCombustible = dto.NivelCombustible,
            Observaciones = dto.Observaciones,
            Estado = "Recibido"
        };

        await _repository.AgregarAsync(recepcion);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarEstadoAsync(
        int id,
        string estado)
    {
        var recepcion =
            await _repository.ObtenerPorIdAsync(id);

        if (recepcion == null)
            return false;

        recepcion.Estado = estado;

        _repository.Actualizar(recepcion);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static RecepcionDto MapearDto(Recepcion recepcion)
    {
        return new RecepcionDto
        {
            IdRecepcion = recepcion.IdRecepcion,

            IdVehiculo = recepcion.IdVehiculo,

            Placa = recepcion.Vehiculo?.Placa
                ?? string.Empty,

            VehiculoDescripcion = recepcion.Vehiculo != null
                ? $"{recepcion.Vehiculo.Marca?.Nombre} " +
                  $"{recepcion.Vehiculo.Modelo?.Nombre}"
                : string.Empty,

            IdEmpleado = recepcion.IdEmpleado,

            EmpleadoNombre = recepcion.Empleado != null
                ? $"{recepcion.Empleado.Nombre} " +
                  $"{recepcion.Empleado.Apellido1}"
                : string.Empty,

            FechaRecepcion = recepcion.FechaRecepcion,
            Kilometraje = recepcion.Kilometraje,
            NivelCombustible = recepcion.NivelCombustible,
            Observaciones = recepcion.Observaciones,
            Estado = recepcion.Estado
        };
    }
}