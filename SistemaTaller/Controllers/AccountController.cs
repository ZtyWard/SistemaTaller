using Datos.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Negocios.DTOs;
using Negocios.Interfaces;
using Negocios.Seguridad;
using System.Security.Claims;

// =====================================================

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly PermisoClaimsFactory _permisoClaimsFactory;

    public AccountController(
        IAuthService authService,
        SignInManager<ApplicationUser> signInManager,
        PermisoClaimsFactory permisoClaimsFactory)
    {
        _authService = authService;
        _signInManager = signInManager;
        _permisoClaimsFactory = permisoClaimsFactory;
    }

    // =====================================================
    // LOGIN - GET
    // =====================================================

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    // =====================================================
    // LOGIN - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginDto dto,
        string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View(dto);
        }

        var usuario =
            await _authService.ValidarUsuarioAsync(dto);

        if (usuario == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "El usuario o la contraseña son incorrectos.");

            ViewBag.ReturnUrl = returnUrl;

            return View(dto);
        }

        // =================================================
        // OBTENER PERMISOS DEL USUARIO
        // =================================================

        var permisos =
            await _permisoClaimsFactory
                .ObtenerClaimsAsync(usuario);

        // =================================================
        // CREAR CLAIMS DE AUTENTICACIÓN
        // =================================================

        var claims =
            new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id),

                new Claim(
                    ClaimTypes.Name,
                    usuario.UserName ?? string.Empty),

                new Claim(
                    "NombreCompleto",
                    usuario.NombreCompleto ?? string.Empty)
            };

        // =================================================
        // AGREGAR ROLES
        // =================================================

        var roles =
            await _signInManager
                .UserManager
                .GetRolesAsync(usuario);

        foreach (var rol in roles)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    rol));
        }

        // =================================================
        // AGREGAR PERMISOS
        // =================================================

        claims.AddRange(permisos);

        // =================================================
        // CREAR IDENTIDAD
        // =================================================

        var identidad =
            new ClaimsIdentity(
                claims,
                IdentityConstants.ApplicationScheme);

        var principal =
            new ClaimsPrincipal(identidad);

        // =================================================
        // CREAR COOKIE
        // =================================================

        await HttpContext.SignInAsync(
            IdentityConstants.ApplicationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = dto.Recordarme,
                ExpiresUtc =
                    DateTimeOffset.UtcNow.AddHours(8)
            });

        // =================================================
        // REDIRECCIÓN
        // =================================================

        if (!string.IsNullOrWhiteSpace(returnUrl) &&
            Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(
            "Index",
            "Home");
    }

    // =====================================================
    // LOGOUT
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction(
            nameof(Login));
    }

    // =====================================================
    // ACCESS DENIED
    // =====================================================

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}