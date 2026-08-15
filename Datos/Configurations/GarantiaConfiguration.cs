using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class GarantiaConfiguration
    : IEntityTypeConfiguration<Garantia>
{
    public void Configure(
        EntityTypeBuilder<Garantia> builder)
    {
        builder.ToTable("Garantia");

        builder.HasKey(x => x.IdGarantia);

        // =================================================
        // CAMPOS
        // =================================================

        builder.Property(x => x.FechaInicio)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.FechaVencimiento)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasMaxLength(30)
            .HasDefaultValue("Vigente")
            .IsRequired();

        builder.Property(x => x.Condiciones)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Observaciones)
            .HasColumnType("nvarchar(max)");

        // =================================================
        // RELACIÓN ORDEN DE TRABAJO
        // =================================================

        builder.HasOne(x => x.OrdenTrabajo)
            .WithMany()
            .HasForeignKey(x => x.IdOrdenTrabajo)
            .OnDelete(DeleteBehavior.Restrict);

        // =================================================
        // RELACIÓN VENTA
        // =================================================

        builder.HasOne(x => x.Venta)
            .WithMany()
            .HasForeignKey(x => x.IdVenta)
            .OnDelete(DeleteBehavior.Restrict);

        // =================================================
        // RELACIÓN PRODUCTO
        // =================================================

        builder.HasOne(x => x.Producto)
            .WithMany()
            .HasForeignKey(x => x.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);

        // =================================================
        // RELACIÓN SERVICIO
        // =================================================

        builder.HasOne(x => x.Servicio)
            .WithMany()
            .HasForeignKey(x => x.IdServicio)
            .OnDelete(DeleteBehavior.Restrict);

        // =================================================
        // ÍNDICES
        // =================================================

        builder.HasIndex(x => x.IdOrdenTrabajo)
            .HasDatabaseName(
                "IX_Garantia_IdOrdenTrabajo");

        builder.HasIndex(x => x.IdVenta)
            .HasDatabaseName(
                "IX_Garantia_IdVenta");

        builder.HasIndex(x => x.IdProducto)
            .HasDatabaseName(
                "IX_Garantia_IdProducto");

        builder.HasIndex(x => x.IdServicio)
            .HasDatabaseName(
                "IX_Garantia_IdServicio");

        builder.HasIndex(x => x.Estado)
            .HasDatabaseName(
                "IX_Garantia_Estado");

        builder.HasIndex(x => x.FechaVencimiento)
            .HasDatabaseName(
                "IX_Garantia_FechaVencimiento");
    }
}