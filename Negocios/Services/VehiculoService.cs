using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class VehiculoService : IVehiculoService
{
    private readonly IVehiculoRepository _repository;

    public VehiculoService(IVehiculoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<VehiculoDto>> ObtenerTodosAsync()
    {
        var vehiculos = await _repository.ObtenerTodosAsync();

        return vehiculos.Select(MapearDto);
    }

    public async Task<IEnumerable<VehiculoDto>> ObtenerActivosAsync()
    {
        var vehiculos = await _repository.ObtenerActivosAsync();

        return vehiculos.Select(MapearDto);
    }

    public async Task<IEnumerable<VehiculoDto>> ObtenerPorClienteAsync(
        int idCliente)
    {
        var vehiculos = await _repository.ObtenerPorClienteAsync(idCliente);

        return vehiculos.Select(MapearDto);
    }

    public async Task<VehiculoDto?> ObtenerPorIdAsync(int id)
    {
        var vehiculo = await _repository.ObtenerCompletoAsync(id);

        return vehiculo == null ? null : MapearDto(vehiculo);
    }

    public async Task<VehiculoDto?> ObtenerPorPlacaAsync(string placa)
    {
        var vehiculo = await _repository.ObtenerPorPlacaAsync(placa);

        return vehiculo == null ? null : MapearDto(vehiculo);
    }

    public async Task CrearAsync(VehiculoGuardarDto dto)
    {
        var existente = await _repository.ObtenerPorPlacaAsync(dto.Placa);

        if (existente != null)
            throw new InvalidOperationException(
                "Ya existe un vehículo registrado con esa placa.");

        var vehiculo = new Vehiculo
        {
            IdCliente = dto.IdCliente,
            IdMarca = dto.IdMarca,
            IdModelo = dto.IdModelo,
            IdTipoVehiculo = dto.IdTipoVehiculo,
            IdTipoCombustible = dto.IdTipoCombustible,
            Placa = dto.Placa,
            VIN = dto.VIN,
            Color = dto.Color,
            Anio = dto.Anio,
            Kilometraje = dto.Kilometraje,
            Activo = true
        };

        await _repository.AgregarAsync(vehiculo);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        VehiculoGuardarDto dto)
    {
        var vehiculo = await _repository.ObtenerPorIdAsync(id);

        if (vehiculo == null)
            return false;

        var otroVehiculo =
            await _repository.ObtenerPorPlacaAsync(dto.Placa);

        if (otroVehiculo != null &&
            otroVehiculo.IdVehiculo != id)
        {
            throw new InvalidOperationException(
                "Ya existe otro vehículo con esa placa.");
        }

        vehiculo.IdCliente = dto.IdCliente;
        vehiculo.IdMarca = dto.IdMarca;
        vehiculo.IdModelo = dto.IdModelo;
        vehiculo.IdTipoVehiculo = dto.IdTipoVehiculo;
        vehiculo.IdTipoCombustible = dto.IdTipoCombustible;
        vehiculo.Placa = dto.Placa;
        vehiculo.VIN = dto.VIN;
        vehiculo.Color = dto.Color;
        vehiculo.Anio = dto.Anio;
        vehiculo.Kilometraje = dto.Kilometraje;

        _repository.Actualizar(vehiculo);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> DesactivarAsync(int id)
    {
        var vehiculo = await _repository.ObtenerPorIdAsync(id);

        if (vehiculo == null)
            return false;

        vehiculo.Activo = false;

        _repository.Actualizar(vehiculo);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static VehiculoDto MapearDto(Vehiculo vehiculo)
    {
        return new VehiculoDto
        {
            IdVehiculo = vehiculo.IdVehiculo,

            IdCliente = vehiculo.IdCliente,
            ClienteNombre = vehiculo.Cliente != null
                ? $"{vehiculo.Cliente.Nombre} {vehiculo.Cliente.Apellido1}"
                : string.Empty,

            IdMarca = vehiculo.IdMarca,
            MarcaNombre = vehiculo.Marca?.Nombre ?? string.Empty,

            IdModelo = vehiculo.IdModelo,
            ModeloNombre = vehiculo.Modelo?.Nombre ?? string.Empty,

            IdTipoVehiculo = vehiculo.IdTipoVehiculo,
            TipoVehiculoNombre =
                vehiculo.TipoVehiculo?.Nombre ?? string.Empty,

            IdTipoCombustible = vehiculo.IdTipoCombustible,
            TipoCombustibleNombre =
                vehiculo.TipoCombustible?.Nombre ?? string.Empty,

            Placa = vehiculo.Placa,
            VIN = vehiculo.VIN,
            Color = vehiculo.Color,
            Anio = vehiculo.Anio,
            Kilometraje = vehiculo.Kilometraje,
            Activo = vehiculo.Activo
        };
    }
}