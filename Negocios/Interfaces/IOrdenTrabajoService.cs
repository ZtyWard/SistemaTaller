using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IOrdenTrabajoService
{
    // =====================================================
    // CONSULTAS
    // =====================================================

    Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerTodasAsync();

    Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerAbiertasAsync();

    Task<IEnumerable<OrdenTrabajoDto>>
        ObtenerPorEstadoAsync(string estado);

    Task<OrdenTrabajoDto?>
        ObtenerPorIdAsync(int id);

    // =====================================================
    // CREAR
    // =====================================================

    Task CrearAsync(
        OrdenTrabajoGuardarDto dto,
        string usuarioId);

    // =====================================================
    // ACTUALIZAR
    // =====================================================

    Task<bool> ActualizarAsync(
        int id,
        OrdenTrabajoGuardarDto dto,
        string usuarioId);

    // =====================================================
    // CAMBIAR ESTADO
    // =====================================================

    Task<bool> CambiarEstadoAsync(
        int id,
        string estado,
        string usuarioId,
        string? observaciones = null);

    // =====================================================
    // FINALIZAR
    // =====================================================

    Task<bool> FinalizarAsync(
        int id,
        string usuarioId);
}