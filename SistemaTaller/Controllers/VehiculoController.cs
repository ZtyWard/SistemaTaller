using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace SistemaTaller.Controllers;

public class VehiculoController : Controller
{
    private readonly IVehiculoService _service;

    public VehiculoController(IVehiculoService service)
    {
        _service = service;
    }

    // GET: Vehiculo
    public async Task<IActionResult> Index()
    {
        var vehiculos = await _service.ObtenerTodosAsync();

        return View(vehiculos);
    }

    // GET: Vehiculo/Activos
    public async Task<IActionResult> Activos()
    {
        var vehiculos = await _service.ObtenerActivosAsync();

        return View("Index", vehiculos);
    }

    // GET: Vehiculo/PorCliente/5
    public async Task<IActionResult> PorCliente(int idCliente)
    {
        var vehiculos = await _service.ObtenerPorClienteAsync(idCliente);

        return View("Index", vehiculos);
    }

    // GET: Vehiculo/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var vehiculo = await _service.ObtenerPorIdAsync(id);

        if (vehiculo == null)
            return NotFound();

        return View(vehiculo);
    }

    // GET: Vehiculo/BuscarPorPlaca
    public async Task<IActionResult> BuscarPorPlaca(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            return RedirectToAction(nameof(Index));

        var vehiculo = await _service.ObtenerPorPlacaAsync(placa);

        if (vehiculo == null)
        {
            TempData["Error"] = "No se encontró ningún vehículo con esa placa.";

            return RedirectToAction(nameof(Index));
        }

        return View("Details", vehiculo);
    }

    // GET: Vehiculo/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Vehiculo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VehiculoGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Vehículo registrado correctamente.";

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

    // GET: Vehiculo/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var vehiculo = await _service.ObtenerPorIdAsync(id);

        if (vehiculo == null)
            return NotFound();

        var dto = new VehiculoGuardarDto
        {
            IdCliente = vehiculo.IdCliente,
            IdMarca = vehiculo.IdMarca,
            IdModelo = vehiculo.IdModelo,
            IdTipoVehiculo = vehiculo.IdTipoVehiculo,
            IdTipoCombustible = vehiculo.IdTipoCombustible,
            Placa = vehiculo.Placa,
            Anio = vehiculo.Anio,
            Color = vehiculo.Color,
            Kilometraje = vehiculo.Kilometraje
        };

        ViewBag.IdVehiculo = vehiculo.IdVehiculo;

        return View(dto);
    }

    // POST: Vehiculo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        VehiculoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdVehiculo = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Vehículo actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdVehiculo = id;

            return View(dto);
        }
    }

    // POST: Vehiculo/Desactivar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desactivar(int id)
    {
        var desactivado =
            await _service.DesactivarAsync(id);

        if (!desactivado)
            return NotFound();

        TempData["Success"] =
            "Vehículo desactivado correctamente.";

        return RedirectToAction(nameof(Index));
    }
}