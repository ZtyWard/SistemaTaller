using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

[Authorize(Policy = Permisos.CitasVer)]
public class CitaController : Controller
{
    private readonly ICitaService _citaService;
    private readonly IClienteService _clienteService;
    private readonly IVehiculoService _vehiculoService;
    private readonly IServicioService _servicioService;
    private readonly IEmpleadoService _empleadoService;

    private static readonly string[] Estados =
    {
        "Programada",
        "Confirmada",
        "En espera",
        "Atendida",
        "Cancelada",
        "No asistió"
    };

    public CitaController(
        ICitaService citaService,
        IClienteService clienteService,
        IVehiculoService vehiculoService,
        IServicioService servicioService,
        IEmpleadoService empleadoService)
    {
        _citaService = citaService;
        _clienteService = clienteService;
        _vehiculoService = vehiculoService;
        _servicioService = servicioService;
        _empleadoService = empleadoService;
    }

    // =====================================================
    // INDEX / AGENDA
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> Index(
        DateTime? fecha)
    {
        var fechaSeleccionada =
            fecha?.Date ?? DateTime.Today;

        var inicio =
            fechaSeleccionada.Date;

        var fin =
            fechaSeleccionada.Date.AddDays(1).AddTicks(-1);

        var citas =
            await _citaService.ObtenerAgendaAsync(
                inicio,
                fin);

        ViewBag.FechaSeleccionada =
            fechaSeleccionada;

        return View(citas);
    }

    // =====================================================
    // CREATE - GET
    // =====================================================

    [HttpGet]
    [Authorize(Policy = Permisos.CitasCrear)]
    public async Task<IActionResult> Create()
    {
        await CargarCombosAsync();

        var ahora = DateTime.Now;

        var dto = new CitaFormularioDto
        {
            FechaInicio =
                new DateTime(
                    ahora.Year,
                    ahora.Month,
                    ahora.Day,
                    ahora.Hour,
                    ahora.Minute,
                    0),

            FechaFin =
                new DateTime(
                    ahora.Year,
                    ahora.Month,
                    ahora.Day,
                    ahora.Hour,
                    ahora.Minute,
                    0)
                .AddHours(1),

            Estado = "Programada"
        };

        return View(dto);
    }

    // =====================================================
    // CREATE - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.CitasCrear)]
    public async Task<IActionResult> Create(
        CitaFormularioDto dto)
    {
        if (!ModelState.IsValid)
        {
            await CargarCombosAsync();
            return View(dto);
        }

        var resultado =
            await _citaService.CrearAsync(dto);

        if (!resultado.Exitoso)
        {
            ModelState.AddModelError(
                string.Empty,
                resultado.Mensaje);

            await CargarCombosAsync();

            return View(dto);
        }

        TempData["Success"] =
            resultado.Mensaje;

        return RedirectToAction(
            nameof(Index),
            new
            {
                fecha = dto.FechaInicio.Date
            });
    }

    // =====================================================
    // EDIT - GET
    // =====================================================

    [HttpGet]
    [Authorize(Policy = Permisos.CitasEditar)]
    public async Task<IActionResult> Edit(
        int id)
    {
        var cita =
            await _citaService.ObtenerPorIdAsync(id);

        if (cita == null)
            return NotFound();

        var dto = new CitaFormularioDto
        {
            IdCliente = cita.IdCliente,
            IdVehiculo = cita.IdVehiculo,
            IdServicio = cita.IdServicio,
            IdEmpleado = cita.IdEmpleado,
            Area = cita.Area,
            FechaInicio = cita.FechaInicio,
            FechaFin = cita.FechaFin,
            Estado = cita.Estado,
            Observaciones = cita.Observaciones
        };

        ViewBag.IdCita =
            cita.IdCita;

        await CargarCombosAsync();

        return View(dto);
    }

    // =====================================================
    // EDIT - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.CitasEditar)]
    public async Task<IActionResult> Edit(
        int id,
        CitaFormularioDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdCita = id;

            await CargarCombosAsync();

            return View(dto);
        }

        var resultado =
            await _citaService.ActualizarAsync(
                id,
                dto);

        if (!resultado.Exitoso)
        {
            ModelState.AddModelError(
                string.Empty,
                resultado.Mensaje);

            ViewBag.IdCita = id;

            await CargarCombosAsync();

            return View(dto);
        }

        TempData["Success"] =
            resultado.Mensaje;

        return RedirectToAction(
            nameof(Index),
            new
            {
                fecha = dto.FechaInicio.Date
            });
    }

    // =====================================================
    // CANCELAR
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.CitasCancelar)]
    public async Task<IActionResult> Cancelar(
        int id)
    {
        var resultado =
            await _citaService.CancelarAsync(id);

        if (!resultado.Exitoso)
        {
            TempData["Error"] =
                resultado.Mensaje;
        }
        else
        {
            TempData["Success"] =
                resultado.Mensaje;
        }

        return RedirectToAction(
            nameof(Index));
    }

    // =====================================================
    // CARGAR COMBOS
    // =====================================================

    private async Task CargarCombosAsync()
    {
        var clientes =
            await _clienteService.ObtenerActivosAsync();

        var vehiculos =
            await _vehiculoService.ObtenerActivosAsync();

        var servicios =
            await _servicioService.ObtenerActivosAsync();

        var empleados =
            await _empleadoService.ObtenerActivosAsync();

        ViewBag.Clientes =
            clientes
                .OrderBy(x => x.Nombre)
                .ThenBy(x => x.Apellido1)
                .ToList();

        ViewBag.Vehiculos =
            vehiculos
                .OrderBy(x => x.Placa)
                .ToList();

        ViewBag.Servicios =
            servicios
                .OrderBy(x => x.Nombre)
                .ToList();

        ViewBag.Empleados =
            empleados
                .OrderBy(x => x.NombreCompleto)
                .ToList();

        ViewBag.Estados =
            Estados;
    }
}