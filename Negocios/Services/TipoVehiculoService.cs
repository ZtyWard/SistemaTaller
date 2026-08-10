using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class TipoVehiculoService : ITipoVehiculoService
{
    private readonly ITipoVehiculoRepository _repository;

    public TipoVehiculoService(
        ITipoVehiculoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TipoVehiculoDto>>
        ObtenerTodosAsync()
    {
        var tipos =
            await _repository.ObtenerTodosAsync();

        return tipos.Select(MapearDto);
    }

    public async Task<IEnumerable<TipoVehiculoDto>>
        ObtenerActivasAsync()
    {
        var tipos =
            await _repository.ObtenerActivasAsync();

        return tipos.Select(MapearDto);
    }

    public async Task<TipoVehiculoDto?>
        ObtenerPorIdAsync(int id)
    {
        var tipo =
            await _repository.ObtenerPorIdAsync(id);

        return tipo == null
            ? null
            : MapearDto(tipo);
    }

    public async Task CrearAsync(
        TipoVehiculoGuardarDto dto)
    {
        Validar(dto);

        var tipo = new TipoVehiculo
        {
            Nombre = dto.Nombre.Trim(),
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(tipo);

        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        TipoVehiculoGuardarDto dto)
    {
        Validar(dto);

        var tipo =
            await _repository.ObtenerPorIdAsync(id);

        if (tipo == null)
            return false;

        tipo.Nombre = dto.Nombre.Trim();
        tipo.Activo = dto.Activo;

        _repository.Actualizar(tipo);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var tipo =
            await _repository.ObtenerPorIdAsync(id);

        if (tipo == null)
            return false;

        tipo.Activo = activo;

        _repository.Actualizar(tipo);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(
        TipoVehiculoGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre del tipo de vehículo es obligatorio.");
        }
    }

    private static TipoVehiculoDto MapearDto(
        TipoVehiculo tipo)
    {
        return new TipoVehiculoDto
        {
            IdTipoVehiculo = tipo.IdTipoVehiculo,
            Nombre = tipo.Nombre,
            Activo = tipo.Activo
        };
    }
}