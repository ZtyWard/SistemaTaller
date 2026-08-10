using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class MarcaConfiguration : IEntityTypeConfiguration<Marca>
{
    public void Configure(EntityTypeBuilder<Marca> builder)
    {
        builder.ToTable("Marca");

        builder.HasKey(x => x.IdMarca);

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Nombre)
            .IsUnique();

        builder.HasMany(x => x.Modelos)
            .WithOne(x => x.Marca)
            .HasForeignKey(x => x.IdMarca)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Vehiculos)
            .WithOne(x => x.Marca)
            .HasForeignKey(x => x.IdMarca)
            .OnDelete(DeleteBehavior.Restrict);
    }
}