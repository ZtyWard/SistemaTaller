using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;
using Negocios.Services;
using System.Security.Claims;

namespace SistemaTaller.Controllers;

[Authorize]
public class PagoController : Controller
{
    private readonly IPagoService _service;

    public PagoController(
        IPagoService service)
    {
        _service = service;
    }

    // =====================================================
    // INDEX
    // =====================================================

    [Authorize(Policy = Permisos.PagosVer)]
    public async Task<IActionResult> Index()
    {
        var pagos =
            await _service
                .ObtenerRecientesAsync();

        return View(pagos);
    }

    // =====================================================
    // DETAILS
    // =====================================================

    [Authorize(Policy = Permisos.PagosVer)]
    public async Task<IActionResult> Details(
        int id)
    {
        var pago =
            await _service
                .ObtenerPorIdAsync(id);

        if (pago == null)
            return NotFound();

        return View(pago);
    }

    // =====================================================
    // CREATE
    // =====================================================

    [Authorize(Policy = Permisos.PagosRegistrar)]
    public async Task<IActionResult> Create(
        int idFactura)
    {
        var pagos =
            await _service
                .ObtenerPorFacturaAsync(
                    idFactura);

        var primerPago =
            pagos.FirstOrDefault();

        ViewBag.IdFactura =
            idFactura;

        ViewBag.NumeroFactura =
            primerPago?.NumeroFactura
            ?? $"Factura #{idFactura}";

        ViewBag.TotalFactura =
            primerPago?.TotalFactura ?? 0m;

        ViewBag.TotalPagado =
            primerPago?.TotalPagado ?? 0m;

        ViewBag.SaldoPendiente =
            primerPago?.SaldoPendiente ?? 0m;

        ViewBag.EstadoFactura =
            primerPago?.EstadoFactura
            ?? "Pendiente";

        return View(
            new PagoGuardarDto
            {
                IdFactura = idFactura,
                FormaPago = "Efectivo"
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.PagosRegistrar)]
    public async Task<IActionResult> Create(
        PagoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            await CargarDatosFactura(
                dto.IdFactura);

            return View(dto);
        }

        try
        {
            var usuarioId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var pago =
                await _service
                    .RegistrarAsync(
                        dto,
                        usuarioId);

            TempData["Success"] =
                $"Pago de {pago.Monto:C2} registrado correctamente.";

            return RedirectToAction(
                "Details",
                "Factura",
                new
                {
                    id = dto.IdFactura
                });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            await CargarDatosFactura(
                dto.IdFactura);

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            await CargarDatosFactura(
                dto.IdFactura);

            return View(dto);
        }
    }

    // =====================================================
    // MÉTODOS AUXILIARES
    // =====================================================

    private async Task CargarDatosFactura(
        int idFactura)
    {
        var pagos =
            await _service
                .ObtenerPorFacturaAsync(
                    idFactura);

        var pago =
            pagos.FirstOrDefault();

        ViewBag.IdFactura =
            idFactura;

        ViewBag.NumeroFactura =
            pago?.NumeroFactura
            ?? $"Factura #{idFactura}";

        ViewBag.TotalFactura =
            pago?.TotalFactura ?? 0m;

        ViewBag.TotalPagado =
            pago?.TotalPagado ?? 0m;

        ViewBag.SaldoPendiente =
            pago?.SaldoPendiente ?? 0m;

        ViewBag.EstadoFactura =
            pago?.EstadoFactura
            ?? "Pendiente";
    }
}