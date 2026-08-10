using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class ModeloConfiguration : IEntityTypeConfiguration<Modelo>
{
    public void Configure(EntityTypeBuilder<Modelo> builder)
    {
        builder.ToTable("Modelo");

        builder.HasKey(x => x.IdModelo);

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasIndex(x => new
        {
            x.IdMarca,
            x.Nombre
        })
        .IsUnique();

        builder.HasOne(x => x.Marca)
            .WithMany(x => x.Modelos)
            .HasForeignKey(x => x.IdMarca)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Vehiculos)
            .WithOne(x => x.Modelo)
            .HasForeignKey(x => x.IdModelo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}