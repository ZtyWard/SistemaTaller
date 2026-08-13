using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IFacturaService
{
    Task<IEnumerable<FacturaDto>>
        ObtenerTodosAsync();

    Task<IEnumerable<FacturaDto>>
        ObtenerPendientesAsync();

    Task<FacturaDto?>
        ObtenerPorIdAsync(int id);

    Task CrearAsync(
        FacturaGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        FacturaGuardarDto dto);

    Task<bool> AnularAsync(
        int id);
}