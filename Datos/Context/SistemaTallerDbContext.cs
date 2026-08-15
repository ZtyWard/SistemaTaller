using Datos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Datos.Context;

public class SistemaTallerDbContext
    : IdentityDbContext<ApplicationUser>
{
    public SistemaTallerDbContext(
        DbContextOptions<SistemaTallerDbContext> options)
        : base(options)
    {
    }

    // =====================================================
    // ENTIDADES PRINCIPALES
    // =====================================================

    public DbSet<Cliente>
        Clientes => Set<Cliente>();

    public DbSet<Vehiculo>
        Vehiculos => Set<Vehiculo>();

    public DbSet<Empleado>
        Empleados => Set<Empleado>();

    public DbSet<Servicio>
        Servicios => Set<Servicio>();

    public DbSet<Producto>
        Productos => Set<Producto>();

    public DbSet<Proveedor>
        Proveedores => Set<Proveedor>();

    public DbSet<Recepcion>
        Recepciones => Set<Recepcion>();

    public DbSet<Diagnostico>
        Diagnosticos => Set<Diagnostico>();

    public DbSet<Cotizacion>
        Cotizaciones => Set<Cotizacion>();

    public DbSet<OrdenTrabajo>
        OrdenesTrabajo => Set<OrdenTrabajo>();

    public DbSet<Cita>
        Citas => Set<Cita>();

    public DbSet<MovimientoInventario>
        MovimientosInventario => Set<MovimientoInventario>();

    public DbSet<Compra>
        Compras => Set<Compra>();

    public DbSet<DetalleCompra>
        DetallesCompra => Set<DetalleCompra>();

    // =====================================================
    // VENTAS
    // =====================================================

    public DbSet<Venta>
        Ventas => Set<Venta>();

    public DbSet<DetalleVenta>
        DetallesVenta => Set<DetalleVenta>();

    // =====================================================
    // FACTURACIÓN
    // =====================================================

    public DbSet<Factura>
        Facturas => Set<Factura>();

    public DbSet<Pago>
        Pagos => Set<Pago>();

    // =====================================================
    // AUDITORÍA
    // =====================================================

    public DbSet<Auditoria>
        Auditorias => Set<Auditoria>();

    // =====================================================
    // CATÁLOGOS
    // =====================================================

    public DbSet<CategoriaProducto>
        CategoriasProducto => Set<CategoriaProducto>();

    public DbSet<Marca>
        Marcas => Set<Marca>();

    public DbSet<Modelo>
        Modelos => Set<Modelo>();

    public DbSet<TipoVehiculo>
        TiposVehiculo => Set<TipoVehiculo>();

    public DbSet<TipoCombustible>
        TiposCombustible => Set<TipoCombustible>();

    public DbSet<Puesto>
        Puestos => Set<Puesto>();

    public DbSet<Especialidad>
        Especialidades => Set<Especialidad>();

    // =====================================================
    // CONFIGURACIÓN
    // =====================================================

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .ToTable("IdentityUsuarios");

        builder.ApplyConfigurationsFromAssembly(
            typeof(SistemaTallerDbContext).Assembly);
    }
}