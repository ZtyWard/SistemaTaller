using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class CotizacionConfiguration : IEntityTypeConfiguration<Cotizacion>
{
    public void Configure(EntityTypeBuilder<Cotizacion> builder)
    {
        builder.ToTable("Cotizacion");

        builder.HasKey(x => x.IdCotizacion);

        builder.Property(x => x.Fecha)
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.Total)
            .HasPrecision(18, 2)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasDefaultValue("Pendiente");

        builder.HasOne(x => x.Diagnostico)
            .WithMany(x => x.Cotizaciones)
            .HasForeignKey(x => x.IdDiagnostico)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.OrdenesTrabajo)
            .WithOne(x => x.Cotizacion)
            .HasForeignKey(x => x.IdCotizacion)
            .OnDelete(DeleteBehavior.Restrict);
    }
}