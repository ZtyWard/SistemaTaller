using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.Interfaces;

namespace SistemaTaller.Controllers;

[Authorize]
public class NotificacionController : Controller
{
    private readonly INotificacionService _service;

    public NotificacionController(
        INotificacionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var notificaciones =
            await _service.ObtenerTodasAsync();

        return View(notificaciones);
    }

    [HttpGet]
    public async Task<IActionResult> Resumen()
    {
        var notificaciones =
            await _service.ObtenerTodasAsync();

        return Json(new
        {
            total = notificaciones.Count(),
            items = notificaciones
        });
    }
}