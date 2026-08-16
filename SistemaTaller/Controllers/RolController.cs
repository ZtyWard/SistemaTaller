using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

[Authorize(Policy = Permisos.RolesVer)]
public class RolController : Controller
{
    private readonly IRolService _service;

    public RolController(
        IRolService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roles =
            await _service.ObtenerTodosAsync();

        return View(roles);
    }

    [HttpGet]
    [Authorize(Policy = Permisos.RolesAdministrar)]
    public async Task<IActionResult> Create()
    {
        var dto = new RolAdministracionDto
        {
            PermisosDisponibles =
                (await _service
                    .ObtenerPermisosDisponiblesAsync())
                .ToList()
        };

        ViewBag.PuedeAdministrarPermisos =
            User.HasClaim(
                "Permiso",
                Permisos.PermisosAdministrar);

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.RolesAdministrar)]
    public async Task<IActionResult> Create(
        RolAdministracionDto dto)
    {
        var puedeAdministrarPermisos =
            User.HasClaim(
                "Permiso",
                Permisos.PermisosAdministrar);

        if (!ModelState.IsValid)
        {
            dto.PermisosDisponibles =
                (await _service
                    .ObtenerPermisosDisponiblesAsync())
                .ToList();

            ViewBag.PuedeAdministrarPermisos =
                puedeAdministrarPermisos;

            return View(dto);
        }

        var resultado =
            await _service.CrearAsync(
                dto,
                puedeAdministrarPermisos);

        if (!resultado.Exitoso)
        {
            ModelState.AddModelError(
                string.Empty,
                resultado.Mensaje);

            dto.PermisosDisponibles =
                (await _service
                    .ObtenerPermisosDisponiblesAsync())
                .ToList();

            ViewBag.PuedeAdministrarPermisos =
                puedeAdministrarPermisos;

            return View(dto);
        }

        TempData["Success"] =
            resultado.Mensaje;

        return RedirectToAction(
            nameof(Index));
    }

    [HttpGet]
    [Authorize(Policy = Permisos.RolesAdministrar)]
    public async Task<IActionResult> Edit(
        string id)
    {
        var dto =
            await _service.ObtenerPorIdAsync(id);

        if (dto == null)
            return NotFound();

        ViewBag.PuedeAdministrarPermisos =
            User.HasClaim(
                "Permiso",
                Permisos.PermisosAdministrar);

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.RolesAdministrar)]
    public async Task<IActionResult> Edit(
        RolAdministracionDto dto)
    {
        var puedeAdministrarPermisos =
            User.HasClaim(
                "Permiso",
                Permisos.PermisosAdministrar);

        if (!ModelState.IsValid)
        {
            var existente =
                await _service
                    .ObtenerPorIdAsync(dto.Id);

            dto.PermisosDisponibles =
                existente?
                    .PermisosDisponibles
                ?? (await _service
                    .ObtenerPermisosDisponiblesAsync())
                    .ToList();

            ViewBag.PuedeAdministrarPermisos =
                puedeAdministrarPermisos;

            return View(dto);
        }

        var resultado =
            await _service.ActualizarAsync(
                dto,
                puedeAdministrarPermisos);

        if (!resultado.Exitoso)
        {
            ModelState.AddModelError(
                string.Empty,
                resultado.Mensaje);

            var existente =
                await _service
                    .ObtenerPorIdAsync(dto.Id);

            dto.PermisosDisponibles =
                existente?
                    .PermisosDisponibles
                ?? (await _service
                    .ObtenerPermisosDisponiblesAsync())
                    .ToList();

            ViewBag.PuedeAdministrarPermisos =
                puedeAdministrarPermisos;

            return View(dto);
        }

        TempData["Success"] =
            resultado.Mensaje;

        return RedirectToAction(
            nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.RolesAdministrar)]
    public async Task<IActionResult> Delete(
        string id)
    {
        var resultado =
            await _service.EliminarAsync(id);

        if (!resultado.Exitoso)
        {
            TempData["Error"] =
                resultado.Mensaje;

            return RedirectToAction(
                nameof(Index));
        }

        TempData["Success"] =
            resultado.Mensaje;

        return RedirectToAction(
            nameof(Index));
    }
}