using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class GarantiaService : IGarantiaService
{
    private readonly IGarantiaRepository _repository;

    public GarantiaService(
        IGarantiaRepository repository)
    {
        _repository = repository;
    }

    // =====================================================
    // OBTENER TODAS
    // =====================================================

    public async Task<IEnumerable<GarantiaDto>>
        ObtenerTodasAsync()
    {
        var garantias =
            await _repository.ObtenerTodosAsync();

        return garantias.Select(MapearDto);
    }

    // =====================================================
    // OBTENER VIGENTES
    // =====================================================

    public async Task<IEnumerable<GarantiaDto>>
        ObtenerVigentesAsync()
    {
        await ActualizarGarantiasVencidasAsync();

        var garantias =
            await _repository.ObtenerVigentesAsync();

        return garantias.Select(MapearDto);
    }

    // =====================================================
    // OBTENER POR VENCER
    // =====================================================

    public async Task<IEnumerable<GarantiaDto>>
        ObtenerPorVencerAsync(
            int dias)
    {
        if (dias < 0)
            dias = 0;

        await ActualizarGarantiasVencidasAsync();

        var garantias =
            await _repository.ObtenerPorVencerAsync(
                dias);

        return garantias.Select(MapearDto);
    }

    // =====================================================
    // POR ORDEN DE TRABAJO
    // =====================================================

    public async Task<IEnumerable<GarantiaDto>>
        ObtenerPorOrdenTrabajoAsync(
            int idOrdenTrabajo)
    {
        if (idOrdenTrabajo <= 0)
            throw new ArgumentException(
                "La orden de trabajo es obligatoria.");

        var garantias =
            await _repository
                .ObtenerPorOrdenTrabajoAsync(
                    idOrdenTrabajo);

        return garantias.Select(MapearDto);
    }

    // =====================================================
    // POR VENTA
    // =====================================================

    public async Task<IEnumerable<GarantiaDto>>
        ObtenerPorVentaAsync(
            int idVenta)
    {
        if (idVenta <= 0)
            throw new ArgumentException(
                "La venta es obligatoria.");

        var garantias =
            await _repository
                .ObtenerPorVentaAsync(
                    idVenta);

        return garantias.Select(MapearDto);
    }

    // =====================================================
    // OBTENER POR ID
    // =====================================================

    public async Task<GarantiaDto?>
        ObtenerPorIdAsync(
            int id)
    {
        if (id <= 0)
            return null;

        var garantia =
            await _repository
                .ObtenerCompletaAsync(id);

        return garantia == null
            ? null
            : MapearDto(garantia);
    }

    // =====================================================
    // CREAR
    // =====================================================

    public async Task<int> CrearAsync(
        GarantiaGuardarDto dto)
    {
        ValidarGarantia(dto);

        var garantia = new Garantia
        {
            IdOrdenTrabajo =
                dto.IdOrdenTrabajo,

            IdVenta =
                dto.IdVenta,

            IdProducto =
                dto.IdProducto,

            IdServicio =
                dto.IdServicio,

            FechaInicio =
                dto.FechaInicio.Date,

            FechaVencimiento =
                dto.FechaVencimiento.Date,

            Estado =
                "Vigente",

            Condiciones =
                dto.Condiciones,

            Observaciones =
                dto.Observaciones
        };

        await _repository.AgregarAsync(
            garantia);

        await _repository.GuardarCambiosAsync();

        return garantia.IdGarantia;
    }

    // =====================================================
    // ACTUALIZAR
    // =====================================================

    public async Task<bool> ActualizarAsync(
        int id,
        GarantiaGuardarDto dto)
    {
        if (id <= 0)
            return false;

        ValidarGarantia(dto);

        var garantia =
            await _repository
                .ObtenerPorIdAsync(id);

        if (garantia == null)
            return false;

        if (garantia.Estado == "Resuelta")
            throw new InvalidOperationException(
                "No se puede modificar una garantía resuelta.");

        if (garantia.Estado == "Rechazada")
            throw new InvalidOperationException(
                "No se puede modificar una garantía rechazada.");

        garantia.IdOrdenTrabajo =
            dto.IdOrdenTrabajo;

        garantia.IdVenta =
            dto.IdVenta;

        garantia.IdProducto =
            dto.IdProducto;

        garantia.IdServicio =
            dto.IdServicio;

        garantia.FechaInicio =
            dto.FechaInicio.Date;

        garantia.FechaVencimiento =
            dto.FechaVencimiento.Date;

        garantia.Condiciones =
            dto.Condiciones;

        garantia.Observaciones =
            dto.Observaciones;

        _repository.Actualizar(
            garantia);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    // =====================================================
    // REGISTRAR RECLAMO
    // =====================================================

    public async Task<bool> RegistrarReclamoAsync(
        int id,
        string motivo)
    {
        if (id <= 0)
            return false;

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException(
                "El motivo del reclamo es obligatorio.");

        var garantia =
            await _repository
                .ObtenerPorIdAsync(id);

        if (garantia == null)
            return false;

        if (garantia.Estado != "Vigente")
            throw new InvalidOperationException(
                "Solo se puede reclamar una garantía vigente.");

        if (garantia.FechaVencimiento.Date <
            DateTime.Today)
        {
            garantia.Estado = "Vencida";

            _repository.Actualizar(
                garantia);

            await _repository
                .GuardarCambiosAsync();

            throw new InvalidOperationException(
                "La garantía ya se encuentra vencida.");
        }

        garantia.Estado =
            "Reclamada";

        garantia.MotivoReclamo =
            motivo.Trim();

        garantia.FechaReclamo =
            DateTime.Today;

        _repository.Actualizar(
            garantia);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    // =====================================================
    // RESOLVER
    // =====================================================

    public async Task<bool> ResolverAsync(
        int id,
        string resolucion)
    {
        if (id <= 0)
            return false;

        if (string.IsNullOrWhiteSpace(resolucion))
            throw new ArgumentException(
                "La resolución es obligatoria.");

        var garantia =
            await _repository
                .ObtenerPorIdAsync(id);

        if (garantia == null)
            return false;

        if (garantia.Estado != "Reclamada")
            throw new InvalidOperationException(
                "Solo se puede resolver una garantía reclamada.");

        garantia.Estado =
            "Resuelta";

        garantia.Resolucion =
            resolucion.Trim();

        garantia.FechaResolucion =
            DateTime.Today;

        _repository.Actualizar(
            garantia);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    // =====================================================
    // RECHAZAR
    // =====================================================

    public async Task<bool> RechazarAsync(
        int id,
        string motivo)
    {
        if (id <= 0)
            return false;

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException(
                "El motivo del rechazo es obligatorio.");

        var garantia =
            await _repository
                .ObtenerPorIdAsync(id);

        if (garantia == null)
            return false;

        if (garantia.Estado != "Reclamada")
            throw new InvalidOperationException(
                "Solo se puede rechazar una garantía reclamada.");

        garantia.Estado =
            "Rechazada";

        garantia.Resolucion =
            motivo.Trim();

        garantia.FechaResolucion =
            DateTime.Today;

        _repository.Actualizar(
            garantia);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    // =====================================================
    // ACTUALIZAR VENCIDAS
    // =====================================================

    public async Task<int>
        ActualizarGarantiasVencidasAsync()
    {
        var garantias =
            await _repository.ObtenerTodosAsync();

        var actualizadas = 0;

        foreach (var garantia in garantias)
        {
            if (
                garantia.Estado == "Vigente" &&
                garantia.FechaVencimiento.Date <
                DateTime.Today
            )
            {
                garantia.Estado =
                    "Vencida";

                _repository.Actualizar(
                    garantia);

                actualizadas++;
            }
        }

        if (actualizadas > 0)
        {
            await _repository
                .GuardarCambiosAsync();
        }

        return actualizadas;
    }

    // =====================================================
    // VALIDACIONES
    // =====================================================

    private static void ValidarGarantia(
        GarantiaGuardarDto dto)
    {
        // Debe existir un origen.
        if (
            !dto.IdOrdenTrabajo.HasValue &&
            !dto.IdVenta.HasValue
        )
        {
            throw new ArgumentException(
                "La garantía debe estar asociada a una orden de trabajo o a una venta.");
        }

        // Debe existir una cobertura.
        if (
            !dto.IdProducto.HasValue &&
            !dto.IdServicio.HasValue
        )
        {
            throw new ArgumentException(
                "La garantía debe cubrir un producto o un servicio.");
        }

        // No permitimos ambas coberturas simultáneamente.
        if (
            dto.IdProducto.HasValue &&
            dto.IdServicio.HasValue
        )
        {
            throw new ArgumentException(
                "La garantía debe corresponder a un producto o a un servicio, no a ambos.");
        }

        if (dto.FechaVencimiento.Date <
            dto.FechaInicio.Date)
        {
            throw new ArgumentException(
                "La fecha de vencimiento no puede ser anterior a la fecha de inicio.");
        }
    }

    // =====================================================
    // MAPEAR DTO
    // =====================================================

    private static GarantiaDto MapearDto(
        Garantia garantia)
    {
        return new GarantiaDto
        {
            IdGarantia =
                garantia.IdGarantia,

            IdOrdenTrabajo =
                garantia.IdOrdenTrabajo,

            IdVenta =
                garantia.IdVenta,

            IdProducto =
                garantia.IdProducto,

            IdServicio =
                garantia.IdServicio,

            ProductoNombre =
                garantia.Producto?.Nombre,

            ServicioNombre =
                garantia.Servicio?.Nombre,

            FechaInicio =
                garantia.FechaInicio,

            FechaVencimiento =
                garantia.FechaVencimiento,

            Estado =
                garantia.Estado,

            Condiciones =
                garantia.Condiciones,

            Observaciones =
                garantia.Observaciones,

            MotivoReclamo =
                garantia.MotivoReclamo,

            FechaReclamo =
                garantia.FechaReclamo,

            Resolucion =
                garantia.Resolucion,

            FechaResolucion =
                garantia.FechaResolucion
        };
    }
}