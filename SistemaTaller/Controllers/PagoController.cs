using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;
using System.Security.Claims;

namespace SistemaTaller.Controllers;

[Authorize]
public class PagoController : Controller
{
    private readonly IPagoService _service;
    private readonly IFacturaService _facturaService;

    public PagoController(
        IPagoService service,
        IFacturaService facturaService)
    {
        _service = service;
        _facturaService = facturaService;
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
    // CREATE GET
    // =====================================================

    [Authorize(Policy = Permisos.PagosRegistrar)]
    public async Task<IActionResult> Create(
        int idFactura)
    {
        var factura =
            await _facturaService
                .ObtenerPorIdAsync(idFactura);

        if (factura == null)
            return NotFound();

        if (factura.Estado == "Anulada")
        {
            TempData["Error"] =
                "No se puede registrar un pago sobre una factura anulada.";

            return RedirectToAction(
                "Details",
                "Factura",
                new { id = idFactura });
        }

        if (factura.SaldoPendiente <= 0)
        {
            TempData["Error"] =
                "La factura no tiene saldo pendiente.";

            return RedirectToAction(
                "Details",
                "Factura",
                new { id = idFactura });
        }

        CargarDatosFactura(factura);

        return View(
            new PagoGuardarDto
            {
                IdFactura = idFactura,
                FormaPago = "Efectivo",
                Monto = factura.SaldoPendiente
            });
    }

    // =====================================================
    // CREATE POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.PagosRegistrar)]
    public async Task<IActionResult> Create(
        PagoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            await CargarDatosFactura(dto.IdFactura);

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
    // MÉTODO AUXILIAR
    // =====================================================

    private async Task CargarDatosFactura(
        int idFactura)
    {
        var factura =
            await _facturaService
                .ObtenerPorIdAsync(idFactura);

        if (factura == null)
            return;

        CargarDatosFactura(factura);
    }

    private void CargarDatosFactura(
        FacturaDto factura)
    {
        ViewBag.IdFactura =
            factura.IdFactura;

        ViewBag.NumeroFactura =
            factura.NumeroFactura;

        ViewBag.Cliente =
            factura.Cliente;

        ViewBag.TotalFactura =
            factura.Total;

        ViewBag.TotalPagado =
            factura.TotalPagado;

        ViewBag.SaldoPendiente =
            factura.SaldoPendiente;

        ViewBag.EstadoFactura =
            factura.Estado;
    }
}