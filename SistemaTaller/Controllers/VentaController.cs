// =====================================================
// VentaController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

public class VentaController : Controller
{
    private readonly IVentaService _service;

    public VentaController(
        IVentaService service)
    {
        _service = service;
    }

    // =====================================================
    // GET: Venta
    // =====================================================

    [Authorize(Policy = Permisos.VentasVer)]
    public async Task<IActionResult> Index()
    {
        var ventas =
            await _service.ObtenerTodosAsync();

        return View(ventas);
    }

    // =====================================================
    // GET: Venta/PorCliente/5
    // =====================================================

    [Authorize(Policy = Permisos.VentasVer)]
    public async Task<IActionResult> PorCliente(
        int idCliente)
    {
        var ventas =
            await _service.ObtenerPorClienteAsync(
                idCliente);

        return View(
            "Index",
            ventas);
    }

    // =====================================================
    // GET: Venta/PorEstado?estado=Pendiente
    // =====================================================

    [Authorize(Policy = Permisos.VentasVer)]
    public async Task<IActionResult> PorEstado(
        string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            return RedirectToAction(
                nameof(Index));
        }

        var ventas =
            await _service.ObtenerPorEstadoAsync(
                estado);

        return View(
            "Index",
            ventas);
    }

    // =====================================================
    // GET: Venta/Recientes
    // =====================================================

    [Authorize(Policy = Permisos.VentasVer)]
    public async Task<IActionResult> Recientes(
        int cantidad = 10)
    {
        if (cantidad <= 0)
            cantidad = 10;

        var ventas =
            await _service.ObtenerRecientesAsync(
                cantidad);

        return View(
            "Index",
            ventas);
    }

    // =====================================================
    // GET: Venta/Details/5
    // =====================================================

    [Authorize(Policy = Permisos.VentasVer)]
    public async Task<IActionResult> Details(
        int id)
    {
        var venta =
            await _service.ObtenerPorIdAsync(
                id);

        if (venta == null)
            return NotFound();

        return View(venta);
    }

    // =====================================================
    // GET: Venta/Create
    // =====================================================

    [Authorize(Policy = Permisos.VentasCrear)]
    public IActionResult Create()
    {
        return View();
    }

    // =====================================================
    // POST: Venta/Create
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VentasCrear)]
    public async Task<IActionResult> Create(
        VentaGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Venta registrada correctamente.";

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
    // GET: Venta/Edit/5
    // =====================================================

    [Authorize(Policy = Permisos.VentasCrear)]
    public async Task<IActionResult> Edit(
        int id)
    {
        var venta =
            await _service.ObtenerPorIdAsync(
                id);

        if (venta == null)
            return NotFound();

        var dto = new VentaGuardarDto
        {
            NumeroVenta =
                venta.NumeroVenta,

            IdCliente =
                venta.IdCliente,

            IdVendedor =
                venta.IdVendedor,

            IdCajero =
                venta.IdCajero,

            FechaVenta =
                venta.FechaVenta,

            Total =
                venta.Total,

            Estado =
                venta.Estado,

            FormaPago =
                venta.FormaPago,

            UsuarioId =
                venta.UsuarioId,

            Detalles =
                venta.Detalles
                    .Select(x =>
                        new DetalleVentaGuardarDto
                        {
                            IdProducto =
                                x.IdProducto,

                            Cantidad =
                                x.Cantidad,

                            PrecioUnitario =
                                x.PrecioUnitario,

                            Impuesto =
                                x.Impuesto,

                            Descuento =
                                x.Descuento
                        })
                    .ToList()
        };

        ViewBag.IdVenta =
            venta.IdVenta;

        return View(dto);
    }

    // =====================================================
    // POST: Venta/Edit/5
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VentasCrear)]
    public async Task<IActionResult> Edit(
        int id,
        VentaGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdVenta = id;

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
                "Venta actualizada correctamente.";

            return RedirectToAction(
                nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdVenta = id;

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdVenta = id;

            return View(dto);
        }
    }

    // =====================================================
    // POST: Venta/CambiarEstado/5
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VentasAnular)]
    public async Task<IActionResult> CambiarEstado(
        int id,
        string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            TempData["Error"] =
                "El estado de la venta es obligatorio.";

            return RedirectToAction(
                nameof(Index));
        }

        try
        {
            var actualizado =
                await _service.CambiarEstadoAsync(
                    id,
                    estado);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Estado de la venta actualizado correctamente.";

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
        catch (ArgumentException ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                nameof(Index));
        }
    }
}

