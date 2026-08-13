using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class FacturaConfiguration
    : IEntityTypeConfiguration<Factura>
{
    public void Configure(
        EntityTypeBuilder<Factura> builder)
    {
        builder.ToTable("Factura");

        builder.HasKey(x => x.IdFactura);

        builder.Property(x => x.NumeroFactura)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.FechaEmision)
            .IsRequired();

        builder.Property(x => x.Subtotal)
            .HasColumnType("decimal(12,2)");

        builder.Property(x => x.Impuesto)
            .HasColumnType("decimal(12,2)");

        builder.Property(x => x.Descuento)
            .HasColumnType("decimal(12,2)");

        builder.Property(x => x.Total)
            .HasColumnType("decimal(12,2)");

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.UsuarioId)
            .HasMaxLength(450);

        builder.HasIndex(x => x.NumeroFactura)
            .IsUnique();

        // =================================================
        // CLIENTE
        // =================================================

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        // =================================================
        // ORDEN DE TRABAJO
        // =================================================

        builder.HasOne(x => x.OrdenTrabajo)
            .WithMany()
            .HasForeignKey(x => x.IdOrdenTrabajo)
            .OnDelete(DeleteBehavior.Restrict);

        // =================================================
        // VENTA
        // =================================================

        builder.HasOne(x => x.Venta)
            .WithMany()
            .HasForeignKey(x => x.IdVenta)
            .OnDelete(DeleteBehavior.Restrict);
    }
}