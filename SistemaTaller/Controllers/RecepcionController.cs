using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace SistemaTaller.Controllers;

public class RecepcionController : Controller
{
    private readonly IRecepcionService _service;

    public RecepcionController(IRecepcionService service)
    {
        _service = service;
    }

    // GET: Recepcion
    public async Task<IActionResult> Index()
    {
        var recepciones = await _service.ObtenerTodasAsync();

        return View(recepciones);
    }

    // GET: Recepcion/Abiertas
    public async Task<IActionResult> Abiertas()
    {
        var recepciones = await _service.ObtenerAbiertasAsync();

        return View("Index", recepciones);
    }

    // GET: Recepcion/PorVehiculo/5
    public async Task<IActionResult> PorVehiculo(int idVehiculo)
    {
        var recepciones =
            await _service.ObtenerPorVehiculoAsync(idVehiculo);

        return View("Index", recepciones);
    }

    // GET: Recepcion/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var recepcion = await _service.ObtenerPorIdAsync(id);

        if (recepcion == null)
            return NotFound();

        return View(recepcion);
    }

    // GET: Recepcion/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Recepcion/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        RecepcionGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Recepción registrada correctamente.";

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

    // POST: Recepcion/ActualizarEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarEstado(
        int id,
        string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            TempData["Error"] =
                "El estado de la recepción es obligatorio.";

            return RedirectToAction(nameof(Index));
        }

        var actualizado =
            await _service.ActualizarEstadoAsync(id, estado);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            "Estado de la recepción actualizado correctamente.";

        return RedirectToAction(nameof(Index));
    }
}