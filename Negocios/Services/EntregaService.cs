using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class EntregaService : IEntregaService
{
    private readonly IEntregaRepository _entregaRepository;
    private readonly IOrdenTrabajoRepository _ordenTrabajoRepository;

    public EntregaService(
        IEntregaRepository entregaRepository,
        IOrdenTrabajoRepository ordenTrabajoRepository)
    {
        _entregaRepository =
            entregaRepository;

        _ordenTrabajoRepository =
            ordenTrabajoRepository;
    }

    // =====================================================
    // OBTENER TODAS
    // =====================================================

    public async Task<IEnumerable<EntregaDto>>
        ObtenerTodasAsync()
    {
        var entregas =
            await _entregaRepository
                .ObtenerTodosAsync();

        return entregas.Select(MapearDto);
    }

    // =====================================================
    // OBTENER POR ID
    // =====================================================

    public async Task<EntregaDto?>
        ObtenerPorIdAsync(int idEntrega)
    {
        var entrega =
            await _entregaRepository
                .ObtenerCompletaAsync(idEntrega);

        return entrega == null
            ? null
            : MapearDto(entrega);
    }

    // =====================================================
    // OBTENER POR ORDEN DE TRABAJO
    // =====================================================

    public async Task<EntregaDto?>
        ObtenerPorOrdenTrabajoAsync(
            int idOrdenTrabajo)
    {
        var entrega =
            await _entregaRepository
                .ObtenerPorOrdenTrabajoAsync(
                    idOrdenTrabajo);

        return entrega == null
            ? null
            : MapearDto(entrega);
    }

    // =====================================================
    // CREAR
    // =====================================================

    public async Task<(bool Exitoso, string Mensaje)>
        CrearAsync(
            EntregaGuardarDto dto)
    {
        var validacion =
            await ValidarEntregaAsync(dto);

        if (!validacion.Exitoso)
            return validacion;

        var yaExiste =
            await _entregaRepository
                .ExisteParaOrdenTrabajoAsync(
                    dto.IdOrdenTrabajo);

        if (yaExiste)
        {
            return (
                false,
                "Esta orden de trabajo ya tiene registrada una entrega.");
        }

        var orden =
            await _ordenTrabajoRepository
                .ObtenerCompletaAsync(
                    dto.IdOrdenTrabajo);

        if (orden == null)
        {
            return (
                false,
                "La orden de trabajo no existe.");
        }

        if (!string.Equals(
                orden.Estado,
                "Finalizada",
                StringComparison.OrdinalIgnoreCase))
        {
            return (
                false,
                "El vehículo solo puede entregarse cuando la orden de trabajo está finalizada.");
        }

        var entrega = new Entrega
        {
            IdOrdenTrabajo =
                dto.IdOrdenTrabajo,

            FechaEntrega =
                DateTime.UtcNow,

            KilometrajeSalida =
                dto.KilometrajeSalida,

            PersonaRecibe =
                dto.PersonaRecibe.Trim(),

            Observaciones =
                Normalizar(dto.Observaciones),

            Recomendaciones =
                Normalizar(dto.Recomendaciones),

            TieneGarantia =
                dto.TieneGarantia,

            EstadoPago =
                dto.EstadoPago.Trim(),

            Aceptacion =
                dto.Aceptacion,

            FirmaNombre =
                dto.Aceptacion
                    ? Normalizar(dto.FirmaNombre)
                    : null,

            FechaAceptacion =
                dto.Aceptacion
                    ? DateTime.UtcNow
                    : null
        };

        await _entregaRepository
            .AgregarAsync(entrega);

        await _entregaRepository
            .GuardarCambiosAsync();

        return (
            true,
            "Entrega registrada correctamente.");
    }

    // =====================================================
    // ACTUALIZAR
    // =====================================================

    public async Task<(bool Exitoso, string Mensaje)>
        ActualizarAsync(
            int idEntrega,
            EntregaGuardarDto dto)
    {
        var entrega =
            await _entregaRepository
                .ObtenerPorIdAsync(idEntrega);

        if (entrega == null)
        {
            return (
                false,
                "La entrega no existe.");
        }

        var validacion =
            await ValidarEntregaAsync(dto);

        if (!validacion.Exitoso)
            return validacion;

        var orden =
            await _ordenTrabajoRepository
                .ObtenerCompletaAsync(
                    dto.IdOrdenTrabajo);

        if (orden == null)
        {
            return (
                false,
                "La orden de trabajo no existe.");
        }

        if (!string.Equals(
                orden.Estado,
                "Finalizada",
                StringComparison.OrdinalIgnoreCase))
        {
            return (
                false,
                "El vehículo solo puede entregarse cuando la orden de trabajo está finalizada.");
        }

        if (entrega.IdOrdenTrabajo !=
            dto.IdOrdenTrabajo)
        {
            var otraEntrega =
                await _entregaRepository
                    .ExisteParaOrdenTrabajoAsync(
                        dto.IdOrdenTrabajo);

            if (otraEntrega)
            {
                return (
                    false,
                    "La nueva orden de trabajo seleccionada ya tiene una entrega.");
            }
        }

        entrega.IdOrdenTrabajo =
            dto.IdOrdenTrabajo;

        entrega.KilometrajeSalida =
            dto.KilometrajeSalida;

        entrega.PersonaRecibe =
            dto.PersonaRecibe.Trim();

        entrega.Observaciones =
            Normalizar(dto.Observaciones);

        entrega.Recomendaciones =
            Normalizar(dto.Recomendaciones);

        entrega.TieneGarantia =
            dto.TieneGarantia;

        entrega.EstadoPago =
            dto.EstadoPago.Trim();

        entrega.Aceptacion =
            dto.Aceptacion;

        if (dto.Aceptacion)
        {
            entrega.FirmaNombre =
                Normalizar(dto.FirmaNombre);

            entrega.FechaAceptacion ??=
                DateTime.UtcNow;
        }
        else
        {
            entrega.FirmaNombre = null;

            entrega.FechaAceptacion = null;
        }

        _entregaRepository
            .Actualizar(entrega);

        await _entregaRepository
            .GuardarCambiosAsync();

        return (
            true,
            "Entrega actualizada correctamente.");
    }

    // =====================================================
    // VALIDACIONES
    // =====================================================

    private async Task<(bool Exitoso, string Mensaje)>
        ValidarEntregaAsync(
            EntregaGuardarDto dto)
    {
        if (dto.IdOrdenTrabajo <= 0)
        {
            return (
                false,
                "Debe seleccionar una orden de trabajo.");
        }

        if (dto.KilometrajeSalida < 0)
        {
            return (
                false,
                "El kilometraje de salida no puede ser negativo.");
        }

        if (string.IsNullOrWhiteSpace(
                dto.PersonaRecibe))
        {
            return (
                false,
                "Debe indicar la persona que recibe el vehículo.");
        }

        var estadosPago =
            new[]
            {
                "Pendiente",
                "Parcial",
                "Pagado"
            };

        if (!estadosPago.Contains(
                dto.EstadoPago,
                StringComparer.OrdinalIgnoreCase))
        {
            return (
                false,
                "El estado de pago seleccionado no es válido.");
        }

        if (dto.Aceptacion &&
            string.IsNullOrWhiteSpace(
                dto.FirmaNombre))
        {
            return (
                false,
                "Debe indicar el nombre de quien acepta la entrega.");
        }

        var orden =
            await _ordenTrabajoRepository
                .ObtenerCompletaAsync(
                    dto.IdOrdenTrabajo);

        if (orden == null)
        {
            return (
                false,
                "La orden de trabajo no existe.");
        }

        if (!string.Equals(
                orden.Estado,
                "Finalizada",
                StringComparison.OrdinalIgnoreCase))
        {
            return (
                false,
                "La orden de trabajo debe estar finalizada antes de registrar la entrega.");
        }

        return (
            true,
            string.Empty);
    }

    // =====================================================
    // MAPEAR DTO
    // =====================================================

    private static EntregaDto MapearDto(
        Entrega entrega)
    {
        var vehiculo =
            entrega.OrdenTrabajo?
                .Cotizacion?
                .Diagnostico?
                .Recepcion?
                .Vehiculo;

        var cliente =
            vehiculo?.Cliente;

        var clienteNombre =
            cliente == null
                ? string.Empty
                : $"{cliente.Nombre} {cliente.Apellido1} {cliente.Apellido2}"
                    .Trim();

        return new EntregaDto
        {
            IdEntrega =
                entrega.IdEntrega,

            IdOrdenTrabajo =
                entrega.IdOrdenTrabajo,

            Placa =
                vehiculo?.Placa
                ?? string.Empty,

            ClienteNombre =
                clienteNombre,

            FechaEntrega =
                entrega.FechaEntrega,

            KilometrajeSalida =
                entrega.KilometrajeSalida,

            PersonaRecibe =
                entrega.PersonaRecibe,

            Observaciones =
                entrega.Observaciones,

            Recomendaciones =
                entrega.Recomendaciones,

            TieneGarantia =
                entrega.TieneGarantia,

            EstadoPago =
                entrega.EstadoPago,

            Aceptacion =
                entrega.Aceptacion,

            FirmaNombre =
                entrega.FirmaNombre,

            FechaAceptacion =
                entrega.FechaAceptacion
        };
    }

    // =====================================================
    // NORMALIZAR
    // =====================================================

    private static string? Normalizar(
        string? valor)
    {
        return string.IsNullOrWhiteSpace(valor)
            ? null
            : valor.Trim();
    }
}