using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.Interfaces;
using System.Security.Claims;

namespace SistemaTaller.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _service;

    public DashboardController(
        IDashboardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var rol =
            User.FindFirstValue(
                ClaimTypes.Role)
            ?? "Usuario";

        var dashboard =
            await _service.ObtenerDashboardAsync(rol);

        return View(dashboard);
    }
}