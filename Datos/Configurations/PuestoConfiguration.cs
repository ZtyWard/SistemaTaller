using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class PuestoConfiguration : IEntityTypeConfiguration<Puesto>
{
    public void Configure(EntityTypeBuilder<Puesto> builder)
    {
        builder.ToTable("Puesto");

        builder.HasKey(x => x.IdPuesto);

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Nombre)
            .IsUnique();

        builder.HasMany(x => x.Empleados)
            .WithOne(x => x.Puesto)
            .HasForeignKey(x => x.IdPuesto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}