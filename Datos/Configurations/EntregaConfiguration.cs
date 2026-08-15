using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class EntregaConfiguration
    : IEntityTypeConfiguration<Entrega>
{
    public void Configure(
        EntityTypeBuilder<Entrega> builder)
    {
        builder.ToTable("Entrega");

        builder.HasKey(x => x.IdEntrega);

        builder.Property(x => x.FechaEntrega)
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.KilometrajeSalida)
            .IsRequired();

        builder.Property(x => x.PersonaRecibe)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Observaciones)
            .HasMaxLength(500);

        builder.Property(x => x.Recomendaciones)
            .HasMaxLength(1000);

        builder.Property(x => x.TieneGarantia)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.EstadoPago)
            .HasMaxLength(30)
            .HasDefaultValue("Pendiente")
            .IsRequired();

        builder.Property(x => x.Aceptacion)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.FirmaNombre)
            .HasMaxLength(200);

        builder.Property(x => x.FechaAceptacion);

        // =================================================
        // UNA SOLA ENTREGA POR ORDEN
        // =================================================

        builder.HasIndex(x => x.IdOrdenTrabajo)
            .IsUnique()
            .HasDatabaseName(
                "UQ_Entrega_IdOrdenTrabajo");

        // =================================================
        // RELACIÓN CON ORDEN DE TRABAJO
        // =================================================

        builder.HasOne(x => x.OrdenTrabajo)
            .WithMany()
            .HasForeignKey(x => x.IdOrdenTrabajo)
            .OnDelete(DeleteBehavior.Restrict);

        // =================================================
        // CHECKS
        // =================================================

        builder.ToTable(
            "Entrega",
            table =>
            {
                table.HasCheckConstraint(
                    "CK_Entrega_KilometrajeSalida",
                    "[KilometrajeSalida] >= 0");

                table.HasCheckConstraint(
                    "CK_Entrega_EstadoPago",
                    "[EstadoPago] IN ('Pendiente', 'Parcial', 'Pagado')");
            });
    }
}