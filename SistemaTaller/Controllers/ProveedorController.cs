using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace SistemaTaller.Controllers;

public class ProveedorController : Controller
{
    private readonly IProveedorService _service;

    public ProveedorController(IProveedorService service)
    {
        _service = service;
    }

    // GET: Proveedor
    public async Task<IActionResult> Index()
    {
        var proveedores = await _service.ObtenerTodosAsync();

        return View(proveedores);
    }

    // GET: Proveedor/Activos
    public async Task<IActionResult> Activos()
    {
        var proveedores = await _service.ObtenerActivosAsync();

        return View("Index", proveedores);
    }

    // GET: Proveedor/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var proveedor = await _service.ObtenerPorIdAsync(id);

        if (proveedor == null)
            return NotFound();

        return View(proveedor);
    }

    // GET: Proveedor/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Proveedor/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ProveedorGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Proveedor registrado correctamente.";

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

    // GET: Proveedor/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var proveedor = await _service.ObtenerPorIdAsync(id);

        if (proveedor == null)
            return NotFound();

        var dto = new ProveedorGuardarDto
        {
            Nombre = proveedor.Nombre,
            CedulaJuridica = proveedor.CedulaJuridica,
            Telefono = proveedor.Telefono,
            Correo = proveedor.Correo,
            Direccion = proveedor.Direccion,
            Activo = proveedor.Activo
        };

        ViewBag.IdProveedor = proveedor.IdProveedor;

        return View(dto);
    }

    // POST: Proveedor/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        ProveedorGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdProveedor = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Proveedor actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdProveedor = id;

            return View(dto);
        }
    }

    // POST: Proveedor/CambiarEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(
        int id,
        bool activo)
    {
        var actualizado =
            await _service.CambiarEstadoAsync(id, activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Proveedor activado correctamente."
                : "Proveedor desactivado correctamente.";

        return RedirectToAction(nameof(Index));
    }
}