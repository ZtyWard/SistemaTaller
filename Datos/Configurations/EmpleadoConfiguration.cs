using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
{
    public void Configure(EntityTypeBuilder<Empleado> builder)
    {
        builder.ToTable("Empleado");

        builder.HasKey(x => x.IdEmpleado);

        builder.Property(x => x.Cedula)
            .IsRequired();

        builder.Property(x => x.Nombre)
            .IsRequired();

        builder.Property(x => x.Apellido1)
            .IsRequired();

        builder.Property(x => x.Apellido2);

        builder.Property(x => x.Telefono);

        builder.Property(x => x.Correo);

        builder.Property(x => x.Salario)
            .HasPrecision(18, 2);

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasOne(x => x.Puesto)
            .WithMany(x => x.Empleados)
            .HasForeignKey(x => x.IdPuesto)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Especialidad)
            .WithMany(x => x.Empleados)
            .HasForeignKey(x => x.IdEspecialidad)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Recepciones)
            .WithOne(x => x.Empleado)
            .HasForeignKey(x => x.IdEmpleado)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Diagnosticos)
            .WithOne(x => x.Empleado)
            .HasForeignKey(x => x.IdEmpleado)
            .OnDelete(DeleteBehavior.Restrict);
    }
}