// =====================================================
// EmpleadoController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

// =====================================================

using Microsoft.AspNetCore.Authorization;
public class EmpleadoController : Controller
{
    private readonly IEmpleadoService _service;

    public EmpleadoController(IEmpleadoService service)
    {
        _service = service;
    }

    // GET: Empleado
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> Index()
    {
        var empleados = await _service.ObtenerTodosAsync();

        return View(empleados);
    }

    // GET: Empleado/Activos
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> Activos()
    {
        var empleados = await _service.ObtenerActivosAsync();

        return View("Index", empleados);
    }

    // GET: Empleado/PorPuesto/5
    public async Task<IActionResult> PorPuesto(int idPuesto)
    {
        var empleados =
            await _service.ObtenerPorPuestoAsync(idPuesto);

        return View("Index", empleados);
    }

    // GET: Empleado/PorEspecialidad/5
    public async Task<IActionResult> PorEspecialidad(
        int idEspecialidad)
    {
        var empleados =
            await _service.ObtenerPorEspecialidadAsync(
                idEspecialidad);

        return View("Index", empleados);
    }

    // GET: Empleado/Details/5
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> Details(int id)
    {
        var empleado = await _service.ObtenerPorIdAsync(id);

        if (empleado == null)
            return NotFound();

        return View(empleado);
    }

    // GET: Empleado/Create
    [Authorize(Policy = Permisos.EmpleadosCrear)]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Empleado/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosCrear)]
    public async Task<IActionResult> Create(
        EmpleadoGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] =
                "Empleado registrado correctamente.";

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

    // GET: Empleado/Edit/5
    [Authorize(Policy = Permisos.EmpleadosEditar)]
    public async Task<IActionResult> Edit(int id)
    {
        var empleado = await _service.ObtenerPorIdAsync(id);

        if (empleado == null)
            return NotFound();

        var partesNombre =
            empleado.NombreCompleto.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

        var dto = new EmpleadoGuardarDto
        {
            Cedula = empleado.Cedula,
            Telefono = empleado.Telefono,
            Correo = empleado.Correo,
            IdPuesto = empleado.IdPuesto,
            IdEspecialidad = empleado.IdEspecialidad,
            Salario = empleado.Salario,
            Activo = empleado.Activo
        };

        if (partesNombre.Length > 0)
            dto.Nombre = partesNombre[0];

        if (partesNombre.Length > 1)
            dto.Apellido1 = partesNombre[1];

        if (partesNombre.Length > 2)
            dto.Apellido2 = string.Join(
                " ",
                partesNombre.Skip(2));

        ViewBag.IdEmpleado = empleado.IdEmpleado;

        return View(dto);
    }

    // POST: Empleado/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosEditar)]
    public async Task<IActionResult> Edit(
        int id,
        EmpleadoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdEmpleado = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Empleado actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdEmpleado = id;

            return View(dto);
        }
    }

    // POST: Empleado/CambiarEstado/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosDesactivar)]
    public async Task<IActionResult> CambiarEstado(
        int id,
        bool activo)
    {
        var actualizado =
            await _service.CambiarEstadoAsync(
                id,
                activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Empleado activado correctamente."
                : "Empleado desactivado correctamente.";

        return RedirectToAction(nameof(Index));
    }
}

