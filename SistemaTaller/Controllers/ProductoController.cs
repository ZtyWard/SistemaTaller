using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

public class ProductoController : Controller
{
    private readonly IProductoService _service;
    private readonly ICategoriaProductoService _categoriaService;
    private readonly IWebHostEnvironment _environment;

    private const long MaxImageSize = 5 * 1024 * 1024;

    private static readonly string[] ExtensionesPermitidas =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    private static readonly string[] ContentTypesPermitidos =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    public ProductoController(
        IProductoService service,
        ICategoriaProductoService categoriaService,
        IWebHostEnvironment environment)
    {
        _service = service;
        _categoriaService = categoriaService;
        _environment = environment;
    }

    // =====================================================
    // MÉTODO AUXILIAR — CARGAR CATEGORÍAS
    // =====================================================

    private async Task CargarCategoriasAsync()
    {
        ViewBag.Categorias =
            await _categoriaService
                .ObtenerActivasAsync();
    }

    // =====================================================
    // INDEX
    // =====================================================

    [Authorize(Policy = Permisos.ProductosVer)]
    public async Task<IActionResult> Index()
    {
        var productos =
            await _service.ObtenerTodosAsync();

        return View(productos);
    }

    // =====================================================
    // ACTIVOS
    // =====================================================

    [Authorize(Policy = Permisos.ProductosVer)]
    public async Task<IActionResult> Activos()
    {
        var productos =
            await _service.ObtenerActivosAsync();

        return View("Index", productos);
    }

    // =====================================================
    // STOCK BAJO
    // =====================================================

    [Authorize(Policy = Permisos.ProductosVer)]
    public async Task<IActionResult> StockBajo()
    {
        var productos =
            await _service.ObtenerStockBajoAsync();

        return View("Index", productos);
    }

    // =====================================================
    // POR CATEGORÍA
    // =====================================================

    [Authorize(Policy = Permisos.ProductosVer)]
    public async Task<IActionResult> PorCategoria(
        int idCategoriaProducto)
    {
        var productos =
            await _service
                .ObtenerPorCategoriaAsync(
                    idCategoriaProducto);

        return View("Index", productos);
    }

    // =====================================================
    // DETAILS
    // =====================================================

    [Authorize(Policy = Permisos.ProductosVer)]
    public async Task<IActionResult> Details(int id)
    {
        var producto =
            await _service.ObtenerPorIdAsync(id);

        if (producto == null)
            return NotFound();

        return View(producto);
    }

    // =====================================================
    // BUSCAR POR CÓDIGO
    // =====================================================

    [Authorize(Policy = Permisos.ProductosVer)]
    public async Task<IActionResult> BuscarPorCodigo(
        string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return RedirectToAction(nameof(Index));

        var producto =
            await _service.ObtenerPorCodigoAsync(codigo);

        if (producto == null)
        {
            TempData["Error"] =
                "No se encontró ningún producto con ese código.";

            return RedirectToAction(nameof(Index));
        }

        return View("Details", producto);
    }

    // =====================================================
    // BUSCAR POR CÓDIGO DE BARRAS
    // =====================================================

    [Authorize(Policy = Permisos.ProductosVer)]
    public async Task<IActionResult> BuscarPorCodigoBarras(
        string codigoBarras)
    {
        if (string.IsNullOrWhiteSpace(codigoBarras))
            return RedirectToAction(nameof(Index));

        var producto =
            await _service
                .ObtenerPorCodigoBarrasAsync(
                    codigoBarras);

        if (producto == null)
        {
            TempData["Error"] =
                "No se encontró ningún producto con ese código de barras.";

            return RedirectToAction(nameof(Index));
        }

        return View("Details", producto);
    }

    // =====================================================
    // CREATE - GET
    // =====================================================

    [Authorize(Policy = Permisos.ProductosCrear)]
    public async Task<IActionResult> Create()
    {
        await CargarCategoriasAsync();

        return View(
            new ProductoGuardarDto
            {
                StockMinimo = 5,
                Activo = true
            });
    }

    // =====================================================
    // CREATE - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ProductosCrear)]
    public async Task<IActionResult> Create(
        ProductoGuardarDto dto,
        IFormFile? Imagen)
    {
        string? imagenGuardada = null;

        try
        {
            if (!ModelState.IsValid)
            {
                await CargarCategoriasAsync();
                return View(dto);
            }

            // =================================================
            // GUARDAR IMAGEN
            // =================================================

            if (Imagen != null &&
                Imagen.Length > 0)
            {
                imagenGuardada =
                    await GuardarImagenAsync(Imagen);

                dto.ImagenUrl =
                    imagenGuardada;
            }

            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Producto registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            // Si la creación falla después de guardar la imagen,
            // eliminamos el archivo para no dejar basura.
            if (!string.IsNullOrWhiteSpace(
                    imagenGuardada))
            {
                EliminarImagen(imagenGuardada);
            }

            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            await CargarCategoriasAsync();

            return View(dto);
        }
        catch (InvalidOperationException ex)
        {
            if (!string.IsNullOrWhiteSpace(
                    imagenGuardada))
            {
                EliminarImagen(imagenGuardada);
            }

            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            await CargarCategoriasAsync();

            return View(dto);
        }
    }

    // =====================================================
    // EDIT - GET
    // =====================================================

    [Authorize(Policy = Permisos.ProductosEditar)]
    public async Task<IActionResult> Edit(int id)
    {
        var producto =
            await _service.ObtenerPorIdAsync(id);

        if (producto == null)
            return NotFound();

        var dto =
            new ProductoGuardarDto
            {
                IdCategoriaProducto =
                    producto.IdCategoriaProducto,

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

        ViewBag.IdProducto =
            producto.IdProducto;

        ViewBag.ImagenActual =
            producto.ImagenUrl;

        await CargarCategoriasAsync();

        return View(dto);
    }

    // =====================================================
    // EDIT - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ProductosEditar)]
    public async Task<IActionResult> Edit(
        int id,
        ProductoGuardarDto dto,
        IFormFile? Imagen)
    {
        string? imagenNueva = null;

        try
        {
            var productoActual =
                await _service.ObtenerPorIdAsync(id);

            if (productoActual == null)
                return NotFound();

            // Conservar imagen actual
            dto.ImagenUrl =
                productoActual.ImagenUrl;

            if (!ModelState.IsValid)
            {
                ViewBag.IdProducto = id;
                ViewBag.ImagenActual =
                    productoActual.ImagenUrl;

                await CargarCategoriasAsync();

                return View(dto);
            }

            // =============================================
            // SI SUBE UNA NUEVA IMAGEN
            // =============================================

            if (Imagen != null &&
                Imagen.Length > 0)
            {
                imagenNueva =
                    await GuardarImagenAsync(Imagen);

                dto.ImagenUrl =
                    imagenNueva;
            }

            var actualizado =
                await _service
                    .ActualizarAsync(
                        id,
                        dto);

            if (!actualizado)
            {
                if (!string.IsNullOrWhiteSpace(
                        imagenNueva))
                {
                    EliminarImagen(imagenNueva);
                }

                return NotFound();
            }

            // =============================================
            // ELIMINAR IMAGEN ANTERIOR SI SE REEMPLAZÓ
            // =============================================

            if (!string.IsNullOrWhiteSpace(
                    imagenNueva) &&
                !string.IsNullOrWhiteSpace(
                    productoActual.ImagenUrl))
            {
                EliminarImagen(
                    productoActual.ImagenUrl);
            }

            TempData["Success"] =
                "Producto actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            if (!string.IsNullOrWhiteSpace(
                    imagenNueva))
            {
                EliminarImagen(imagenNueva);
            }

            ViewBag.IdProducto = id;

            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            await CargarCategoriasAsync();

            return View(dto);
        }
        catch (InvalidOperationException ex)
        {
            if (!string.IsNullOrWhiteSpace(
                    imagenNueva))
            {
                EliminarImagen(imagenNueva);
            }

            ViewBag.IdProducto = id;

            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            await CargarCategoriasAsync();

            return View(dto);
        }
    }

    // =====================================================
    // CAMBIAR ESTADO
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ProductosDesactivar)]
    public async Task<IActionResult> CambiarEstado(
        int id,
        bool activo)
    {
        var actualizado =
            await _service
                .CambiarEstadoAsync(
                    id,
                    activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Producto activado correctamente."
                : "Producto desactivado correctamente.";

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // GUARDAR IMAGEN
    // =====================================================

    private async Task<string> GuardarImagenAsync(
        IFormFile imagen)
    {
        if (imagen.Length <= 0)
        {
            throw new ArgumentException(
                "La imagen seleccionada está vacía.");
        }

        if (imagen.Length > MaxImageSize)
        {
            throw new ArgumentException(
                "La imagen no puede superar los 5 MB.");
        }

        var extension =
            Path.GetExtension(
                imagen.FileName)
            .ToLowerInvariant();

        if (!ExtensionesPermitidas.Contains(
                extension))
        {
            throw new ArgumentException(
                "Formato de imagen no permitido. Use JPG, JPEG, PNG o WEBP.");
        }

        if (!ContentTypesPermitidos.Contains(
                imagen.ContentType.ToLowerInvariant()))
        {
            throw new ArgumentException(
                "El tipo de imagen no es válido.");
        }

        var carpeta =
            Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "productos");

        Directory.CreateDirectory(carpeta);

        var nombreArchivo =
            $"{Guid.NewGuid():N}{extension}";

        var rutaCompleta =
            Path.Combine(
                carpeta,
                nombreArchivo);

        await using var stream =
            new FileStream(
                rutaCompleta,
                FileMode.Create);

        await imagen.CopyToAsync(stream);

        return
            $"/uploads/productos/{nombreArchivo}";
    }

    // =====================================================
    // ELIMINAR IMAGEN
    // =====================================================

    private void EliminarImagen(
        string? imagenUrl)
    {
        if (string.IsNullOrWhiteSpace(
                imagenUrl))
        {
            return;
        }

        try
        {
            var rutaRelativa =
                imagenUrl
                    .TrimStart('/')
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar);

            var rutaCompleta =
                Path.Combine(
                    _environment.WebRootPath,
                    rutaRelativa);

            if (System.IO.File.Exists(
                    rutaCompleta))
            {
                System.IO.File.Delete(
                    rutaCompleta);
            }
        }
        catch
        {
            // No interrumpimos la operación principal
            // si falla la eliminación del archivo anterior.
        }
    }
}