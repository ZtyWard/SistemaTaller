using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace SistemaTaller.Controllers;

public class DiagnosticoController : Controller
{
    private readonly IDiagnosticoService _service;

    public DiagnosticoController(IDiagnosticoService service)
    {
        _service = service;
    }

    // GET: Diagnostico
    public async Task<IActionResult> Index()
    {
        var diagnosticos = await _service.ObtenerTodosAsync();

        return View(diagnosticos);
    }

    // GET: Diagnostico/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var diagnostico = await _service.ObtenerPorIdAsync(id);

        if (diagnostico == null)
            return NotFound();

        return View(diagnostico);
    }

    // GET: Diagnostico/PorRecepcion/5
    public async Task<IActionResult> PorRecepcion(int idRecepcion)
    {
        var diagnosticos =
            await _service.ObtenerPorRecepcionAsync(idRecepcion);

        return View("Index", diagnosticos);
    }

    // GET: Diagnostico/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Diagnostico/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        DiagnosticoGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Diagnóstico registrado correctamente.";

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

    // GET: Diagnostico/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var diagnostico = await _service.ObtenerPorIdAsync(id);

        if (diagnostico == null)
            return NotFound();

        var dto = new DiagnosticoGuardarDto
        {
            IdRecepcion = diagnostico.IdRecepcion,
            IdEmpleado = diagnostico.IdEmpleado,
            Descripcion = diagnostico.Descripcion
        };

        ViewBag.IdDiagnostico = diagnostico.IdDiagnostico;

        return View(dto);
    }

    // POST: Diagnostico/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        DiagnosticoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdDiagnostico = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Diagnóstico actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdDiagnostico = id;

            return View(dto);
        }
    }
}