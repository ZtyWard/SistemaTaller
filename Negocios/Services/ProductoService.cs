using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(
        IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductoDto>>
        ObtenerTodosAsync()
    {
        var productos =
            await _repository.ObtenerTodosAsync();

        return productos.Select(MapearDto);
    }

    public async Task<IEnumerable<ProductoDto>>
        ObtenerActivosAsync()
    {
        var productos =
            await _repository.ObtenerActivosAsync();

        return productos.Select(MapearDto);
    }

    public async Task<IEnumerable<ProductoDto>>
        ObtenerStockBajoAsync()
    {
        var productos =
            await _repository.ObtenerStockBajoAsync();

        return productos.Select(MapearDto);
    }

    public async Task<IEnumerable<ProductoDto>>
        ObtenerPorCategoriaAsync(
            int idCategoriaProducto)
    {
        var productos =
            await _repository
                .ObtenerPorCategoriaAsync(
                    idCategoriaProducto);

        return productos.Select(MapearDto);
    }

    public async Task<ProductoDto?>
        ObtenerPorIdAsync(int id)
    {
        var producto =
            await _repository.ObtenerPorIdAsync(id);

        return producto == null
            ? null
            : MapearDto(producto);
    }

    public async Task<ProductoDto?>
        ObtenerPorCodigoAsync(string codigo)
    {
        var producto =
            await _repository
                .ObtenerPorCodigoAsync(
                    codigo.Trim());

        return producto == null
            ? null
            : MapearDto(producto);
    }

    public async Task CrearAsync(
        ProductoGuardarDto dto)
    {
        Validar(dto);

        var existente =
            await _repository
                .ObtenerPorCodigoAsync(
                    dto.Codigo.Trim());

        if (existente != null)
            throw new ArgumentException(
                "Ya existe un producto con ese código.");

        var producto = new Producto
        {
            IdCategoriaProducto =
                dto.IdCategoriaProducto,

            Codigo = dto.Codigo.Trim(),

            Nombre = dto.Nombre.Trim(),

            Descripcion =
                dto.Descripcion?.Trim(),

            PrecioCompra =
                dto.PrecioCompra,

            PrecioVenta =
                dto.PrecioVenta,

            Stock =
                dto.Stock,

            StockMinimo =
                dto.StockMinimo,

            Activo =
                dto.Activo
        };

        await _repository.AgregarAsync(
            producto);

        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ProductoGuardarDto dto)
    {
        Validar(dto);

        var producto =
            await _repository.ObtenerPorIdAsync(id);

        if (producto == null)
            return false;

        var existente =
            await _repository
                .ObtenerPorCodigoAsync(
                    dto.Codigo.Trim());

        if (existente != null &&
            existente.IdProducto != id)
        {
            throw new ArgumentException(
                "Ya existe otro producto con ese código.");
        }

        producto.IdCategoriaProducto =
            dto.IdCategoriaProducto;

        producto.Codigo =
            dto.Codigo.Trim();

        producto.Nombre =
            dto.Nombre.Trim();

        producto.Descripcion =
            dto.Descripcion?.Trim();

        producto.PrecioCompra =
            dto.PrecioCompra;

        producto.PrecioVenta =
            dto.PrecioVenta;

        producto.Stock =
            dto.Stock;

        producto.StockMinimo =
            dto.StockMinimo;

        producto.Activo =
            dto.Activo;

        _repository.Actualizar(producto);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var producto =
            await _repository.ObtenerPorIdAsync(id);

        if (producto == null)
            return false;

        producto.Activo = activo;

        _repository.Actualizar(producto);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(
        ProductoGuardarDto dto)
    {
        if (dto.IdCategoriaProducto <= 0)
            throw new ArgumentException(
                "Debe seleccionar una categoría.");

        if (string.IsNullOrWhiteSpace(
                dto.Codigo))
        {
            throw new ArgumentException(
                "El código es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(
                dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre es obligatorio.");
        }

        if (dto.PrecioCompra < 0)
            throw new ArgumentException(
                "El precio de compra no puede ser negativo.");

        if (dto.PrecioVenta < 0)
            throw new ArgumentException(
                "El precio de venta no puede ser negativo.");

        if (dto.Stock < 0)
            throw new ArgumentException(
                "El stock no puede ser negativo.");

        if (dto.StockMinimo < 0)
            throw new ArgumentException(
                "El stock mínimo no puede ser negativo.");
    }

    private static ProductoDto MapearDto(
        Producto producto)
    {
        return new ProductoDto
        {
            IdProducto =
                producto.IdProducto,

            IdCategoriaProducto =
                producto.IdCategoriaProducto,

            Categoria =
                producto.CategoriaProducto?.Nombre
                ?? string.Empty,

            Codigo =
                producto.Codigo,

            Nombre =
                producto.Nombre,

            Descripcion =
                producto.Descripcion,

            PrecioCompra =
                producto.PrecioCompra,

            PrecioVenta =
                producto.PrecioVenta,

            Stock =
                producto.Stock,

            StockMinimo =
                producto.StockMinimo,

            Activo =
                producto.Activo
        };
    }
}