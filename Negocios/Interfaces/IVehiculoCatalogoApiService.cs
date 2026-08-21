using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IVehiculoCatalogoApiService
{
    Task<IEnumerable<string>> BuscarMarcasAsync(
        string texto,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<VehiculoApiResultadoDto>> BuscarModelosAsync(
        string marca,
        string texto,
        CancellationToken cancellationToken = default);
}