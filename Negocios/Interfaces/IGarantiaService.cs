using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IGarantiaService
{
    // =====================================================
    // CONSULTAS
    // =====================================================

    Task<IEnumerable<GarantiaDto>>
        ObtenerTodasAsync();

    Task<IEnumerable<GarantiaDto>>
        ObtenerVigentesAsync();

    Task<IEnumerable<GarantiaDto>>
        ObtenerPorVencerAsync(
            int dias);

    Task<IEnumerable<GarantiaDto>>
        ObtenerPorOrdenTrabajoAsync(
            int idOrdenTrabajo);

    Task<IEnumerable<GarantiaDto>>
        ObtenerPorVentaAsync(
            int idVenta);

    Task<GarantiaDto?>
        ObtenerPorIdAsync(
            int id);

    // =====================================================
    // CRUD
    // =====================================================

    Task<int> CrearAsync(
        GarantiaGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        GarantiaGuardarDto dto);

    // =====================================================
    // RECLAMO
    // =====================================================

    Task<bool> RegistrarReclamoAsync(
        int id,
        string motivo);

    // =====================================================
    // RESOLUCIÓN
    // =====================================================

    Task<bool> ResolverAsync(
        int id,
        string resolucion);

    // =====================================================
    // RECHAZAR
    // =====================================================

    Task<bool> RechazarAsync(
        int id,
        string motivo);

    // =====================================================
    // ESTADOS
    // =====================================================

    Task<int> ActualizarGarantiasVencidasAsync();
}