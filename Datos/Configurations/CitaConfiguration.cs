using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        builder.ToTable("Cita");

        builder.HasKey(x => x.IdCita);

        builder.Property(x => x.NumeroCita)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.NumeroCita)
            .IsUnique();

        builder.Property(x => x.Area)
            .HasMaxLength(100);

        builder.Property(x => x.FechaInicio)
            .IsRequired();

        builder.Property(x => x.FechaFin)
            .IsRequired();

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasMaxLength(30)
            .HasDefaultValue("Programada");

        builder.Property(x => x.Observaciones)
            .HasMaxLength(500);

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Vehiculo)
            .WithMany()
            .HasForeignKey(x => x.IdVehiculo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Servicio)
            .WithMany()
            .HasForeignKey(x => x.IdServicio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Empleado)
            .WithMany()
            .HasForeignKey(x => x.IdEmpleado)
            .OnDelete(DeleteBehavior.Restrict);
    }
}