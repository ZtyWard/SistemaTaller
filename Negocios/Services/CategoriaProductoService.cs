using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class CategoriaProductoService
    : ICategoriaProductoService
{
    private readonly ICategoriaProductoRepository
        _repository;

    public CategoriaProductoService(
        ICategoriaProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoriaProductoDto>>
        ObtenerTodosAsync()
    {
        var categorias =
            await _repository.ObtenerTodosAsync();

        return categorias.Select(MapearDto);
    }

    public async Task<IEnumerable<CategoriaProductoDto>>
        ObtenerActivasAsync()
    {
        var categorias =
            await _repository.ObtenerActivasAsync();

        return categorias.Select(MapearDto);
    }

    public async Task<CategoriaProductoDto?>
        ObtenerPorIdAsync(int id)
    {
        var categoria =
            await _repository.ObtenerPorIdAsync(id);

        return categoria == null
            ? null
            : MapearDto(categoria);
    }

    public async Task CrearAsync(
        CategoriaProductoGuardarDto dto)
    {
        Validar(dto);

        var categoria = new CategoriaProducto
        {
            Nombre = dto.Nombre.Trim(),
            Activo = dto.Activo
        };

        await _repository.AgregarAsync(categoria);

        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        CategoriaProductoGuardarDto dto)
    {
        Validar(dto);

        var categoria =
            await _repository.ObtenerPorIdAsync(id);

        if (categoria == null)
            return false;

        categoria.Nombre =
            dto.Nombre.Trim();

        categoria.Activo =
            dto.Activo;

        _repository.Actualizar(categoria);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var categoria =
            await _repository.ObtenerPorIdAsync(id);

        if (categoria == null)
            return false;

        categoria.Activo = activo;

        _repository.Actualizar(categoria);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(
        CategoriaProductoGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre de la categoría es obligatorio.");
        }
    }

    private static CategoriaProductoDto
        MapearDto(CategoriaProducto categoria)
    {
        return new CategoriaProductoDto
        {
            IdCategoriaProducto =
                categoria.IdCategoriaProducto,

            Nombre =
                categoria.Nombre,

            Activo =
                categoria.Activo
        };
    }
}