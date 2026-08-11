// =====================================================
// OrdenTrabajoController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

// =====================================================

using Microsoft.AspNetCore.Authorization;
public class OrdenTrabajoController : Controller
{
    private readonly IOrdenTrabajoService _service;

    public OrdenTrabajoController(IOrdenTrabajoService service)
    {
        _service = service;
    }

    // GET: OrdenTrabajo
    [Authorize(Policy = Permisos.OrdenesVer)]
    public async Task<IActionResult> Index()
    {
        var ordenes = await _service.ObtenerTodasAsync();

        return View(ordenes);
    }

    // GET: OrdenTrabajo/Abiertas
    [Authorize(Policy = Permisos.OrdenesVer)]
    public async Task<IActionResult> Abiertas()
    {
        var ordenes = await _service.ObtenerAbiertasAsync();

        return View("Index", ordenes);
    }

    // GET: OrdenTrabajo/PorEstado?estado=Pendiente
    [Authorize(Policy = Permisos.OrdenesVer)]
    public async Task<IActionResult> PorEstado(string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
            return RedirectToAction(nameof(Index));

        var ordenes =
            await _service.ObtenerPorEstadoAsync(estado);

        return View("Index", ordenes);
    }

    // GET: OrdenTrabajo/Details/5
    [Authorize(Policy = Permisos.OrdenesVer)]
    public async Task<IActionResult> Details(int id)
    {
        var orden = await _service.ObtenerPorIdAsync(id);

        if (orden == null)
            return NotFound();

        return View(orden);
    }

    // GET: OrdenTrabajo/Create
    [Authorize(Policy = Permisos.OrdenesCrear)]
    public IActionResult Create()
    {
        return View();
    }

    // POST: OrdenTrabajo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.OrdenesCrear)]
    public async Task<IActionResult> Create(
        OrdenTrabajoGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Orden de trabajo creada correctamente.";

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

    // GET: OrdenTrabajo/Edit/5
    [Authorize(Policy = Permisos.OrdenesEditar)]
    public async Task<IActionResult> Edit(int id)
    {
        var orden = await _service.ObtenerPorIdAsync(id);

        if (orden == null)
            return NotFound();

        var dto = new OrdenTrabajoGuardarDto
        {
            IdCotizacion = orden.IdCotizacion,
            Observaciones = orden.Observaciones
        };

        ViewBag.IdOrdenTrabajo = orden.IdOrdenTrabajo;

        return View(dto);
    }

    // POST: OrdenTrabajo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.OrdenesEditar)]
    public async Task<IActionResult> Edit(
        int id,
        OrdenTrabajoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdOrdenTrabajo = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Orden de trabajo actualizada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdOrdenTrabajo = id;

            return View(dto);
        }
    }

    // POST: OrdenTrabajo/CambiarEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.OrdenesAprobar)]
    public async Task<IActionResult> CambiarEstado(
        int id,
        string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            TempData["Error"] =
                "El estado de la orden de trabajo es obligatorio.";

            return RedirectToAction(nameof(Index));
        }

        var actualizado =
            await _service.CambiarEstadoAsync(id, estado);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            "Estado de la orden de trabajo actualizado correctamente.";

        return RedirectToAction(nameof(Index));
    }

    // POST: OrdenTrabajo/Finalizar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.OrdenesAprobar)]
    public async Task<IActionResult> Finalizar(int id)
    {
        var finalizada =
            await _service.FinalizarAsync(id);

        if (!finalizada)
            return NotFound();

        TempData["Success"] =
            "Orden de trabajo finalizada correctamente.";

        return RedirectToAction(nameof(Index));
    }
}