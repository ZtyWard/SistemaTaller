using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace SistemaTaller.Controllers;

public class CompraController : Controller
{
    private readonly ICompraService _service;

    public CompraController(ICompraService service)
    {
        _service = service;
    }

    // GET: Compra
    public async Task<IActionResult> Index()
    {
        var compras = await _service.ObtenerTodosAsync();

        return View(compras);
    }

    // GET: Compra/PorProveedor/5
    public async Task<IActionResult> PorProveedor(int idProveedor)
    {
        var compras =
            await _service.ObtenerPorProveedorAsync(idProveedor);

        return View("Index", compras);
    }

    // GET: Compra/PorEstado?estado=Pendiente
    public async Task<IActionResult> PorEstado(string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
            return RedirectToAction(nameof(Index));

        var compras =
            await _service.ObtenerPorEstadoAsync(estado);

        return View("Index", compras);
    }

    // GET: Compra/Recientes
    public async Task<IActionResult> Recientes(int cantidad = 10)
    {
        if (cantidad <= 0)
            cantidad = 10;

        var compras =
            await _service.ObtenerRecientesAsync(cantidad);

        return View("Index", compras);
    }

    // GET: Compra/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var compra = await _service.ObtenerPorIdAsync(id);

        if (compra == null)
            return NotFound();

        return View(compra);
    }

    // GET: Compra/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Compra/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CompraGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Compra registrada correctamente.";

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

    // GET: Compra/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var compra = await _service.ObtenerPorIdAsync(id);

        if (compra == null)
            return NotFound();

        var dto = new CompraGuardarDto
        {
            IdProveedor = compra.IdProveedor,
            FechaCompra = compra.FechaCompra,
            Total = compra.Total,
            Estado = compra.Estado
        };

        ViewBag.IdCompra = compra.IdCompra;

        return View(dto);
    }

    // POST: Compra/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        CompraGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdCompra = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Compra actualizada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdCompra = id;

            return View(dto);
        }
    }

    // POST: Compra/CambiarEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(
        int id,
        string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            TempData["Error"] =
                "El estado de la compra es obligatorio.";

            return RedirectToAction(nameof(Index));
        }

        var actualizado =
            await _service.CambiarEstadoAsync(id, estado);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            "Estado de la compra actualizado correctamente.";

        return RedirectToAction(nameof(Index));
    }
}