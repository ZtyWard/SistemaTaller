using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class MarcaService : IMarcaService
{
    private readonly IMarcaRepository _repository;

    public MarcaService(IMarcaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MarcaDto>> ObtenerTodosAsync()
    {
        var marcas = await _repository.ObtenerTodosAsync();

        return marcas.Select(MapearDto);
    }

    public async Task<IEnumerable<MarcaDto>> ObtenerActivasAsync()
    {
        var marcas = await _repository.ObtenerActivasAsync();

        return marcas.Select(MapearDto);
    }

    public async Task<MarcaDto?> ObtenerPorIdAsync(int id)
    {
        var marca = await _repository.ObtenerPorIdAsync(id);

        return marca == null
            ? null
            : MapearDto(marca);
    }

    public async Task CrearAsync(MarcaGuardarDto dto)
    {
        Validar(dto);

        var marca = new Marca
        {
            Nombre = dto.Nombre.Trim(),
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(marca);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        MarcaGuardarDto dto)
    {
        Validar(dto);

        var marca = await _repository.ObtenerPorIdAsync(id);

        if (marca == null)
            return false;

        marca.Nombre = dto.Nombre.Trim();
        marca.Activo = dto.Activo;

        _repository.Actualizar(marca);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var marca = await _repository.ObtenerPorIdAsync(id);

        if (marca == null)
            return false;

        marca.Activo = activo;

        _repository.Actualizar(marca);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(MarcaGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre de la marca es obligatorio.");
        }
    }

    private static MarcaDto MapearDto(Marca marca)
    {
        return new MarcaDto
        {
            IdMarca = marca.IdMarca,
            Nombre = marca.Nombre,
            Activo = marca.Activo
        };
    }
}