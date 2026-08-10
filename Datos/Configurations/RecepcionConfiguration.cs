using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class RecepcionConfiguration : IEntityTypeConfiguration<Recepcion>
{
    public void Configure(EntityTypeBuilder<Recepcion> builder)
    {
        builder.ToTable("Recepcion");

        builder.HasKey(x => x.IdRecepcion);

        builder.Property(x => x.FechaRecepcion)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.Kilometraje)
            .IsRequired();

        builder.Property(x => x.NivelCombustible);

        builder.Property(x => x.Observaciones);

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasDefaultValue("Recibido");

        builder.HasOne(x => x.Vehiculo)
            .WithMany(x => x.Recepciones)
            .HasForeignKey(x => x.IdVehiculo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Empleado)
            .WithMany(x => x.Recepciones)
            .HasForeignKey(x => x.IdEmpleado)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Diagnosticos)
            .WithOne(x => x.Recepcion)
            .HasForeignKey(x => x.IdRecepcion)
            .OnDelete(DeleteBehavior.Restrict);
    }
}