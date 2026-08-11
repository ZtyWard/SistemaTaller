// =====================================================
// ServicioController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

// =====================================================

using Microsoft.AspNetCore.Authorization;
public class ServicioController : Controller
{
    private readonly IServicioService _service;

    public ServicioController(IServicioService service)
    {
        _service = service;
    }

    // GET: Servicio
    [Authorize(Policy = Permisos.ServiciosVer)]
    public async Task<IActionResult> Index()
    {
        var servicios = await _service.ObtenerTodosAsync();

        return View(servicios);
    }

    // GET: Servicio/Activos
    [Authorize(Policy = Permisos.ServiciosVer)]
    public async Task<IActionResult> Activos()
    {
        var servicios = await _service.ObtenerActivosAsync();

        return View("Index", servicios);
    }

    // GET: Servicio/Details/5
    [Authorize(Policy = Permisos.ServiciosVer)]
    public async Task<IActionResult> Details(int id)
    {
        var servicio = await _service.ObtenerPorIdAsync(id);

        if (servicio == null)
            return NotFound();

        return View(servicio);
    }

    // GET: Servicio/Create
    [Authorize(Policy = Permisos.ServiciosCrear)]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Servicio/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosCrear)]
    public async Task<IActionResult> Create(
        ServicioGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Servicio registrado correctamente.";

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

    // GET: Servicio/Edit/5
    [Authorize(Policy = Permisos.ServiciosEditar)]
    public async Task<IActionResult> Edit(int id)
    {
        var servicio = await _service.ObtenerPorIdAsync(id);

        if (servicio == null)
            return NotFound();

        var dto = new ServicioGuardarDto
        {
            Nombre = servicio.Nombre,
            Descripcion = servicio.Descripcion,
            Precio = servicio.Precio,
            DuracionEstimada = servicio.DuracionEstimada,
            Activo = servicio.Activo
        };

        ViewBag.IdServicio = servicio.IdServicio;

        return View(dto);
    }

    // POST: Servicio/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosEditar)]
    public async Task<IActionResult> Edit(
        int id,
        ServicioGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdServicio = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Servicio actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdServicio = id;

            return View(dto);
        }
    }

    // POST: Servicio/CambiarEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosDesactivar)]
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
                ? "Servicio activado correctamente."
                : "Servicio desactivado correctamente.";

        return RedirectToAction(nameof(Index));
    }
}