using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

[Authorize(Policy = Permisos.ReportesVer)]
public class ReportesController : Controller
{
    private readonly IReportesService _service;

    public ReportesController(
        IReportesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        DateTime? desde,
        DateTime? hasta,
        string? placa)
    {
        try
        {
            var modelo =
                await _service.ObtenerAsync(
                    desde,
                    hasta,
                    placa);

            return View(modelo);
        }
        catch (ArgumentException ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                nameof(Index));
        }
    }
}