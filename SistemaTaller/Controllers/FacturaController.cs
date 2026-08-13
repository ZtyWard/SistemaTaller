using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

[Authorize]
public class FacturaController : Controller
{
    private readonly IFacturaService _service;

    public FacturaController(
        IFacturaService service)
    {
        _service = service;
    }

    // =====================================================
    // INDEX
    // =====================================================

    [Authorize(Policy = Permisos.FacturacionVer)]
    public async Task<IActionResult> Index()
    {
        var facturas =
            await _service.ObtenerTodosAsync();

        return View(facturas);
    }

    // =====================================================
    // PENDIENTES
    // =====================================================

    [Authorize(Policy = Permisos.FacturacionVer)]
    public async Task<IActionResult> Pendientes()
    {
        var facturas =
            await _service.ObtenerPendientesAsync();

        return View("Index", facturas);
    }

    // =====================================================
    // DETAILS
    // =====================================================

    [Authorize(Policy = Permisos.FacturacionVer)]
    public async Task<IActionResult> Details(
        int id)
    {
        var factura =
            await _service.ObtenerPorIdAsync(id);

        if (factura == null)
            return NotFound();

        return View(factura);
    }

    // =====================================================
    // CREATE
    // =====================================================

    [Authorize(Policy = Permisos.FacturacionCrear)]
    public IActionResult Create()
    {
        return View(
            new FacturaGuardarDto
            {
                FechaEmision = DateTime.Now,
                Estado = "Pendiente"
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.FacturacionCrear)]
    public async Task<IActionResult> Create(
        FacturaGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Factura registrada correctamente.";

            return RedirectToAction(
                nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }

    // =====================================================
    // EDIT
    // =====================================================

    [Authorize(Policy = Permisos.FacturacionCrear)]
    public async Task<IActionResult> Edit(
        int id)
    {
        var factura =
            await _service.ObtenerPorIdAsync(id);

        if (factura == null)
            return NotFound();

        var dto = new FacturaGuardarDto
        {
            NumeroFactura =
                factura.NumeroFactura,

            IdCliente =
                factura.IdCliente,

            IdOrdenTrabajo =
                factura.IdOrdenTrabajo,

            IdVenta =
                factura.IdVenta,

            FechaEmision =
                factura.FechaEmision,

            Subtotal =
                factura.Subtotal,

            Impuesto =
                factura.Impuesto,

            Descuento =
                factura.Descuento,

            Total =
                factura.Total,

            Estado =
                factura.Estado
        };

        ViewBag.IdFactura =
            factura.IdFactura;

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.FacturacionCrear)]
    public async Task<IActionResult> Edit(
        int id,
        FacturaGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdFactura = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service
                    .ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Factura actualizada correctamente.";

            return RedirectToAction(
                nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdFactura = id;

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdFactura = id;

            return View(dto);
        }
    }

    // =====================================================
    // ANULAR
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.FacturacionAnular)]
    public async Task<IActionResult> Anular(
        int id)
    {
        try
        {
            var anulada =
                await _service.AnularAsync(id);

            if (!anulada)
                return NotFound();

            TempData["Success"] =
                "Factura anulada correctamente.";

            return RedirectToAction(
                nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                nameof(Index));
        }
    }
}