using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class VentaConfiguration
    : IEntityTypeConfiguration<Venta>
{
    public void Configure(
        EntityTypeBuilder<Venta> builder)
    {
        // =====================================================
        // TABLA
        // =====================================================

        builder.ToTable("Venta");

        // =====================================================
        // PRIMARY KEY
        // =====================================================

        builder.HasKey(x =>
            x.IdVenta);

        builder.Property(x =>
                x.IdVenta)
            .ValueGeneratedOnAdd();

        // =====================================================
        // PROPIEDADES
        // =====================================================

        builder.Property(x =>
                x.NumeroVenta)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x =>
                x.IdCliente)
            .IsRequired(false);

        builder.Property(x =>
                x.IdVendedor)
            .IsRequired();

        builder.Property(x =>
                x.IdCajero)
            .IsRequired(false);

        builder.Property(x =>
                x.FechaVenta)
            .IsRequired();

        builder.Property(x =>
                x.Subtotal)
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
                x.Total)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x =>
                x.FormaPago)
            .HasMaxLength(50);

        builder.Property(x =>
                x.Estado)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x =>
                x.UsuarioId)
            .HasMaxLength(450);

        // =====================================================
        // CLIENTE
        // =====================================================

        builder.HasOne(x =>
                x.Cliente)
            .WithMany()
            .HasForeignKey(x =>
                x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        // =====================================================
        // DETALLES
        // =====================================================

        builder.HasMany(x =>
                x.Detalles)
            .WithOne(x =>
                x.Venta)
            .HasForeignKey(x =>
                x.IdVenta)
            .OnDelete(DeleteBehavior.Cascade);
    }
}