using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace SistemaTaller.Controllers;

[Authorize(Roles = "Administrador")]
public class ConfiguracionController : Controller
{
    private readonly IConfiguracionGeneralService _service;

    public ConfiguracionController(
        IConfiguracionGeneralService service)
    {
        _service = service;
    }

    // GET: /Configuracion
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var configuracion =
            await _service.ObtenerAsync();

        if (configuracion == null)
        {
            configuracion = new ConfiguracionGeneralDto
            {
                NombreTaller = "AXIS - Taller Automotriz",
                Moneda = "CRC",
                ImpuestoPorcentaje = 13,
                LimiteDescuentoPorcentaje = 0,
                PrefijoRecepcion = "REC",
                SiguienteRecepcion = 1,
                PrefijoCotizacion = "COT",
                SiguienteCotizacion = 1,
                PrefijoOrdenTrabajo = "OT",
                SiguienteOrdenTrabajo = 1,
                PrefijoVenta = "VEN",
                SiguienteVenta = 1,
                PrefijoFactura = "FAC",
                SiguienteFactura = 1,
                HoraApertura = new TimeSpan(8, 0, 0),
                HoraCierre = new TimeSpan(17, 0, 0),
                DiasGarantia = 30,
                ExistenciaMinimaPredeterminada = 1,
                EstadosProceso =
                    "Pendiente,En proceso,Completado,Cancelado"
            };
        }

        return View(configuracion);
    }

    // POST: /Configuracion
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        ConfiguracionGeneralDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _service.GuardarAsync(dto);

            TempData["Success"] =
                "La configuración general fue guardada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }
}