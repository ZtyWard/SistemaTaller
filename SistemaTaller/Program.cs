using Datos.Context;
using Datos.Interfaces;
using Datos.Models;
using Datos.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Negocios.Interfaces;
using Negocios.Services;

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
    })
    .AddEntityFrameworkStores<SistemaTallerDbContext>()
    .AddDefaultTokenProviders();

// =====================================================
// REPOSITORIES - DATOS
// =====================================================

// Clientes
builder.Services.AddScoped<
    IClienteRepository,
    ClienteRepository>();

// Vehículos
builder.Services.AddScoped<
    IVehiculoRepository,
    VehiculoRepository>();

// Empleados
builder.Services.AddScoped<
    IEmpleadoRepository,
    EmpleadoRepository>();

// Servicios
builder.Services.AddScoped<
    IServicioRepository,
    ServicioRepository>();

// Productos
builder.Services.AddScoped<
    IProductoRepository,
    ProductoRepository>();

// Proveedores
builder.Services.AddScoped<
    IProveedorRepository,
    ProveedorRepository>();

// Recepciones
builder.Services.AddScoped<
    IRecepcionRepository,
    RecepcionRepository>();

// Diagnósticos
builder.Services.AddScoped<
    IDiagnosticoRepository,
    DiagnosticoRepository>();

// Cotizaciones
builder.Services.AddScoped<
    ICotizacionRepository,
    CotizacionRepository>();

// Órdenes de trabajo
builder.Services.AddScoped<
    IOrdenTrabajoRepository,
    OrdenTrabajoRepository>();

// Movimientos de inventario
builder.Services.AddScoped<
    IMovimientoInventarioRepository,
    MovimientoInventarioRepository>();

// Compras
builder.Services.AddScoped<
    ICompraRepository,
    CompraRepository>();

// =====================================================
// SERVICES - NEGOCIOS
// =====================================================

// Clientes
builder.Services.AddScoped<
    IClienteService,
    ClienteService>();

// Vehículos
builder.Services.AddScoped<
    IVehiculoService,
    VehiculoService>();

// Empleados
builder.Services.AddScoped<
    IEmpleadoService,
    EmpleadoService>();

// Servicios
builder.Services.AddScoped<
    IServicioService,
    ServicioService>();

// Productos
builder.Services.AddScoped<
    IProductoService,
    ProductoService>();

// Proveedores
builder.Services.AddScoped<
    IProveedorService,
    ProveedorService>();

// Recepciones
builder.Services.AddScoped<
    IRecepcionService,
    RecepcionService>();

// Diagnósticos
builder.Services.AddScoped<
    IDiagnosticoService,
    DiagnosticoService>();

// Cotizaciones
builder.Services.AddScoped<
    ICotizacionService,
    CotizacionService>();

// Órdenes de trabajo
builder.Services.AddScoped<
    IOrdenTrabajoService,
    OrdenTrabajoService>();

// Movimientos de inventario
builder.Services.AddScoped<
    IMovimientoInventarioService,
    MovimientoInventarioService>();

// Compras
builder.Services.AddScoped<
    ICompraService,
    CompraService>();

// =====================================================
// MVC
// =====================================================

builder.Services.AddControllersWithViews();

// =====================================================
// CONSTRUCCIÓN DE LA APLICACIÓN
// =====================================================

var app = builder.Build();

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