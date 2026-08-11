// =====================================================
// CatalogoProductoController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

// =====================================================

using Microsoft.AspNetCore.Authorization;
public class CatalogoProductoController : Controller
{
    private readonly ICategoriaProductoService _service;

    public CatalogoProductoController(
        ICategoriaProductoService service)
    {
        _service = service;
    }

    // GET: CatalogoProducto/Categorias
    public async Task<IActionResult> Categorias()
    {
        var categorias = await _service.ObtenerTodosAsync();

        return View(categorias);
    }

    // GET: CatalogoProducto/CategoriasActivas
    public async Task<IActionResult> CategoriasActivas()
    {
        var categorias = await _service.ObtenerActivasAsync();

        return View("Categorias", categorias);
    }

    // GET: CatalogoProducto/DetalleCategoria/5
    public async Task<IActionResult> DetalleCategoria(int id)
    {
        var categoria = await _service.ObtenerPorIdAsync(id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    // GET: CatalogoProducto/CrearCategoria
    [Authorize(Policy = Permisos.ProductosCrear)]
    public IActionResult CrearCategoria()
    {
        return View();
    }

    // POST: CatalogoProducto/CrearCategoria
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ProductosCrear)]
    public async Task<IActionResult> CrearCategoria(
        CategoriaProductoGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Categoría de producto creada correctamente.";

            return RedirectToAction(nameof(Categorias));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }

    // GET: CatalogoProducto/EditarCategoria/5
    [Authorize(Policy = Permisos.ProductosEditar)]
    public async Task<IActionResult> EditarCategoria(int id)
    {
        var categoria = await _service.ObtenerPorIdAsync(id);

        if (categoria == null)
            return NotFound();

        var dto = new CategoriaProductoGuardarDto
        {
            Nombre = categoria.Nombre,
            Activo = categoria.Activo
        };

        ViewBag.IdCategoriaProducto =
            categoria.IdCategoriaProducto;

        return View(dto);
    }

    // POST: CatalogoProducto/EditarCategoria/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ProductosEditar)]
    public async Task<IActionResult> EditarCategoria(
        int id,
        CategoriaProductoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdCategoriaProducto = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Categoría de producto actualizada correctamente.";

            return RedirectToAction(nameof(Categorias));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdCategoriaProducto = id;

            return View(dto);
        }
    }

    // POST: CatalogoProducto/CambiarEstadoCategoria/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ProductosDesactivar)]
    public async Task<IActionResult> CambiarEstadoCategoria(
        int id,
        bool activo)
    {
        var actualizado =
            await _service.CambiarEstadoAsync(id, activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Categoría activada correctamente."
                : "Categoría desactivada correctamente.";

        return RedirectToAction(nameof(Categorias));
    }
}