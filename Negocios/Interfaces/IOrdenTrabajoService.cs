using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IOrdenTrabajoService
{
    Task<IEnumerable<OrdenTrabajoDto>> ObtenerTodasAsync();

    Task<IEnumerable<OrdenTrabajoDto>> ObtenerAbiertasAsync();

    Task<IEnumerable<OrdenTrabajoDto>> ObtenerPorEstadoAsync(
        string estado);

    Task<OrdenTrabajoDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(OrdenTrabajoGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        OrdenTrabajoGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        string estado);

    Task<bool> FinalizarAsync(int id);
}