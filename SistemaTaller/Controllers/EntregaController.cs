using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

public class EntregaController : Controller
{
    private readonly IEntregaService _service;
    private readonly IOrdenTrabajoService _ordenTrabajoService;

    public EntregaController(
        IEntregaService service,
        IOrdenTrabajoService ordenTrabajoService)
    {
        _service = service;
        _ordenTrabajoService = ordenTrabajoService;
    }

    // =====================================================
    // INDEX
    // =====================================================

    [Authorize(Policy = Permisos.OrdenesVer)]
    public async Task<IActionResult> Index()
    {
        var entregas =
            await _service.ObtenerTodasAsync();

        return View(entregas);
    }

    // =====================================================
    // DETAILS
    // =====================================================

    [Authorize(Policy = Permisos.OrdenesVer)]
    public async Task<IActionResult> Details(int id)
    {
        var entrega =
            await _service.ObtenerPorIdAsync(id);

        if (entrega == null)
            return NotFound();

        return View(entrega);
    }

    // =====================================================
    // CREATE - GET
    // =====================================================

    [Authorize(Policy = Permisos.OrdenesCrear)]
    public async Task<IActionResult> Create()
    {
        await CargarOrdenesFinalizadasAsync();

        return View(
            new EntregaGuardarDto());
    }

    // =====================================================
    // CREATE - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.OrdenesCrear)]
    public async Task<IActionResult> Create(
        EntregaGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            await CargarOrdenesFinalizadasAsync();

            return View(dto);
        }

        try
        {
            var resultado =
                await _service.CrearAsync(dto);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                await CargarOrdenesFinalizadasAsync();

                return View(dto);
            }

            TempData["Success"] =
                resultado.Mensaje;

            return RedirectToAction(
                nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            await CargarOrdenesFinalizadasAsync();

            return View(dto);
        }
    }

    // =====================================================
    // EDIT - GET
    // =====================================================

    [Authorize(Policy = Permisos.OrdenesEditar)]
    public async Task<IActionResult> Edit(int id)
    {
        var entrega =
            await _service.ObtenerPorIdAsync(id);

        if (entrega == null)
            return NotFound();

        var dto = new EntregaGuardarDto
        {
            IdOrdenTrabajo =
                entrega.IdOrdenTrabajo,

            KilometrajeSalida =
                entrega.KilometrajeSalida,

            PersonaRecibe =
                entrega.PersonaRecibe,

            Observaciones =
                entrega.Observaciones,

            Recomendaciones =
                entrega.Recomendaciones,

            TieneGarantia =
                entrega.TieneGarantia,

            EstadoPago =
                entrega.EstadoPago,

            Aceptacion =
                entrega.Aceptacion,

            FirmaNombre =
                entrega.FirmaNombre
        };

        ViewBag.IdEntrega =
            entrega.IdEntrega;

        await CargarOrdenesFinalizadasAsync();

        return View(dto);
    }

    // =====================================================
    // EDIT - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.OrdenesEditar)]
    public async Task<IActionResult> Edit(
        int id,
        EntregaGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdEntrega = id;

            await CargarOrdenesFinalizadasAsync();

            return View(dto);
        }

        try
        {
            var resultado =
                await _service.ActualizarAsync(
                    id,
                    dto);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(
                    string.Empty,
                    resultado.Mensaje);

                ViewBag.IdEntrega = id;

                await CargarOrdenesFinalizadasAsync();

                return View(dto);
            }

            TempData["Success"] =
                resultado.Mensaje;

            return RedirectToAction(
                nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdEntrega = id;

            await CargarOrdenesFinalizadasAsync();

            return View(dto);
        }
    }

    // =====================================================
    // OBTENER POR OT
    // =====================================================

    [Authorize(Policy = Permisos.OrdenesVer)]
    public async Task<IActionResult> PorOrdenTrabajo(
        int idOrdenTrabajo)
    {
        var entrega =
            await _service
                .ObtenerPorOrdenTrabajoAsync(
                    idOrdenTrabajo);

        if (entrega == null)
            return NotFound();

        return View(
            "Details",
            entrega);
    }

    // =====================================================
    // CARGAR OT FINALIZADAS
    // =====================================================

    private async Task CargarOrdenesFinalizadasAsync()
    {
        var ordenes =
            await _ordenTrabajoService
                .ObtenerTodasAsync();

        var finalizadas =
            ordenes
                .Where(x =>
                    string.Equals(
                        x.Estado,
                        "Finalizada",
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

        var entregas =
            await _service.ObtenerTodasAsync();

        var idsConEntrega =
            entregas
                .Select(x => x.IdOrdenTrabajo)
                .ToHashSet();

        ViewBag.OrdenesFinalizadas =
            finalizadas
                .Where(x =>
                    !idsConEntrega.Contains(
                        x.IdOrdenTrabajo))
                .ToList();
    }
}