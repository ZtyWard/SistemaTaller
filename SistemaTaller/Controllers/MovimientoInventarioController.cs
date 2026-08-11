// =====================================================
// MovimientoInventarioController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

// =====================================================

using Microsoft.AspNetCore.Authorization;
public class MovimientoInventarioController : Controller
{
    private readonly IMovimientoInventarioService _service;

    public MovimientoInventarioController(
        IMovimientoInventarioService service)
    {
        _service = service;
    }

    // GET: MovimientoInventario
    [Authorize(Policy = Permisos.MovimientosInventarioVer)]
    public async Task<IActionResult> Index()
    {
        var movimientos =
            await _service.ObtenerTodosAsync();

        return View(movimientos);
    }

    // GET: MovimientoInventario/PorProducto/5
    [Authorize(Policy = Permisos.MovimientosInventarioVer)]
    public async Task<IActionResult> PorProducto(
        int idProducto)
    {
        var movimientos =
            await _service.ObtenerPorProductoAsync(
                idProducto);

        return View("Index", movimientos);
    }

    // GET: MovimientoInventario/PorTipo?tipoMovimiento=Entrada
    public async Task<IActionResult> PorTipo(
        string tipoMovimiento)
    {
        if (string.IsNullOrWhiteSpace(tipoMovimiento))
            return RedirectToAction(nameof(Index));

        var movimientos =
            await _service.ObtenerPorTipoAsync(
                tipoMovimiento);

        return View("Index", movimientos);
    }

    // GET: MovimientoInventario/Recientes?cantidad=10
    [Authorize(Policy = Permisos.MovimientosInventarioVer)]
    public async Task<IActionResult> Recientes(
        int cantidad = 10)
    {
        if (cantidad <= 0)
            cantidad = 10;

        var movimientos =
            await _service.ObtenerRecientesAsync(
                cantidad);

        return View("Index", movimientos);
    }

    // GET: MovimientoInventario/Details/5
    [Authorize(Policy = Permisos.MovimientosInventarioVer)]
    public async Task<IActionResult> Details(int id)
    {
        var movimiento =
            await _service.ObtenerPorIdAsync(id);

        if (movimiento == null)
            return NotFound();

        return View(movimiento);
    }

    // GET: MovimientoInventario/Create
    [Authorize(Policy = Permisos.MovimientosInventarioCrear)]
    public IActionResult Create()
    {
        return View();
    }

    // POST: MovimientoInventario/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.MovimientosInventarioCrear)]
    public async Task<IActionResult> Create(
        MovimientoInventarioGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Movimiento de inventario registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }
}