using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

[Authorize]
public class VehiculoController : Controller
{
    private readonly IVehiculoService _service;
    private readonly IClienteService _clienteService;
    private readonly IMarcaService _marcaService;
    private readonly IModeloService _modeloService;
    private readonly ITipoVehiculoService _tipoVehiculoService;
    private readonly ITipoCombustibleService _tipoCombustibleService;

    public VehiculoController(
        IVehiculoService service,
        IClienteService clienteService,
        IMarcaService marcaService,
        IModeloService modeloService,
        ITipoVehiculoService tipoVehiculoService,
        ITipoCombustibleService tipoCombustibleService)
    {
        _service = service;
        _clienteService = clienteService;
        _marcaService = marcaService;
        _modeloService = modeloService;
        _tipoVehiculoService = tipoVehiculoService;
        _tipoCombustibleService = tipoCombustibleService;
    }

    // =====================================================
    // CONSULTAR VEHÍCULOS
    // =====================================================

    // GET: Vehiculo
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> Index()
    {
        var vehiculos =
            await _service.ObtenerTodosAsync();

        return View(vehiculos);
    }

    // GET: Vehiculo/Activos
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> Activos()
    {
        var vehiculos =
            await _service.ObtenerActivosAsync();

        return View("Index", vehiculos);
    }

    // GET: Vehiculo/PorCliente/5
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> PorCliente(
        int idCliente)
    {
        var vehiculos =
            await _service.ObtenerPorClienteAsync(
                idCliente);

        return View("Index", vehiculos);
    }

    // GET: Vehiculo/Details/5
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> Details(int id)
    {
        var vehiculo =
            await _service.ObtenerPorIdAsync(id);

        if (vehiculo == null)
            return NotFound();

        return View(vehiculo);
    }

    // GET: Vehiculo/BuscarPorPlaca
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> BuscarPorPlaca(
        string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            return RedirectToAction(nameof(Index));

        var vehiculo =
            await _service.ObtenerPorPlacaAsync(
                placa);

        if (vehiculo == null)
        {
            TempData["Error"] =
                "No se encontró ningún vehículo con esa placa.";

            return RedirectToAction(nameof(Index));
        }

        return View("Details", vehiculo);
    }

    // =====================================================
    // CARGAR CATÁLOGOS PARA FORMULARIOS
    // =====================================================

    private async Task CargarClasificacionesAsync()
    {
        var clientes =
            await _clienteService.ObtenerActivosAsync();

        var marcas =
            await _marcaService.ObtenerActivasAsync();

        var modelos =
            await _modeloService.ObtenerActivasAsync();

        var tiposVehiculo =
            await _tipoVehiculoService.ObtenerActivasAsync();

        var tiposCombustible =
            await _tipoCombustibleService.ObtenerActivasAsync();

        ViewBag.Clientes = clientes
            .Select(x => new SelectListItem
            {
                Value = x.IdCliente.ToString(),
                Text =
                    $"{x.IdCliente} — {x.Nombre} {x.Apellido1} {x.Apellido2}"
                    .Trim()
            })
            .ToList();

        ViewBag.Marcas = marcas
            .Select(x => new SelectListItem
            {
                Value = x.IdMarca.ToString(),
                Text = x.Nombre
            })
            .ToList();

        ViewBag.Modelos = modelos
            .Select(x => new SelectListItem
            {
                Value = x.IdModelo.ToString(),
                Text = x.Nombre
            })
            .ToList();

        ViewBag.TiposVehiculo = tiposVehiculo
            .Select(x => new SelectListItem
            {
                Value = x.IdTipoVehiculo.ToString(),
                Text = x.Nombre
            })
            .ToList();

        ViewBag.TiposCombustible = tiposCombustible
            .Select(x => new SelectListItem
            {
                Value = x.IdTipoCombustible.ToString(),
                Text = x.Nombre
            })
            .ToList();
    }

    // =====================================================
    // CREAR VEHÍCULO
    // =====================================================

    // GET: Vehiculo/Create
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public async Task<IActionResult> Create()
    {
        await CargarClasificacionesAsync();

        return View();
    }

    // POST: Vehiculo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public async Task<IActionResult> Create(
        VehiculoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            await CargarClasificacionesAsync();

            return View(dto);
        }

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

            await CargarClasificacionesAsync();

            return View(dto);
        }
    }

    // =====================================================
    // EDITAR VEHÍCULO
    // =====================================================

    // GET: Vehiculo/Edit/5
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> Edit(int id)
    {
        var vehiculo =
            await _service.ObtenerPorIdAsync(id);

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
            VIN = vehiculo.VIN,
            Anio = vehiculo.Anio,
            Color = vehiculo.Color,
            Kilometraje = vehiculo.Kilometraje
        };

        ViewBag.IdVehiculo =
            vehiculo.IdVehiculo;

        await CargarClasificacionesAsync();

        return View(dto);
    }

    // POST: Vehiculo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> Edit(
        int id,
        VehiculoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdVehiculo = id;

            await CargarClasificacionesAsync();

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
                "Vehículo actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdVehiculo = id;

            await CargarClasificacionesAsync();

            return View(dto);
        }
    }

    // =====================================================
    // DESACTIVAR VEHÍCULO
    // =====================================================

    // POST: Vehiculo/Desactivar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosDesactivar)]
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