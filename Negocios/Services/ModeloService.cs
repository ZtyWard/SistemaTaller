using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class ModeloService : IModeloService
{
    private readonly IModeloRepository _repository;

    public ModeloService(IModeloRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ModeloDto>> ObtenerTodosAsync()
    {
        var modelos = await _repository.ObtenerTodosAsync();

        return modelos.Select(MapearDto);
    }

    public async Task<IEnumerable<ModeloDto>> ObtenerActivasAsync()
    {
        var modelos = await _repository.ObtenerActivasAsync();

        return modelos.Select(MapearDto);
    }

    public async Task<ModeloDto?> ObtenerPorIdAsync(int id)
    {
        var modelo = await _repository.ObtenerPorIdAsync(id);

        return modelo == null
            ? null
            : MapearDto(modelo);
    }

    public async Task CrearAsync(ModeloGuardarDto dto)
    {
        Validar(dto);

        var nombreNormalizado = dto.Nombre.Trim();

        var existe = await _repository.ExisteAsync(x =>
            x.IdMarca == dto.IdMarca &&
            x.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existe)
        {
            throw new InvalidOperationException(
                "Ya existe un modelo con ese nombre para la marca seleccionada.");
        }

        var modelo = new Modelo
        {
            IdMarca = dto.IdMarca,
            Nombre = nombreNormalizado,
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(modelo);
        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ModeloGuardarDto dto)
    {
        Validar(dto);

        var modelo = await _repository.ObtenerPorIdAsync(id);

        if (modelo == null)
            return false;

        var nombreNormalizado = dto.Nombre.Trim();

        var existe = await _repository.ExisteAsync(x =>
            x.IdModelo != id &&
            x.IdMarca == dto.IdMarca &&
            x.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existe)
        {
            throw new InvalidOperationException(
                "Ya existe otro modelo con ese nombre para la marca seleccionada.");
        }

        modelo.IdMarca = dto.IdMarca;
        modelo.Nombre = nombreNormalizado;
        modelo.Activo = dto.Activo;

        _repository.Actualizar(modelo);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var modelo = await _repository.ObtenerPorIdAsync(id);

        if (modelo == null)
            return false;

        modelo.Activo = activo;

        _repository.Actualizar(modelo);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(ModeloGuardarDto dto)
    {
        if (dto.IdMarca <= 0)
        {
            throw new ArgumentException(
                "La marca es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre del modelo es obligatorio.");
        }

        if (dto.Nombre.Trim().Length < 2)
        {
            throw new ArgumentException(
                "El nombre del modelo debe tener al menos 2 caracteres.");
        }
    }

    private static ModeloDto MapearDto(Modelo modelo)
    {
        return new ModeloDto
        {
            IdModelo = modelo.IdModelo,
            IdMarca = modelo.IdMarca,
            Nombre = modelo.Nombre,
            MarcaNombre = modelo.Marca?.Nombre ?? "Sin marca",
            Activo = modelo.Activo
        };
    }
}