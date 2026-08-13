using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IPagoService
{
    Task<IEnumerable<PagoDto>> ObtenerRecientesAsync(
        int cantidad = 20);

    Task<IEnumerable<PagoDto>> ObtenerPorFacturaAsync(
        int idFactura);

    Task<PagoDto?> ObtenerPorIdAsync(
        int idPago);

    Task<PagoDto> RegistrarAsync(
        PagoGuardarDto dto,
        string? usuarioId);
}