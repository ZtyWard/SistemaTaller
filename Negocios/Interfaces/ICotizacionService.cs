using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface ICotizacionService
{
    Task<IEnumerable<CotizacionDto>> ObtenerTodasAsync();

    Task<IEnumerable<CotizacionDto>> ObtenerPendientesAsync();

    Task<IEnumerable<CotizacionDto>> ObtenerPorDiagnosticoAsync(
        int idDiagnostico);

    Task<CotizacionDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(CotizacionGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        CotizacionGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        string estado);
}