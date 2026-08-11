// =====================================================
// ClienteController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

// =====================================================

using Microsoft.AspNetCore.Authorization;
public class ClienteController : Controller
{
    private readonly IClienteService _service;

    public ClienteController(IClienteService service)
    {
        _service = service;
    }

    // GET: Cliente
    [Authorize(Policy = Permisos.ClientesVer)]
    public async Task<IActionResult> Index()
    {
        var clientes = await _service.ObtenerTodosAsync();

        return View(clientes);
    }

    // GET: Cliente/Activos
    [Authorize(Policy = Permisos.ClientesVer)]
    public async Task<IActionResult> Activos()
    {
        var clientes = await _service.ObtenerActivosAsync();

        return View("Index", clientes);
    }

    // GET: Cliente/Details/5
    [Authorize(Policy = Permisos.ClientesVer)]
    public async Task<IActionResult> Details(int id)
    {
        var cliente = await _service.ObtenerPorIdAsync(id);

        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    // GET: Cliente/Create
    [Authorize(Policy = Permisos.ClientesCrear)]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Cliente/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ClientesCrear)]
    public async Task<IActionResult> Create(ClienteGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.CrearAsync(dto);

            TempData["Success"] = "Cliente registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            return View(dto);
        }
    }

    // GET: Cliente/Edit/5
    [Authorize(Policy = Permisos.ClientesEditar)]
    public async Task<IActionResult> Edit(int id)
    {
        var cliente = await _service.ObtenerPorIdAsync(id);

        if (cliente == null)
            return NotFound();

        var dto = new ClienteGuardarDto
        {
            Cedula = cliente.Cedula,
            Nombre = cliente.Nombre,
            Apellido1 = cliente.Apellido1,
            Apellido2 = cliente.Apellido2,
            Telefono = cliente.Telefono,
            Correo = cliente.Correo,
            Direccion = cliente.Direccion
        };

        ViewBag.IdCliente = cliente.IdCliente;

        return View(dto);
    }

    // POST: Cliente/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.ClientesEditar)]
    public async Task<IActionResult> Edit(
        int id,
        ClienteGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdCliente = id;
            return View(dto);
        }

        try
        {
            var actualizado = await _service.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] = "Cliente actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            ViewBag.IdCliente = id;

            return View(dto);
        }
    }

    // POST: Cliente/Desactivar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Desactivar(int id)
    {
        var desactivado = await _service.DesactivarAsync(id);

        if (!desactivado)
            return NotFound();

        TempData["Success"] = "Cliente desactivado correctamente.";

        return RedirectToAction(nameof(Index));
    }

    // GET: Cliente/BuscarPorCedula
    [Authorize(Policy = Permisos.ClientesVer)]
    public async Task<IActionResult> BuscarPorCedula(string cedula)
    {
        if (string.IsNullOrWhiteSpace(cedula))
            return RedirectToAction(nameof(Index));

        var cliente = await _service.ObtenerPorCedulaAsync(cedula);

        if (cliente == null)
        {
            TempData["Error"] = "No se encontró ningún cliente con esa cédula.";
            return RedirectToAction(nameof(Index));
        }

        return View("Details", cliente);
    }
}
