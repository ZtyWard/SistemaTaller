using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IVentaService
{
    Task<IEnumerable<VentaDto>>
        ObtenerTodosAsync();

    Task<IEnumerable<VentaDto>>
        ObtenerPorClienteAsync(
            int idCliente);

    Task<IEnumerable<VentaDto>>
        ObtenerPorEstadoAsync(
            string estado);

    Task<IEnumerable<VentaDto>>
        ObtenerRecientesAsync(
            int cantidad = 10);

    Task<VentaDto?>
        ObtenerPorIdAsync(
            int id);

    Task CrearAsync(
        VentaGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        VentaGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        string estado);
}