using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ClienteDto>> ObtenerTodosAsync()
    {
        var clientes = await _repository.ObtenerTodosAsync();

        return clientes.Select(MapearDto);
    }

    public async Task<IEnumerable<ClienteDto>> ObtenerActivosAsync()
    {
        var clientes = await _repository.ObtenerActivosAsync();

        return clientes.Select(MapearDto);
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
    {
        var cliente = await _repository.ObtenerPorIdAsync(id);

        return cliente == null ? null : MapearDto(cliente);
    }

    public async Task<ClienteDto?> ObtenerPorCedulaAsync(string cedula)
    {
        var cliente = await _repository.ObtenerPorCedulaAsync(cedula);

        return cliente == null ? null : MapearDto(cliente);
    }

    public async Task CrearAsync(ClienteGuardarDto dto)
    {
        var existente = await _repository.ObtenerPorCedulaAsync(dto.Cedula);

        if (existente != null)
            throw new InvalidOperationException(
                "Ya existe un cliente con esa cédula.");

        var cliente = new Cliente
        {
            Cedula = dto.Cedula,
            Nombre = dto.Nombre,
            Apellido1 = dto.Apellido1,
            Apellido2 = dto.Apellido2,
            Telefono = dto.Telefono,
            Correo = dto.Correo,
            Direccion = dto.Direccion,
            Activo = true,
            FechaRegistro = DateTime.UtcNow
        };

        await _repository.AgregarAsync(cliente);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ClienteGuardarDto dto)
    {
        var cliente = await _repository.ObtenerPorIdAsync(id);

        if (cliente == null)
            return false;

        var otroCliente = await _repository.ObtenerPorCedulaAsync(dto.Cedula);

        if (otroCliente != null && otroCliente.IdCliente != id)
            throw new InvalidOperationException(
                "Ya existe otro cliente con esa cédula.");

        cliente.Cedula = dto.Cedula;
        cliente.Nombre = dto.Nombre;
        cliente.Apellido1 = dto.Apellido1;
        cliente.Apellido2 = dto.Apellido2;
        cliente.Telefono = dto.Telefono;
        cliente.Correo = dto.Correo;
        cliente.Direccion = dto.Direccion;

        _repository.Actualizar(cliente);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> DesactivarAsync(int id)
    {
        var cliente = await _repository.ObtenerPorIdAsync(id);

        if (cliente == null)
            return false;

        cliente.Activo = false;

        _repository.Actualizar(cliente);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static ClienteDto MapearDto(Cliente cliente)
    {
        return new ClienteDto
        {
            IdCliente = cliente.IdCliente,
            Cedula = cliente.Cedula,
            Nombre = cliente.Nombre,
            Apellido1 = cliente.Apellido1,
            Apellido2 = cliente.Apellido2,
            Telefono = cliente.Telefono,
            Correo = cliente.Correo,
            Direccion = cliente.Direccion,
            Activo = cliente.Activo,
            FechaRegistro = cliente.FechaRegistro
        };
    }
}