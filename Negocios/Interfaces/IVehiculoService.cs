using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IVehiculoService
{
    Task<IEnumerable<VehiculoDto>> ObtenerTodosAsync();

    Task<IEnumerable<VehiculoDto>> ObtenerActivosAsync();

    Task<IEnumerable<VehiculoDto>> ObtenerPorClienteAsync(int idCliente);

    Task<VehiculoDto?> ObtenerPorIdAsync(int id);

    Task<VehiculoDto?> ObtenerPorPlacaAsync(string placa);

    Task CrearAsync(VehiculoGuardarDto dto);

    Task<bool> ActualizarAsync(int id, VehiculoGuardarDto dto);

    Task<bool> DesactivarAsync(int id);
}