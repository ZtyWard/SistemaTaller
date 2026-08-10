using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface ICompraService
{
    Task<IEnumerable<CompraDto>>
        ObtenerTodosAsync();

    Task<IEnumerable<CompraDto>>
        ObtenerPorProveedorAsync(
            int idProveedor);

    Task<IEnumerable<CompraDto>>
        ObtenerPorEstadoAsync(
            string estado);

    Task<IEnumerable<CompraDto>>
        ObtenerRecientesAsync(
            int cantidad = 10);

    Task<CompraDto?>
        ObtenerPorIdAsync(int id);

    Task CrearAsync(
        CompraGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        CompraGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        string estado);
}