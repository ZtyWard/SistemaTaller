using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

// =====================================================

using Microsoft.AspNetCore.Authorization;
public class CatalogoPersonalController : Controller
{
    private readonly IPuestoService _puestoService;
    private readonly IEspecialidadService _especialidadService;

    public CatalogoPersonalController(
        IPuestoService puestoService,
        IEspecialidadService especialidadService)
    {
        _puestoService = puestoService;
        _especialidadService = especialidadService;
    }

    // =====================================================
    // PUESTOS
    // =====================================================

    // GET: CatalogoPersonal/Puestos
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> Puestos()
    {
        var puestos = await _puestoService.ObtenerTodosAsync();

        return View(puestos);
    }

    // GET: CatalogoPersonal/PuestosActivos
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> PuestosActivos()
    {
        var puestos = await _puestoService.ObtenerActivosAsync();

        return View("Puestos", puestos);
    }

    // GET: CatalogoPersonal/DetallePuesto/5
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> DetallePuesto(int id)
    {
        var puesto = await _puestoService.ObtenerPorIdAsync(id);

        if (puesto == null)
            return NotFound();

        return View(puesto);
    }

    // GET: CatalogoPersonal/CrearPuesto
    [Authorize(Policy = Permisos.EmpleadosCrear)]
    public IActionResult CrearPuesto()
    {
        return View();
    }

    // POST: CatalogoPersonal/CrearPuesto
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosCrear)]
    public async Task<IActionResult> CrearPuesto(
        PuestoGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _puestoService.CrearAsync(dto);

            TempData["Success"] =
                "Puesto creado correctamente.";

            return RedirectToAction(nameof(Puestos));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }

    // GET: CatalogoPersonal/EditarPuesto/5
    [Authorize(Policy = Permisos.EmpleadosEditar)]
    public async Task<IActionResult> EditarPuesto(int id)
    {
        var puesto = await _puestoService.ObtenerPorIdAsync(id);

        if (puesto == null)
            return NotFound();

        var dto = new PuestoGuardarDto
        {
            Nombre = puesto.Nombre,
            Activo = puesto.Activo
        };

        ViewBag.IdPuesto = puesto.IdPuesto;

        return View(dto);
    }

    // POST: CatalogoPersonal/EditarPuesto/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosEditar)]
    public async Task<IActionResult> EditarPuesto(
        int id,
        PuestoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdPuesto = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _puestoService.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Puesto actualizado correctamente.";

            return RedirectToAction(nameof(Puestos));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdPuesto = id;

            return View(dto);
        }
    }

    // POST: CatalogoPersonal/CambiarEstadoPuesto/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosDesactivar)]
    public async Task<IActionResult> CambiarEstadoPuesto(
        int id,
        bool activo)
    {
        var actualizado =
            await _puestoService.CambiarEstadoAsync(id, activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Puesto activado correctamente."
                : "Puesto desactivado correctamente.";

        return RedirectToAction(nameof(Puestos));
    }

    // =====================================================
    // ESPECIALIDADES
    // =====================================================

    // GET: CatalogoPersonal/Especialidades
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> Especialidades()
    {
        var especialidades =
            await _especialidadService.ObtenerTodosAsync();

        return View(especialidades);
    }

    // GET: CatalogoPersonal/EspecialidadesActivas
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> EspecialidadesActivas()
    {
        var especialidades =
            await _especialidadService.ObtenerActivasAsync();

        return View("Especialidades", especialidades);
    }

    // GET: CatalogoPersonal/DetalleEspecialidad/5
    [Authorize(Policy = Permisos.EmpleadosVer)]
    public async Task<IActionResult> DetalleEspecialidad(int id)
    {
        var especialidad =
            await _especialidadService.ObtenerPorIdAsync(id);

        if (especialidad == null)
            return NotFound();

        return View(especialidad);
    }

    // GET: CatalogoPersonal/CrearEspecialidad
    [Authorize(Policy = Permisos.EmpleadosCrear)]
    public IActionResult CrearEspecialidad()
    {
        return View();
    }

    // POST: CatalogoPersonal/CrearEspecialidad
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosCrear)]
    public async Task<IActionResult> CrearEspecialidad(
        EspecialidadGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _especialidadService.CrearAsync(dto);

            TempData["Success"] =
                "Especialidad creada correctamente.";

            return RedirectToAction(nameof(Especialidades));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }

    // GET: CatalogoPersonal/EditarEspecialidad/5
    [Authorize(Policy = Permisos.EmpleadosEditar)]
    public async Task<IActionResult> EditarEspecialidad(int id)
    {
        var especialidad =
            await _especialidadService.ObtenerPorIdAsync(id);

        if (especialidad == null)
            return NotFound();

        var dto = new EspecialidadGuardarDto
        {
            Nombre = especialidad.Nombre,
            Activo = especialidad.Activo
        };

        ViewBag.IdEspecialidad = especialidad.IdEspecialidad;

        return View(dto);
    }

    // POST: CatalogoPersonal/EditarEspecialidad/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosEditar)]
    public async Task<IActionResult> EditarEspecialidad(
        int id,
        EspecialidadGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdEspecialidad = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _especialidadService.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Especialidad actualizada correctamente.";

            return RedirectToAction(nameof(Especialidades));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdEspecialidad = id;

            return View(dto);
        }
    }

    // POST: CatalogoPersonal/CambiarEstadoEspecialidad/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.EmpleadosDesactivar)]
    public async Task<IActionResult> CambiarEstadoEspecialidad(
        int id,
        bool activo)
    {
        var actualizado =
            await _especialidadService.CambiarEstadoAsync(
                id,
                activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Especialidad activada correctamente."
                : "Especialidad desactivada correctamente.";

        return RedirectToAction(nameof(Especialidades));
    }
}