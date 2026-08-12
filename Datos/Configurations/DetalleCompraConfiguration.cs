using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class DetalleCompraConfiguration
    : IEntityTypeConfiguration<DetalleCompra>
{
    public void Configure(
        EntityTypeBuilder<DetalleCompra> builder)
    {
        // =====================================================
        // TABLA
        // =====================================================

        builder.ToTable("DetalleCompra");

        // =====================================================
        // PRIMARY KEY
        // =====================================================

        builder.HasKey(x =>
            x.IdDetalleCompra);

        // =====================================================
        // PROPIEDADES
        // =====================================================

        builder.Property(x =>
                x.IdDetalleCompra)
            .ValueGeneratedOnAdd();

        builder.Property(x =>
                x.IdCompra)
            .IsRequired();

        builder.Property(x =>
                x.IdProducto)
            .IsRequired();

        builder.Property(x =>
                x.Cantidad)
            .IsRequired();

        builder.Property(x =>
                x.CostoUnitario)
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
        // RELACIÓN CON COMPRA
        // =====================================================

        builder.HasOne(x =>
                x.Compra)
            .WithMany(x =>
                x.Detalles)
            .HasForeignKey(x =>
                x.IdCompra)
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