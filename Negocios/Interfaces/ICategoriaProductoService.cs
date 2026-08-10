using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface ICategoriaProductoService
{
    Task<IEnumerable<CategoriaProductoDto>>
        ObtenerTodosAsync();

    Task<IEnumerable<CategoriaProductoDto>>
        ObtenerActivasAsync();

    Task<CategoriaProductoDto?>
        ObtenerPorIdAsync(int id);

    Task CrearAsync(
        CategoriaProductoGuardarDto dto);

    Task<bool> ActualizarAsync(
        int id,
        CategoriaProductoGuardarDto dto);

    Task<bool> CambiarEstadoAsync(
        int id,
        bool activo);
}