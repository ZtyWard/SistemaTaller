using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Datos.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Negocios.Interfaces;
using Negocios.Services;
using Negocios.Seguridad;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// BASE DE DATOS
// =====================================================

builder.Services.AddDbContext<SistemaTallerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================================
// IDENTITY
// =====================================================

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(10);

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<SistemaTallerDbContext>()
    .AddDefaultTokenProviders();

// =====================================================
// COOKIE DE AUTENTICACIÓN
// =====================================================

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan =
        TimeSpan.FromHours(8);

    options.SlidingExpiration = true;
});

// =====================================================
// REPOSITORIES - DATOS
// =====================================================

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<IRecepcionRepository, RecepcionRepository>();
builder.Services.AddScoped<IDiagnosticoRepository, DiagnosticoRepository>();
builder.Services.AddScoped<ICotizacionRepository, CotizacionRepository>();
builder.Services.AddScoped<IOrdenTrabajoRepository, OrdenTrabajoRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

// =====================================================
// CITAS
// =====================================================

builder.Services.AddScoped<ICitaRepository, CitaRepository>();

// =====================================================
// ENTREGA DE VEHÍCULO
// =====================================================

builder.Services.AddScoped<IEntregaRepository, EntregaRepository>();

// =====================================================
// GARANTÍAS
// =====================================================

builder.Services.AddScoped<IGarantiaRepository, GarantiaRepository>();

// =====================================================
// INVENTARIO, COMPRAS, VENTAS Y FACTURACIÓN
// =====================================================

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IMovimientoInventarioRepository, MovimientoInventarioRepository>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();

// =====================================================
// PAGOS
// =====================================================

builder.Services.AddScoped<IPagoRepository, PagoRepository>();

// =====================================================
// REPORTES
// =====================================================

builder.Services.AddScoped<IReportesRepository, ReportesRepository>();

// =====================================================
// CONFIGURACIÓN GENERAL
// =====================================================

builder.Services.AddScoped<
    IConfiguracionGeneralRepository,
    ConfiguracionGeneralRepository>();

// =====================================================
// CATÁLOGOS
// =====================================================

builder.Services.AddScoped<ICategoriaProductoRepository, CategoriaProductoRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<IModeloRepository, ModeloRepository>();
builder.Services.AddScoped<ITipoVehiculoRepository, TipoVehiculoRepository>();
builder.Services.AddScoped<ITipoCombustibleRepository, TipoCombustibleRepository>();
builder.Services.AddScoped<IPuestoRepository, PuestoRepository>();
builder.Services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();

// =====================================================
// SERVICES - NEGOCIOS
// =====================================================

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IVehiculoService, VehiculoService>();
builder.Services.AddScoped<IRecepcionService, RecepcionService>();
builder.Services.AddScoped<IDiagnosticoService, DiagnosticoService>();
builder.Services.AddScoped<ICotizacionService, CotizacionService>();
builder.Services.AddScoped<IOrdenTrabajoService, OrdenTrabajoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IServicioService, ServicioService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// =====================================================
// CITAS
// =====================================================

builder.Services.AddScoped<ICitaService, CitaService>();

// =====================================================
// ENTREGA DE VEHÍCULO
// =====================================================

builder.Services.AddScoped<IEntregaService, EntregaService>();

// =====================================================
// GARANTÍAS
// =====================================================

builder.Services.AddScoped<IGarantiaService, GarantiaService>();

// =====================================================
// INVENTARIO, COMPRAS, VENTAS Y FACTURACIÓN
// =====================================================

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IMovimientoInventarioService, MovimientoInventarioService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();

// =====================================================
// PAGOS
// =====================================================

builder.Services.AddScoped<IPagoService, PagoService>();

// =====================================================
// REPORTES
// =====================================================

builder.Services.AddScoped<IReportesService, ReportesService>();

// =====================================================
// NOTIFICACIONES
// =====================================================

builder.Services.AddScoped<INotificacionService, NotificacionService>();

// =====================================================
// CONFIGURACIÓN GENERAL
// =====================================================

builder.Services.AddScoped<
    IConfiguracionGeneralService,
    ConfiguracionGeneralService>();

// =====================================================
// CATÁLOGOS
// =====================================================

builder.Services.AddScoped<ICategoriaProductoService, CategoriaProductoService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<IModeloService, ModeloService>();
builder.Services.AddScoped<ITipoVehiculoService, TipoVehiculoService>();
builder.Services.AddScoped<ITipoCombustibleService, TipoCombustibleService>();
builder.Services.AddScoped<IPuestoService, PuestoService>();
builder.Services.AddScoped<IEspecialidadService, EspecialidadService>();

// =====================================================
// AUTENTICACIÓN - NEGOCIOS
// =====================================================

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<PermisoClaimsFactory>();

// =====================================================
// USUARIOS - ADMINISTRACIÓN
// =====================================================

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// =====================================================
// ROLES - NEGOCIOS
// =====================================================

builder.Services.AddScoped<IRolService, RolService>();

// =====================================================
// AUTORIZACIÓN POR PERMISOS
// =====================================================

builder.Services.AddAuthorization(options =>
{
    foreach (var permiso in PermisosCatalogo.Todos)
    {
        options.AddPolicy(
            permiso,
            policy =>
            {
                policy.RequireClaim(
                    "Permiso",
                    permiso);
            });
    }
});

// =====================================================
// MVC
// =====================================================

builder.Services.AddControllersWithViews();

// =====================================================
// CONSTRUIR APLICACIÓN
// =====================================================

var app = builder.Build();

// =====================================================
// ROLES Y USUARIO ADMINISTRADOR
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager =
        scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles =
    {
        "Administrador",
        "Recepcionista",
        "Mecanico",
        "EncargadoInventario",
        "Vendedor",
        "Cajero",
        "Supervisor"
    };

    foreach (var nombreRol in roles)
    {
        if (!await roleManager.RoleExistsAsync(nombreRol))
        {
            var resultadoRol =
                await roleManager.CreateAsync(
                    new IdentityRole(nombreRol));

            if (!resultadoRol.Succeeded)
            {
                foreach (var error in resultadoRol.Errors)
                {
                    Console.WriteLine(
                        $"ERROR CREANDO ROL {nombreRol}: {error.Description}");
                }
            }
        }

        var rol =
            await roleManager.FindByNameAsync(nombreRol);

        if (rol == null)
        {
            continue;
        }

        var claims =
            await roleManager.GetClaimsAsync(rol);

        var permisosYaConfigurados =
            claims.Any(c =>
                c.Type ==
                RolClaims.PermisosConfigurados);

        if (!permisosYaConfigurados)
        {
            var resultadoMarcador =
                await roleManager.AddClaimAsync(
                    rol,
                    new System.Security.Claims.Claim(
                        RolClaims.PermisosConfigurados,
                        "true"));

            if (!resultadoMarcador.Succeeded)
            {
                foreach (
                    var error
                    in resultadoMarcador.Errors)
                {
                    Console.WriteLine(
                        $"ERROR CONFIGURANDO PERMISOS DEL ROL {nombreRol}: {error.Description}");
                }

                continue;
            }

            var permisosPorRol =
                PermisosPorRol.Obtener();

            if (permisosPorRol.TryGetValue(
                    nombreRol,
                    out var permisosRol))
            {
                foreach (var permiso in permisosRol)
                {
                    var resultadoPermiso =
                        await roleManager.AddClaimAsync(
                            rol,
                            new System.Security.Claims.Claim(
                                RolClaims.Permiso,
                                permiso));

                    if (!resultadoPermiso.Succeeded)
                    {
                        foreach (
                            var error
                            in resultadoPermiso.Errors)
                        {
                            Console.WriteLine(
                                $"ERROR ASIGNANDO PERMISO {permiso} AL ROL {nombreRol}: {error.Description}");
                        }
                    }
                }
            }
        }
    }

    // =====================================================
    // USUARIO ADMINISTRADOR
    // =====================================================

    var usuario =
        await userManager.FindByNameAsync("admin");

    if (usuario == null)
    {
        usuario = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@axis.local",
            NombreCompleto = "Administrador AXIS",
            Activo = true,
            EmailConfirmed = true
        };

        var resultadoCrear =
            await userManager.CreateAsync(
                usuario,
                "Admin123");

        if (!resultadoCrear.Succeeded)
        {
            foreach (var error in resultadoCrear.Errors)
            {
                Console.WriteLine(
                    $"ERROR CREANDO ADMIN: {error.Description}");
            }
        }
    }
    else
    {
        usuario.Activo = true;
        usuario.EmailConfirmed = true;

        await userManager.UpdateAsync(usuario);

        var token =
            await userManager.GeneratePasswordResetTokenAsync(
                usuario);

        var resultadoPassword =
            await userManager.ResetPasswordAsync(
                usuario,
                token,
                "Admin123");

        if (!resultadoPassword.Succeeded)
        {
            foreach (var error in resultadoPassword.Errors)
            {
                Console.WriteLine(
                    $"ERROR CAMBIANDO PASSWORD: {error.Description}");
            }
        }
    }

    if (!await userManager.IsInRoleAsync(
            usuario,
            "Administrador"))
    {
        var resultadoRolUsuario =
            await userManager.AddToRoleAsync(
                usuario,
                "Administrador");

        if (!resultadoRolUsuario.Succeeded)
        {
            foreach (var error in resultadoRolUsuario.Errors)
            {
                Console.WriteLine(
                    $"ERROR ASIGNANDO ROL: {error.Description}");
            }
        }
    }

    Console.WriteLine(
        "ROLES Y USUARIO ADMINISTRADOR VERIFICADOS.");
}

// =====================================================
// PIPELINE HTTP
// =====================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// =====================================================
// RUTA PRINCIPAL
// =====================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();