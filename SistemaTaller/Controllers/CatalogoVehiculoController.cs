// =====================================================
// CatalogoVehiculoController
// =====================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;

public class CatalogoVehiculoController : Controller
{
    private readonly IMarcaService _marcaService;
    private readonly IModeloService _modeloService;
    private readonly ITipoVehiculoService _tipoVehiculoService;
    private readonly ITipoCombustibleService _tipoCombustibleService;

    public CatalogoVehiculoController(
        IMarcaService marcaService,
        IModeloService modeloService,
        ITipoVehiculoService tipoVehiculoService,
        ITipoCombustibleService tipoCombustibleService)
    {
        _marcaService = marcaService;
        _modeloService = modeloService;
        _tipoVehiculoService = tipoVehiculoService;
        _tipoCombustibleService = tipoCombustibleService;
    }

    // =====================================================
    // MARCAS
    // =====================================================

    // GET: CatalogoVehiculo/Marcas
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> Marcas()
    {
        var marcas = await _marcaService.ObtenerTodosAsync();

        return View(marcas);
    }

    // GET: CatalogoVehiculo/MarcasActivas
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> MarcasActivas()
    {
        var marcas = await _marcaService.ObtenerActivasAsync();

        return View("Marcas", marcas);
    }

    // GET: CatalogoVehiculo/DetalleMarca/5
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> DetalleMarca(int id)
    {
        var marca = await _marcaService.ObtenerPorIdAsync(id);

        if (marca == null)
            return NotFound();

        return View(marca);
    }

    // GET: CatalogoVehiculo/CrearMarca
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public IActionResult CrearMarca()
    {
        return View();
    }

    // POST: CatalogoVehiculo/CrearMarca
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public async Task<IActionResult> CrearMarca(
        MarcaGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _marcaService.CrearAsync(dto);

            TempData["Success"] =
                "Marca creada correctamente.";

            return RedirectToAction(nameof(Marcas));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }

    // GET: CatalogoVehiculo/EditarMarca/5
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> EditarMarca(int id)
    {
        var marca = await _marcaService.ObtenerPorIdAsync(id);

        if (marca == null)
            return NotFound();

        var dto = new MarcaGuardarDto
        {
            Nombre = marca.Nombre,
            Activo = marca.Activo
        };

        ViewBag.IdMarca = marca.IdMarca;

        return View(dto);
    }

    // POST: CatalogoVehiculo/EditarMarca/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> EditarMarca(
        int id,
        MarcaGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdMarca = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _marcaService.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Marca actualizada correctamente.";

            return RedirectToAction(nameof(Marcas));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdMarca = id;

            return View(dto);
        }
    }

    // POST: CatalogoVehiculo/CambiarEstadoMarca/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosDesactivar)]
    public async Task<IActionResult> CambiarEstadoMarca(
        int id,
        bool activo)
    {
        var actualizado =
            await _marcaService.CambiarEstadoAsync(id, activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Marca activada correctamente."
                : "Marca desactivada correctamente.";

        return RedirectToAction(nameof(Marcas));
    }


    // =====================================================
    // MODELOS
    // =====================================================

    // GET: CatalogoVehiculo/Modelos
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> Modelos()
    {
        var modelos =
            await _modeloService.ObtenerTodosAsync();

        return View(modelos);
    }

    // GET: CatalogoVehiculo/ModelosActivos
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> ModelosActivos()
    {
        var modelos =
            await _modeloService.ObtenerActivasAsync();

        return View("Modelos", modelos);
    }

    // GET: CatalogoVehiculo/DetalleModelo/5
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> DetalleModelo(int id)
    {
        var modelo =
            await _modeloService.ObtenerPorIdAsync(id);

        if (modelo == null)
            return NotFound();

        return View(modelo);
    }

    // GET: CatalogoVehiculo/CrearModelo
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public async Task<IActionResult> CrearModelo()
    {
        var marcas =
            await _marcaService.ObtenerActivasAsync();

        ViewBag.Marcas = marcas;

        return View();
    }

    // POST: CatalogoVehiculo/CrearModelo
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public async Task<IActionResult> CrearModelo(
        ModeloGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Marcas =
                await _marcaService.ObtenerActivasAsync();

            return View(dto);
        }

        try
        {
            await _modeloService.CrearAsync(dto);

            TempData["Success"] =
                "Modelo creado correctamente.";

            return RedirectToAction(nameof(Modelos));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.Marcas =
                await _marcaService.ObtenerActivasAsync();

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.Marcas =
                await _marcaService.ObtenerActivasAsync();

            return View(dto);
        }
    }

    // GET: CatalogoVehiculo/EditarModelo/5
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> EditarModelo(int id)
    {
        var modelo =
            await _modeloService.ObtenerPorIdAsync(id);

        if (modelo == null)
            return NotFound();

        var dto = new ModeloGuardarDto
        {
            IdMarca = modelo.IdMarca,
            Nombre = modelo.Nombre,
            Activo = modelo.Activo
        };

        ViewBag.IdModelo = modelo.IdModelo;

        return View(dto);
    }

    // POST: CatalogoVehiculo/EditarModelo/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> EditarModelo(
        int id,
        ModeloGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdModelo = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _modeloService.ActualizarAsync(
                    id,
                    dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Modelo actualizado correctamente.";

            return RedirectToAction(nameof(Modelos));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdModelo = id;

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdModelo = id;

            return View(dto);
        }
    }

    // POST: CatalogoVehiculo/CambiarEstadoModelo/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosDesactivar)]
    public async Task<IActionResult> CambiarEstadoModelo(
        int id,
        bool activo)
    {
        var actualizado =
            await _modeloService.CambiarEstadoAsync(
                id,
                activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Modelo activado correctamente."
                : "Modelo desactivado correctamente.";

        return RedirectToAction(nameof(Modelos));
    }


    // =====================================================
    // TIPOS DE VEHÍCULO
    // =====================================================

    // GET: CatalogoVehiculo/TiposVehiculo
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> TiposVehiculo()
    {
        var tipos =
            await _tipoVehiculoService.ObtenerTodosAsync();

        return View(tipos);
    }

    // GET: CatalogoVehiculo/TiposVehiculoActivos
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> TiposVehiculoActivos()
    {
        var tipos =
            await _tipoVehiculoService.ObtenerActivasAsync();

        return View("TiposVehiculo", tipos);
    }

    // GET: CatalogoVehiculo/DetalleTipoVehiculo/5
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> DetalleTipoVehiculo(int id)
    {
        var tipo =
            await _tipoVehiculoService.ObtenerPorIdAsync(id);

        if (tipo == null)
            return NotFound();

        return View(tipo);
    }

    // GET: CatalogoVehiculo/CrearTipoVehiculo
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public IActionResult CrearTipoVehiculo()
    {
        return View();
    }

    // POST: CatalogoVehiculo/CrearTipoVehiculo
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public async Task<IActionResult> CrearTipoVehiculo(
        TipoVehiculoGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _tipoVehiculoService.CrearAsync(dto);

            TempData["Success"] =
                "Tipo de vehículo creado correctamente.";

            return RedirectToAction(
                nameof(TiposVehiculo));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }

    // GET: CatalogoVehiculo/EditarTipoVehiculo/5
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> EditarTipoVehiculo(int id)
    {
        var tipo =
            await _tipoVehiculoService.ObtenerPorIdAsync(id);

        if (tipo == null)
            return NotFound();

        var dto = new TipoVehiculoGuardarDto
        {
            Nombre = tipo.Nombre,
            Activo = tipo.Activo
        };

        ViewBag.IdTipoVehiculo =
            tipo.IdTipoVehiculo;

        return View(dto);
    }

    // POST: CatalogoVehiculo/EditarTipoVehiculo/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> EditarTipoVehiculo(
        int id,
        TipoVehiculoGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdTipoVehiculo = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _tipoVehiculoService.ActualizarAsync(
                    id,
                    dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Tipo de vehículo actualizado correctamente.";

            return RedirectToAction(
                nameof(TiposVehiculo));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdTipoVehiculo = id;

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdTipoVehiculo = id;

            return View(dto);
        }
    }

    // POST: CatalogoVehiculo/CambiarEstadoTipoVehiculo/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosDesactivar)]
    public async Task<IActionResult> CambiarEstadoTipoVehiculo(
        int id,
        bool activo)
    {
        var actualizado =
            await _tipoVehiculoService.CambiarEstadoAsync(
                id,
                activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Tipo de vehículo activado correctamente."
                : "Tipo de vehículo desactivado correctamente.";

        return RedirectToAction(
            nameof(TiposVehiculo));
    }


    // =====================================================
    // TIPOS DE COMBUSTIBLE
    // =====================================================

    // GET: CatalogoVehiculo/TiposCombustible
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> TiposCombustible()
    {
        var tipos =
            await _tipoCombustibleService.ObtenerTodosAsync();

        return View(tipos);
    }

    // GET: CatalogoVehiculo/TiposCombustibleActivos
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> TiposCombustibleActivos()
    {
        var tipos =
            await _tipoCombustibleService.ObtenerActivasAsync();

        return View(
            "TiposCombustible",
            tipos);
    }

    // GET: CatalogoVehiculo/DetalleTipoCombustible/5
    [Authorize(Policy = Permisos.VehiculosVer)]
    public async Task<IActionResult> DetalleTipoCombustible(
        int id)
    {
        var tipo =
            await _tipoCombustibleService.ObtenerPorIdAsync(id);

        if (tipo == null)
            return NotFound();

        return View(tipo);
    }

    // GET: CatalogoVehiculo/CrearTipoCombustible
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public IActionResult CrearTipoCombustible()
    {
        return View();
    }

    // POST: CatalogoVehiculo/CrearTipoCombustible
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosCrear)]
    public async Task<IActionResult> CrearTipoCombustible(
        TipoCombustibleGuardarDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _tipoCombustibleService.CrearAsync(dto);

            TempData["Success"] =
                "Tipo de combustible creado correctamente.";

            return RedirectToAction(
                nameof(TiposCombustible));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(dto);
        }
    }

    // GET: CatalogoVehiculo/EditarTipoCombustible/5
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> EditarTipoCombustible(
        int id)
    {
        var tipo =
            await _tipoCombustibleService.ObtenerPorIdAsync(id);

        if (tipo == null)
            return NotFound();

        var dto = new TipoCombustibleGuardarDto
        {
            Nombre = tipo.Nombre,
            Activo = tipo.Activo
        };

        ViewBag.IdTipoCombustible =
            tipo.IdTipoCombustible;

        return View(dto);
    }

    // POST: CatalogoVehiculo/EditarTipoCombustible/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosEditar)]
    public async Task<IActionResult> EditarTipoCombustible(
        int id,
        TipoCombustibleGuardarDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.IdTipoCombustible = id;

            return View(dto);
        }

        try
        {
            var actualizado =
                await _tipoCombustibleService.ActualizarAsync(
                    id,
                    dto);

            if (!actualizado)
                return NotFound();

            TempData["Success"] =
                "Tipo de combustible actualizado correctamente.";

            return RedirectToAction(
                nameof(TiposCombustible));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdTipoCombustible = id;

            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            ViewBag.IdTipoCombustible = id;

            return View(dto);
        }
    }

    // POST: CatalogoVehiculo/CambiarEstadoTipoCombustible/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permisos.VehiculosDesactivar)]
    public async Task<IActionResult> CambiarEstadoTipoCombustible(
        int id,
        bool activo)
    {
        var actualizado =
            await _tipoCombustibleService.CambiarEstadoAsync(
                id,
                activo);

        if (!actualizado)
            return NotFound();

        TempData["Success"] =
            activo
                ? "Tipo de combustible activado correctamente."
                : "Tipo de combustible desactivado correctamente.";

        return RedirectToAction(
            nameof(TiposCombustible));
    }
}