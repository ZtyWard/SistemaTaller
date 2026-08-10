using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class TipoCombustibleService : ITipoCombustibleService
{
    private readonly ITipoCombustibleRepository _repository;

    public TipoCombustibleService(
        ITipoCombustibleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TipoCombustibleDto>>
        ObtenerTodosAsync()
    {
        var tipos =
            await _repository.ObtenerTodosAsync();

        return tipos.Select(MapearDto);
    }

    public async Task<IEnumerable<TipoCombustibleDto>>
        ObtenerActivasAsync()
    {
        var tipos =
            await _repository.ObtenerActivasAsync();

        return tipos.Select(MapearDto);
    }

    public async Task<TipoCombustibleDto?>
        ObtenerPorIdAsync(int id)
    {
        var tipo =
            await _repository.ObtenerPorIdAsync(id);

        return tipo == null
            ? null
            : MapearDto(tipo);
    }

    public async Task CrearAsync(
        TipoCombustibleGuardarDto dto)
    {
        Validar(dto);

        var tipo = new TipoCombustible
        {
            Nombre = dto.Nombre.Trim(),
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(tipo);

        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        TipoCombustibleGuardarDto dto)
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
        TipoCombustibleGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre del tipo de combustible es obligatorio.");
        }
    }

    private static TipoCombustibleDto MapearDto(
        TipoCombustible tipo)
    {
        return new TipoCombustibleDto
        {
            IdTipoCombustible = tipo.IdTipoCombustible,
            Nombre = tipo.Nombre,
            Activo = tipo.Activo
        };
    }
}