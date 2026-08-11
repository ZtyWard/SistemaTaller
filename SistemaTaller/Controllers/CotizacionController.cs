// =====================================================
// CotizacionController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

// =====================================================

using Microsoft.AspNetCore.Authorization;
public class CotizacionController : Controller
{
    private readonly ICotizacionService _service;

    public CotizacionController(ICotizacionService service)
    {
        _service = service;
    }

    // GET: Cotizacion
    [Authorize(Policy = Permisos.CotizacionesVer)]
    public async Task<IActionResult> Index()
    {
        var cotizaciones = await _service.ObtenerTodasAsync();

        return View(cotizaciones);
    }

    // GET: Cotizacion/Pendientes
    [Authorize(Policy = Permisos.CotizacionesVer)]
    public async Task<IActionResult> Pendientes()
    {
        var cotizaciones = await _service.ObtenerPendientesAsync();

        return View("Index", cotizaciones);
    }

    // GET: Cotizacion/PorDiagnostico/5
    [Authorize(Policy = Permisos.CotizacionesVer)]
    public async Task<IActionResult> PorDiagnostico(int idDiagnostico)
    {
        var cotizaciones =
            await _service.ObtenerPorDiagnosticoAsync(idDiagnostico);

        return View("Index", cotizaciones);
    }

    // GET: Cotizacion/Details/5
    [Authorize(Policy = Permisos.CotizacionesVer)]
    public async Task<IActionResult> Details(int id)
    {
        var cotizacion = await _service.ObtenerPorIdAsync(id);

        if (cotizacion == null)
            return NotFound();

        return View(cotizacion);
    }

    // GET: Cotizacion/Create
    [Authorize(Policy = Permisos.CotizacionesCrear)]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Cotizacion/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.CotizacionesCrear)]
    public async Task<IActionResult> Create(
        CotizacionGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Cotización registrada correctamente.";

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

    // GET: Cotizacion/Edit/5
    [Authorize(Policy = Permisos.CotizacionesEditar)]
    public async Task<IActionResult> Edit(int id)
    {
        var cotizacion = await _service.ObtenerPorIdAsync(id);

        if (cotizacion == null)
            return NotFound();

        var dto = new CotizacionGuardarDto
        {
            IdDiagnostico = cotizacion.IdDiagnostico,
            Total = cotizacion.Total,
            Estado = cotizacion.Estado
        };

        ViewBag.IdCotizacion = cotizacion.IdCotizacion;

        return View(dto);
    }

    // POST: Cotizacion/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.CotizacionesEditar)]
    public async Task<IActionResult> Edit(
        int id,
        CotizacionGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdCotizacion = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Cotización actualizada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdCotizacion = id;

            return View(dto);
        }
    }

    // POST: Cotizacion/CambiarEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.CotizacionesAprobar)]
    public async Task<IActionResult> CambiarEstado(
        int id,
        string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            TempData["Error"] =
                "El estado de la cotización es obligatorio.";

            return RedirectToAction(nameof(Index));
        }

        var actualizado =
            await _service.CambiarEstadoAsync(id, estado);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            "Estado de la cotización actualizado correctamente.";

        return RedirectToAction(nameof(Index));
    }
}