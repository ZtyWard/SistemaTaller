using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class DetalleVentaConfiguration
    : IEntityTypeConfiguration<DetalleVenta>
{
    public void Configure(
        EntityTypeBuilder<DetalleVenta> builder)
    {
        // =====================================================
        // TABLA
        // =====================================================

        builder.ToTable("DetalleVenta");

        // =====================================================
        // PRIMARY KEY
        // =====================================================

        builder.HasKey(x =>
            x.IdDetalleVenta);

        builder.Property(x =>
                x.IdDetalleVenta)
            .ValueGeneratedOnAdd();

        // =====================================================
        // PROPIEDADES
        // =====================================================

        builder.Property(x =>
                x.IdVenta)
            .IsRequired();

        builder.Property(x =>
                x.IdProducto)
            .IsRequired();

        builder.Property(x =>
                x.Cantidad)
            .IsRequired();

        builder.Property(x =>
                x.PrecioUnitario)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x =>
                x.Impuesto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x =>
                x.Descuento)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x =>
                x.Subtotal)
            .HasPrecision(18, 2)
            .IsRequired();

        // =====================================================
        // RELACIÓN CON VENTA
        // =====================================================

        builder.HasOne(x =>
                x.Venta)
            .WithMany(x =>
                x.Detalles)
            .HasForeignKey(x =>
                x.IdVenta)
            .OnDelete(DeleteBehavior.Cascade);

        // =====================================================
        // RELACIÓN CON PRODUCTO
        // =====================================================

        builder.HasOne(x =>
                x.Producto)
            .WithMany()
            .HasForeignKey(x =>
                x.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}