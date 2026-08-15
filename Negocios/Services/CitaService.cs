using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class CitaService : ICitaService
{
    private readonly ICitaRepository _citaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IServicioRepository _servicioRepository;
    private readonly IEmpleadoRepository _empleadoRepository;

    public CitaService(
        ICitaRepository citaRepository,
        IClienteRepository clienteRepository,
        IVehiculoRepository vehiculoRepository,
        IServicioRepository servicioRepository,
        IEmpleadoRepository empleadoRepository)
    {
        _citaRepository = citaRepository;
        _clienteRepository = clienteRepository;
        _vehiculoRepository = vehiculoRepository;
        _servicioRepository = servicioRepository;
        _empleadoRepository = empleadoRepository;
    }

    public async Task<IEnumerable<CitaDto>>
        ObtenerAgendaAsync(
            DateTime? fechaInicio,
            DateTime? fechaFin)
    {
        var citas =
            await _citaRepository.ObtenerAgendaAsync(
                fechaInicio,
                fechaFin);

        return citas.Select(MapearDto);
    }

    public async Task<CitaDto?>
        ObtenerPorIdAsync(int id)
    {
        var cita =
            await _citaRepository.ObtenerCompletaAsync(id);

        return cita == null
            ? null
            : MapearDto(cita);
    }

    public async Task<(bool Exitoso, string Mensaje)>
        CrearAsync(CitaFormularioDto dto)
    {
        var servicio =
            await _servicioRepository
                .ObtenerPorIdAsync(dto.IdServicio);

        if (servicio == null ||
            !servicio.Activo)
        {
            return (
                false,
                "El servicio seleccionado no existe o está inactivo.");
        }

        if (!servicio.DuracionEstimada.HasValue ||
            servicio.DuracionEstimada.Value <= 0)
        {
            return (
                false,
                "El servicio seleccionado no tiene una duración configurada.");
        }

        dto.FechaFin =
            dto.FechaInicio.AddMinutes(
                servicio.DuracionEstimada.Value);

        var validacion =
            await ValidarDatosAsync(
                dto,
                servicio);

        if (!validacion.Exitoso)
            return validacion;

        var conflicto =
            await _citaRepository.ExisteConflictoAsync(
                dto.FechaInicio,
                dto.FechaFin,
                dto.IdEmpleado,
                dto.Area);

        if (conflicto)
        {
            return (
                false,
                "Existe un conflicto de horario con el mecánico o área seleccionada.");
        }

        var cita = new Cita
        {
            NumeroCita = GenerarNumeroCita(),

            IdCliente = dto.IdCliente,

            IdVehiculo = dto.IdVehiculo,

            IdServicio = dto.IdServicio,

            IdEmpleado = dto.IdEmpleado,

            Area = Normalizar(dto.Area),

            FechaInicio = dto.FechaInicio,

            FechaFin = dto.FechaFin,

            Estado = string.IsNullOrWhiteSpace(dto.Estado)
                ? "Programada"
                : dto.Estado,

            Observaciones =
                Normalizar(dto.Observaciones)
        };

        await _citaRepository.AgregarAsync(cita);

        await _citaRepository.GuardarCambiosAsync();

        return (
            true,
            $"Cita {cita.NumeroCita} creada correctamente.");
    }

    public async Task<(bool Exitoso, string Mensaje)>
        ActualizarAsync(
            int id,
            CitaFormularioDto dto)
    {
        var cita =
            await _citaRepository.ObtenerPorIdAsync(id);

        if (cita == null)
        {
            return (
                false,
                "La cita no existe.");
        }

        var servicio =
            await _servicioRepository
                .ObtenerPorIdAsync(dto.IdServicio);

        if (servicio == null ||
            !servicio.Activo)
        {
            return (
                false,
                "El servicio seleccionado no existe o está inactivo.");
        }

        if (!servicio.DuracionEstimada.HasValue ||
            servicio.DuracionEstimada.Value <= 0)
        {
            return (
                false,
                "El servicio seleccionado no tiene una duración configurada.");
        }

        dto.FechaFin =
            dto.FechaInicio.AddMinutes(
                servicio.DuracionEstimada.Value);

        var validacion =
            await ValidarDatosAsync(
                dto,
                servicio);

        if (!validacion.Exitoso)
            return validacion;

        var conflicto =
            await _citaRepository.ExisteConflictoAsync(
                dto.FechaInicio,
                dto.FechaFin,
                dto.IdEmpleado,
                dto.Area,
                id);

        if (conflicto)
        {
            return (
                false,
                "Existe un conflicto de horario con el mecánico o área seleccionada.");
        }

        cita.IdCliente =
            dto.IdCliente;

        cita.IdVehiculo =
            dto.IdVehiculo;

        cita.IdServicio =
            dto.IdServicio;

        cita.IdEmpleado =
            dto.IdEmpleado;

        cita.Area =
            Normalizar(dto.Area);

        cita.FechaInicio =
            dto.FechaInicio;

        cita.FechaFin =
            dto.FechaFin;

        cita.Estado =
            string.IsNullOrWhiteSpace(dto.Estado)
                ? "Programada"
                : dto.Estado;

        cita.Observaciones =
            Normalizar(dto.Observaciones);

        _citaRepository.Actualizar(cita);

        await _citaRepository.GuardarCambiosAsync();

        return (
            true,
            $"Cita {cita.NumeroCita} actualizada correctamente.");
    }

    public async Task<(bool Exitoso, string Mensaje)>
        CancelarAsync(int id)
    {
        var cita =
            await _citaRepository
                .ObtenerPorIdAsync(id);

        if (cita == null)
        {
            return (
                false,
                "La cita no existe.");
        }

        if (cita.Estado == "Cancelada")
        {
            return (
                false,
                "La cita ya está cancelada.");
        }

        cita.Estado = "Cancelada";

        _citaRepository.Actualizar(cita);

        await _citaRepository.GuardarCambiosAsync();

        return (
            true,
            "Cita cancelada correctamente.");
    }

    private async Task<(bool Exitoso, string Mensaje)>
        ValidarDatosAsync(
            CitaFormularioDto dto,
            Datos.Models.Servicio servicio)
    {
        if (dto.FechaInicio == default)
        {
            return (
                false,
                "Debe indicar la fecha y hora de inicio.");
        }

        if (dto.FechaInicio < DateTime.Now.AddMinutes(-5))
        {
            return (
                false,
                "La cita no puede programarse en una fecha pasada.");
        }

        if (dto.FechaFin <= dto.FechaInicio)
        {
            return (
                false,
                "La fecha de finalización debe ser posterior al inicio.");
        }

        if (dto.IdEmpleado == null &&
            string.IsNullOrWhiteSpace(dto.Area))
        {
            return (
                false,
                "Debe asignar un mecánico o un área.");
        }

        var cliente =
            await _clienteRepository
                .ObtenerPorIdAsync(dto.IdCliente);

        if (cliente == null ||
            !cliente.Activo)
        {
            return (
                false,
                "El cliente seleccionado no existe o está inactivo.");
        }

        var vehiculo =
            await _vehiculoRepository
                .ObtenerPorIdAsync(dto.IdVehiculo);

        if (vehiculo == null ||
            !vehiculo.Activo)
        {
            return (
                false,
                "El vehículo seleccionado no existe o está inactivo.");
        }

        if (vehiculo.IdCliente != dto.IdCliente)
        {
            return (
                false,
                "El vehículo seleccionado no pertenece al cliente indicado.");
        }

        if (servicio == null ||
            !servicio.Activo)
        {
            return (
                false,
                "El servicio seleccionado no existe o está inactivo.");
        }

        if (dto.IdEmpleado.HasValue)
        {
            var empleado =
                await _empleadoRepository
                    .ObtenerPorIdAsync(
                        dto.IdEmpleado.Value);

            if (empleado == null ||
                !empleado.Activo)
            {
                return (
                    false,
                    "El mecánico seleccionado no existe o está inactivo.");
            }
        }

        return (
            true,
            string.Empty);
    }

    private static CitaDto MapearDto(
        Cita cita)
    {
        var clienteNombre =
            cita.Cliente == null
                ? string.Empty
                : $"{cita.Cliente.Nombre} {cita.Cliente.Apellido1}".Trim();

        var empleadoNombre =
            cita.Empleado == null
                ? string.Empty
                : $"{cita.Empleado.Nombre} {cita.Empleado.Apellido1}".Trim();

        return new CitaDto
        {
            IdCita =
                cita.IdCita,

            NumeroCita =
                cita.NumeroCita,

            IdCliente =
                cita.IdCliente,

            IdVehiculo =
                cita.IdVehiculo,

            IdServicio =
                cita.IdServicio,

            IdEmpleado =
                cita.IdEmpleado,

            Area =
                cita.Area,

            FechaInicio =
                cita.FechaInicio,

            FechaFin =
                cita.FechaFin,

            Estado =
                cita.Estado,

            Observaciones =
                cita.Observaciones,

            ClienteNombre =
                clienteNombre,

            VehiculoPlaca =
                cita.Vehiculo?.Placa
                ?? string.Empty,

            ServicioNombre =
                cita.Servicio?.Nombre
                ?? string.Empty,

            EmpleadoNombre =
                empleadoNombre
        };
    }

    private static string GenerarNumeroCita()
    {
        var fecha =
            DateTime.Now.ToString(
                "yyyyMMddHHmmssfff");

        var codigo =
            Guid.NewGuid()
                .ToString("N")
                .Substring(0, 6)
                .ToUpperInvariant();

        return $"CIT-{fecha}-{codigo}";
    }

    private static string? Normalizar(
        string? valor)
    {
        return string.IsNullOrWhiteSpace(valor)
            ? null
            : valor.Trim();
    }
}