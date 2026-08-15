using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IEntregaService
{
    Task<IEnumerable<EntregaDto>>
        ObtenerTodasAsync();

    Task<EntregaDto?>
        ObtenerPorIdAsync(int idEntrega);

    Task<EntregaDto?>
        ObtenerPorOrdenTrabajoAsync(
            int idOrdenTrabajo);

    Task<(bool Exitoso, string Mensaje)>
        CrearAsync(
            EntregaGuardarDto dto);

    Task<(bool Exitoso, string Mensaje)>
        ActualizarAsync(
            int idEntrega,
            EntregaGuardarDto dto);
}