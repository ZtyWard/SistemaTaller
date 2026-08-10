using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class CategoriaProductoConfiguration : IEntityTypeConfiguration<CategoriaProducto>
{
    public void Configure(EntityTypeBuilder<CategoriaProducto> builder)
    {
        builder.ToTable("CategoriaProducto");

        builder.HasKey(x => x.IdCategoriaProducto);

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Nombre)
            .IsUnique();

        builder.HasMany(x => x.Productos)
            .WithOne(x => x.CategoriaProducto)
            .HasForeignKey(x => x.IdCategoriaProducto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}