using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class OrdenTrabajoConfiguration : IEntityTypeConfiguration<OrdenTrabajo>
{
    public void Configure(EntityTypeBuilder<OrdenTrabajo> builder)
    {
        builder.ToTable("OrdenTrabajo");

        builder.HasKey(x => x.IdOrdenTrabajo);

        builder.Property(x => x.FechaInicio)
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.FechaFin);

        builder.Property(x => x.Estado)
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("Abierta");

        builder.Property(x => x.Observaciones);

        builder.HasOne(x => x.Cotizacion)
            .WithMany(x => x.OrdenesTrabajo)
            .HasForeignKey(x => x.IdCotizacion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.Estado,
            x.FechaInicio
        })
        .HasDatabaseName("IX_OrdenTrabajo_Estado_FechaInicio")
        .IsDescending(false, true);
    }
}