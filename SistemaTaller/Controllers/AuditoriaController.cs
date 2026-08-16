using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

[Authorize(Policy = Permisos.AuditoriaVer)]
public class AuditoriaController : Controller
{
    private readonly IAuditoriaService _service;

    public AuditoriaController(
        IAuditoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? usuarioId = null,
        string? modulo = null,
        string? accion = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null,
        string? registroId = null)
    {
        var registros =
            await _service.ObtenerAsync(
                usuarioId,
                modulo,
                accion,
                fechaDesde,
                fechaHasta,
                registroId);

        ViewBag.UsuarioId = usuarioId;
        ViewBag.Modulo = modulo;
        ViewBag.Accion = accion;
        ViewBag.FechaDesde =
            fechaDesde?.ToString("yyyy-MM-dd");
        ViewBag.FechaHasta =
            fechaHasta?.ToString("yyyy-MM-dd");
        ViewBag.RegistroId = registroId;

        return View(registros);
    }
}