using Negocios.IntegracionesExternas.Vehiculos.DTOs;

namespace Negocios.IntegracionesExternas.Vehiculos.Interfaces
{
    public interface IVehiculoApiService
    {
        Task<IEnumerable<VehiculoApiResultadoDto>> BuscarAsync(
            string consulta,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<VehiculoApiResultadoDto>> BuscarPorMarcaYAñoAsync(
            string marca,
            int año,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<VehiculoApiTipoDto>> ObtenerTiposPorMarcaAsync(
            string marca,
            CancellationToken cancellationToken = default);
    }
}