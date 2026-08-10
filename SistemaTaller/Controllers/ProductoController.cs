using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace SistemaTaller.Controllers;

public class ProductoController : Controller
{
    private readonly IProductoService _service;

    public ProductoController(IProductoService service)
    {
        _service = service;
    }

    // GET: Producto
    public async Task<IActionResult> Index()
    {
        var productos = await _service.ObtenerTodosAsync();

        return View(productos);
    }

    // GET: Producto/Activos
    public async Task<IActionResult> Activos()
    {
        var productos = await _service.ObtenerActivosAsync();

        return View("Index", productos);
    }

    // GET: Producto/StockBajo
    public async Task<IActionResult> StockBajo()
    {
        var productos = await _service.ObtenerStockBajoAsync();

        return View("Index", productos);
    }

    // GET: Producto/PorCategoria/5
    public async Task<IActionResult> PorCategoria(
        int idCategoriaProducto)
    {
        var productos =
            await _service.ObtenerPorCategoriaAsync(
                idCategoriaProducto);

        return View("Index", productos);
    }

    // GET: Producto/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var producto = await _service.ObtenerPorIdAsync(id);

        if (producto == null)
            return NotFound();

        return View(producto);
    }

    // GET: Producto/BuscarPorCodigo
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

    // GET: Producto/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Producto/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ProductoGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Producto registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }

    // GET: Producto/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _service.ObtenerPorIdAsync(id);

        if (producto == null)
            return NotFound();

        var dto = new ProductoGuardarDto
        {
            IdCategoriaProducto =
                producto.IdCategoriaProducto,

            Codigo = producto.Codigo,

            Nombre = producto.Nombre,

            Descripcion = producto.Descripcion,

            PrecioCompra = producto.PrecioCompra,

            PrecioVenta = producto.PrecioVenta,

            Stock = producto.Stock,

            StockMinimo = producto.StockMinimo,

            Activo = producto.Activo
        };

        ViewBag.IdProducto = producto.IdProducto;

        return View(dto);
    }

    // POST: Producto/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        ProductoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdProducto = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Producto actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdProducto = id;

            return View(dto);
        }
    }

    // POST: Producto/CambiarEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(
        int id,
        bool activo)
    {
        var actualizado =
            await _service.CambiarEstadoAsync(
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
}