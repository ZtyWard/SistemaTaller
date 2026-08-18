using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Producto", table =>
        {
            table.HasTrigger("TR_Auditoria_Producto");
        });

        builder.HasKey(x => x.IdProducto);

        builder.Property(x => x.Codigo)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.Nombre)
            .IsRequired();

        builder.Property(x => x.Descripcion);

        builder.Property(x => x.PrecioCompra)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.PrecioVenta)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Stock)
            .IsRequired();

        builder.Property(x => x.StockMinimo)
            .HasDefaultValue(5)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Codigo)
            .IsUnique();

        builder.HasOne(x => x.CategoriaProducto)
            .WithMany(x => x.Productos)
            .HasForeignKey(x => x.IdCategoriaProducto)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.MovimientosInventario)
            .WithOne(x => x.Producto)
            .HasForeignKey(x => x.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}