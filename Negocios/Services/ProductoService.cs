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

    // =====================================================
    // OBTENER TODOS
    // =====================================================

    public async Task<IEnumerable<ProductoDto>>
        ObtenerTodosAsync()
    {
        var productos =
            await _repository.ObtenerTodosAsync();

        return productos.Select(MapearDto);
    }

    // =====================================================
    // OBTENER ACTIVOS
    // =====================================================

    public async Task<IEnumerable<ProductoDto>>
        ObtenerActivosAsync()
    {
        var productos =
            await _repository.ObtenerActivosAsync();

        return productos.Select(MapearDto);
    }

    // =====================================================
    // STOCK BAJO
    // =====================================================

    public async Task<IEnumerable<ProductoDto>>
        ObtenerStockBajoAsync()
    {
        var productos =
            await _repository.ObtenerStockBajoAsync();

        return productos.Select(MapearDto);
    }

    // =====================================================
    // POR CATEGORÍA
    // =====================================================

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

    // =====================================================
    // POR ID
    // =====================================================

    public async Task<ProductoDto?>
        ObtenerPorIdAsync(int id)
    {
        var producto =
            await _repository.ObtenerPorIdAsync(id);

        return producto == null
            ? null
            : MapearDto(producto);
    }

    // =====================================================
    // POR CÓDIGO INTERNO
    // =====================================================

    public async Task<ProductoDto?>
        ObtenerPorCodigoAsync(
            string codigo)
    {
        var producto =
            await _repository
                .ObtenerPorCodigoAsync(
                    codigo.Trim());

        return producto == null
            ? null
            : MapearDto(producto);
    }

    // =====================================================
    // POR CÓDIGO DE BARRAS
    // =====================================================

    public async Task<ProductoDto?>
        ObtenerPorCodigoBarrasAsync(
            string codigoBarras)
    {
        var producto =
            await _repository
                .ObtenerPorCodigoBarrasAsync(
                    codigoBarras.Trim());

        return producto == null
            ? null
            : MapearDto(producto);
    }

    // =====================================================
    // CREAR
    // =====================================================

    public async Task CrearAsync(
        ProductoGuardarDto dto)
    {
        Validar(dto);

        var existente =
            await _repository
                .ObtenerPorCodigoAsync(
                    dto.Codigo.Trim());

        if (existente != null)
        {
            throw new ArgumentException(
                "Ya existe un producto con ese código.");
        }

        // Validar código de barras si fue proporcionado
        if (!string.IsNullOrWhiteSpace(dto.CodigoBarras))
        {
            var existenteBarra =
                await _repository
                    .ObtenerPorCodigoBarrasAsync(
                        dto.CodigoBarras.Trim());

            if (existenteBarra != null)
            {
                throw new ArgumentException(
                    "Ya existe un producto con ese código de barras.");
            }
        }

        var producto = new Producto
        {
            IdCategoriaProducto =
                dto.IdCategoriaProducto,

            Codigo =
                dto.Codigo.Trim(),

            CodigoBarras =
                string.IsNullOrWhiteSpace(dto.CodigoBarras)
                    ? null
                    : dto.CodigoBarras.Trim(),

            Nombre =
                dto.Nombre.Trim(),

            Descripcion =
                dto.Descripcion?.Trim(),

            ImagenUrl =
                string.IsNullOrWhiteSpace(dto.ImagenUrl)
                    ? null
                    : dto.ImagenUrl.Trim(),

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

        await _repository.AgregarAsync(producto);

        await _repository.GuardarCambiosAsync();
    }

    // =====================================================
    // ACTUALIZAR
    // =====================================================

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

        // Validar código de barras
        if (!string.IsNullOrWhiteSpace(dto.CodigoBarras))
        {
            var existenteBarra =
                await _repository
                    .ObtenerPorCodigoBarrasAsync(
                        dto.CodigoBarras.Trim());

            if (existenteBarra != null &&
                existenteBarra.IdProducto != id)
            {
                throw new ArgumentException(
                    "Ya existe otro producto con ese código de barras.");
            }
        }

        producto.IdCategoriaProducto =
            dto.IdCategoriaProducto;

        producto.Codigo =
            dto.Codigo.Trim();

        producto.CodigoBarras =
            string.IsNullOrWhiteSpace(dto.CodigoBarras)
                ? producto.CodigoBarras
                : dto.CodigoBarras.Trim();

        producto.Nombre =
            dto.Nombre.Trim();

        producto.Descripcion =
            dto.Descripcion?.Trim();

        // Si viene una imagen nueva, se actualiza.
        // Si no viene, se conserva la anterior.
        if (!string.IsNullOrWhiteSpace(dto.ImagenUrl))
        {
            producto.ImagenUrl =
                dto.ImagenUrl.Trim();
        }

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

    // =====================================================
    // CAMBIAR ESTADO
    // =====================================================

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

    // =====================================================
    // VALIDACIONES
    // =====================================================

    private static void Validar(
        ProductoGuardarDto dto)
    {
        if (dto.IdCategoriaProducto <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar una categoría.");
        }

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
        {
            throw new ArgumentException(
                "El precio de compra no puede ser negativo.");
        }

        if (dto.PrecioVenta < 0)
        {
            throw new ArgumentException(
                "El precio de venta no puede ser negativo.");
        }

        if (dto.Stock < 0)
        {
            throw new ArgumentException(
                "El stock no puede ser negativo.");
        }

        if (dto.StockMinimo < 0)
        {
            throw new ArgumentException(
                "El stock mínimo no puede ser negativo.");
        }
    }

    // =====================================================
    // MAPEAR DTO
    // =====================================================

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

            CodigoBarras =
                producto.CodigoBarras,

            Nombre =
                producto.Nombre,

            Descripcion =
                producto.Descripcion,

            ImagenUrl =
                producto.ImagenUrl,

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