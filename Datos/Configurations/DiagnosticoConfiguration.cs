using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class DiagnosticoConfiguration : IEntityTypeConfiguration<Diagnostico>
{
    public void Configure(EntityTypeBuilder<Diagnostico> builder)
    {
        builder.ToTable("Diagnostico");

        builder.HasKey(x => x.IdDiagnostico);

        builder.Property(x => x.Descripcion)
            .IsRequired();

        builder.Property(x => x.FechaDiagnostico)
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.HasOne(x => x.Recepcion)
            .WithMany(x => x.Diagnosticos)
            .HasForeignKey(x => x.IdRecepcion)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Empleado)
            .WithMany(x => x.Diagnosticos)
            .HasForeignKey(x => x.IdEmpleado)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Cotizaciones)
            .WithOne(x => x.Diagnostico)
            .HasForeignKey(x => x.IdDiagnostico)
            .OnDelete(DeleteBehavior.Restrict);
    }
}