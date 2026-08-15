using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

public class GarantiaController : Controller
{
    private readonly IGarantiaService _service;

    public GarantiaController(
        IGarantiaService service)
    {
        _service = service;
    }

    // =====================================================
    // INDEX
    // =====================================================

    [Authorize(Policy = Permisos.ReportesVer)]
    public async Task<IActionResult> Index()
    {
        try
        {
            var garantias =
                await _service.ObtenerTodasAsync();

            return View(garantias);
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                $"No fue posible cargar las garantías: {ex.Message}";

            return View(
                Enumerable.Empty<GarantiaDto>());
        }
    }

    // =====================================================
    // VIGENTES
    // =====================================================

    [Authorize(Policy = Permisos.ReportesVer)]
    public async Task<IActionResult> Vigentes()
    {
        var garantias =
            await _service.ObtenerVigentesAsync();

        return View(
            "Index",
            garantias);
    }

    // =====================================================
    // POR VENCER
    // =====================================================

    [Authorize(Policy = Permisos.ReportesVer)]
    public async Task<IActionResult> PorVencer(
        int dias = 30)
    {
        if (dias < 0)
            dias = 0;

        var garantias =
            await _service.ObtenerPorVencerAsync(
                dias);

        ViewBag.Dias = dias;

        return View(
            "Index",
            garantias);
    }

    // =====================================================
    // POR ORDEN DE TRABAJO
    // =====================================================

    [Authorize(Policy = Permisos.OrdenesVer)]
    public async Task<IActionResult> PorOrdenTrabajo(
        int idOrdenTrabajo)
    {
        if (idOrdenTrabajo <= 0)
            return BadRequest();

        var garantias =
            await _service
                .ObtenerPorOrdenTrabajoAsync(
                    idOrdenTrabajo);

        ViewBag.IdOrdenTrabajo =
            idOrdenTrabajo;

        return View(
            "Index",
            garantias);
    }

    // =====================================================
    // POR VENTA
    // =====================================================

    [Authorize(Policy = Permisos.VentasVer)]
    public async Task<IActionResult> PorVenta(
        int idVenta)
    {
        if (idVenta <= 0)
            return BadRequest();

        var garantias =
            await _service
                .ObtenerPorVentaAsync(
                    idVenta);

        ViewBag.IdVenta =
            idVenta;

        return View(
            "Index",
            garantias);
    }

    // =====================================================
    // DETAILS
    // =====================================================

    [Authorize(Policy = Permisos.ReportesVer)]
    public async Task<IActionResult> Details(
        int id)
    {
        var garantia =
            await _service.ObtenerPorIdAsync(id);

        if (garantia == null)
            return NotFound();

        return View(garantia);
    }

    // =====================================================
    // CREATE - GET
    // =====================================================

    [Authorize(Policy = Permisos.ServiciosCrear)]
    public IActionResult Create()
    {
        return View(
            new GarantiaGuardarDto());
    }

    // =====================================================
    // CREATE - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosCrear)]
    public async Task<IActionResult> Create(
        GarantiaGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            var id =
                await _service.CrearAsync(dto);

            TempData["Success"] =
                "Garantía registrada correctamente.";

            return RedirectToAction(
                nameof(Details),
                new { id });
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

    // =====================================================
    // EDIT - GET
    // =====================================================

    [Authorize(Policy = Permisos.ServiciosEditar)]
    public async Task<IActionResult> Edit(
        int id)
    {
        var garantia =
            await _service.ObtenerPorIdAsync(id);

        if (garantia == null)
            return NotFound();

        var dto =
            new GarantiaGuardarDto
            {
                IdOrdenTrabajo =
                    garantia.IdOrdenTrabajo,

                IdVenta =
                    garantia.IdVenta,

                IdProducto =
                    garantia.IdProducto,

                IdServicio =
                    garantia.IdServicio,

                FechaInicio =
                    garantia.FechaInicio,

                FechaVencimiento =
                    garantia.FechaVencimiento,

                Condiciones =
                    garantia.Condiciones,

                Observaciones =
                    garantia.Observaciones
            };

        ViewBag.IdGarantia =
            garantia.IdGarantia;

        return View(dto);
    }

    // =====================================================
    // EDIT - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosEditar)]
    public async Task<IActionResult> Edit(
        int id,
        GarantiaGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdGarantia = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(
                    id,
                    dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Garantía actualizada correctamente.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdGarantia = id;

            return View(dto);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdGarantia = id;

            return View(dto);
        }
    }

    // =====================================================
    // RECLAMAR
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosEditar)]
    public async Task<IActionResult> Reclamar(
        int id,
        string motivo)
    {
        try
        {
            var resultado =
                await _service.RegistrarReclamoAsync(
                    id,
                    motivo);

            if (!resultado)
                return NotFound();

            TempData["Success"] =
                "Reclamo de garantía registrado correctamente.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
        catch (ArgumentException ex)
        {
            TempData["Error"] =
                ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] =
                ex.Message;
        }

        return RedirectToAction(
            nameof(Details),
            new { id });
    }

    // =====================================================
    // RESOLVER
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosEditar)]
    public async Task<IActionResult> Resolver(
        int id,
        string resolucion)
    {
        try
        {
            var resultado =
                await _service.ResolverAsync(
                    id,
                    resolucion);

            if (!resultado)
                return NotFound();

            TempData["Success"] =
                "Garantía resuelta correctamente.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
        catch (ArgumentException ex)
        {
            TempData["Error"] =
                ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] =
                ex.Message;
        }

        return RedirectToAction(
            nameof(Details),
            new { id });
    }

    // =====================================================
    // RECHAZAR
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosEditar)]
    public async Task<IActionResult> Rechazar(
        int id,
        string motivo)
    {
        try
        {
            var resultado =
                await _service.RechazarAsync(
                    id,
                    motivo);

            if (!resultado)
                return NotFound();

            TempData["Success"] =
                "Reclamo rechazado correctamente.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
        catch (ArgumentException ex)
        {
            TempData["Error"] =
                ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] =
                ex.Message;
        }

        return RedirectToAction(
            nameof(Details),
            new { id });
    }

    // =====================================================
    // ACTUALIZAR VENCIDAS
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ServiciosEditar)]
    public async Task<IActionResult>
        ActualizarVencidas()
    {
        var cantidad =
            await _service
                .ActualizarGarantiasVencidasAsync();

        TempData["Success"] =
            cantidad == 0
                ? "No había garantías pendientes de vencer."
                : $"{cantidad} garantía(s) actualizada(s) a estado Vencida.";

        return RedirectToAction(
            nameof(Index));
    }
}