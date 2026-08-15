using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

[Authorize(Policy = Permisos.UsuariosVer)]
public class UsuarioController : Controller
{
    private readonly IUsuarioService _service;

    public UsuarioController(IUsuarioService service)
    {
        _service = service;
    }

    // =====================================================
    // INDEX
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var usuarios =
            await _service.ObtenerTodosAsync();

        return View(usuarios);
    }

    // =====================================================
    // CREATE - GET
    // =====================================================

    [HttpGet]
    [Authorize(Policy = Permisos.UsuariosCrear)]
    public async Task<IActionResult> Create()
    {
        ViewBag.Roles =
            await _service.ObtenerRolesDisponiblesAsync();

        return View(
            new UsuarioGuardarDto());
    }

    // =====================================================
    // CREATE - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.UsuariosCrear)]
    public async Task<IActionResult> Create(
        UsuarioGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles =
                await _service.ObtenerRolesDisponiblesAsync();

            return View(dto);
        }

        var resultado =
            await _service.CrearAsync(dto);

        if (!resultado.Exitoso)
        {
            ModelState.AddModelError(
                string.Empty,
                resultado.Mensaje);

            ViewBag.Roles =
                await _service.ObtenerRolesDisponiblesAsync();

            return View(dto);
        }

        TempData["Success"] =
            resultado.Mensaje;

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // EDIT - GET
    // =====================================================

    [HttpGet]
    [Authorize(Policy = Permisos.UsuariosEditar)]
    public async Task<IActionResult> Edit(string id)
    {
        var usuario =
            await _service.ObtenerPorIdAsync(id);

        if (usuario == null)
            return NotFound();

        var dto = new UsuarioGuardarDto
        {
            Usuario = usuario.Usuario,
            Email = usuario.Email,
            NombreCompleto = usuario.NombreCompleto,
            Activo = usuario.Activo,
            Roles = usuario.Roles.ToList()
        };

        ViewBag.IdUsuario = usuario.Id;

        ViewBag.Roles =
            await _service.ObtenerRolesDisponiblesAsync();

        ViewBag.PuedeAdministrarRoles =
            User.HasClaim(
                "Permiso",
                Permisos.RolesAdministrar);

        return View(dto);
    }

    // =====================================================
    // EDIT - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.UsuariosEditar)]
    public async Task<IActionResult> Edit(
        string id,
        UsuarioGuardarDto dto)
    {
        var puedeAdministrarRoles =
            User.HasClaim(
                "Permiso",
                Permisos.RolesAdministrar);

        if (!ModelState.IsValid)
        {
            ViewBag.IdUsuario = id;

            ViewBag.Roles =
                await _service.ObtenerRolesDisponiblesAsync();

            ViewBag.PuedeAdministrarRoles =
                puedeAdministrarRoles;

            return View(dto);
        }

        var resultado =
            await _service.ActualizarAsync(
                id,
                dto,
                puedeAdministrarRoles);

        if (!resultado.Exitoso)
        {
            ModelState.AddModelError(
                string.Empty,
                resultado.Mensaje);

            ViewBag.IdUsuario = id;

            ViewBag.Roles =
                await _service.ObtenerRolesDisponiblesAsync();

            ViewBag.PuedeAdministrarRoles =
                puedeAdministrarRoles;

            return View(dto);
        }

        TempData["Success"] =
            resultado.Mensaje;

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // TOGGLE ACTIVO
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.UsuariosDesactivar)]
    public async Task<IActionResult> CambiarEstado(
        string id)
    {
        var resultado =
            await _service.CambiarEstadoAsync(id);

        if (!resultado.Exitoso)
        {
            TempData["Error"] =
                resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] =
            resultado.Mensaje;

        return RedirectToAction(nameof(Index));
    }

    // =====================================================
    // DETAILS
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> Details(
        string id)
    {
        var usuario =
            await _service.ObtenerPorIdAsync(id);

        if (usuario == null)
            return NotFound();

        return View(usuario);
    }
}