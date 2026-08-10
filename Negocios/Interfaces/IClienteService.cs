using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteDto>> ObtenerTodosAsync();

    Task<IEnumerable<ClienteDto>> ObtenerActivosAsync();

    Task<ClienteDto?> ObtenerPorIdAsync(int id);

    Task<ClienteDto?> ObtenerPorCedulaAsync(string cedula);

    Task CrearAsync(ClienteGuardarDto dto);

    Task<bool> ActualizarAsync(int id, ClienteGuardarDto dto);

    Task<bool> DesactivarAsync(int id);
}