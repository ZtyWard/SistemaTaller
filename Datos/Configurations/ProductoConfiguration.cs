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

        // ==========================================
        // CLAVE PRIMARIA
        // ==========================================

        builder.HasKey(x => x.IdProducto);


        // ==========================================
        // IDENTIFICACIÓN
        // ==========================================

        builder.Property(x => x.Codigo)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.CodigoBarras)
            .HasMaxLength(50)
            .IsRequired(false);


        // ==========================================
        // INFORMACIÓN DEL PRODUCTO
        // ==========================================

        builder.Property(x => x.Nombre)
            .IsRequired();

        builder.Property(x => x.Descripcion)
            .IsRequired(false);

        builder.Property(x => x.ImagenUrl)
            .HasMaxLength(500)
            .IsRequired(false);


        // ==========================================
        // PRECIOS
        // ==========================================

        builder.Property(x => x.PrecioCompra)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.PrecioVenta)
            .HasPrecision(18, 2)
            .IsRequired();


        // ==========================================
        // INVENTARIO
        // ==========================================

        builder.Property(x => x.Stock)
            .IsRequired();

        builder.Property(x => x.StockMinimo)
            .HasDefaultValue(5)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasDefaultValue(true)
            .IsRequired();


        // ==========================================
        // ÍNDICES
        // ==========================================

        builder.HasIndex(x => x.Codigo)
            .IsUnique();

        // El código de barras puede ser NULL
        // para productos antiguos.
        //
        // Cuando exista un código de barras,
        // debe ser único.
        builder.HasIndex(x => x.CodigoBarras)
            .IsUnique()
            .HasFilter("[CodigoBarras] IS NOT NULL");


        // ==========================================
        // CATEGORÍA
        // ==========================================

        builder.HasOne(x => x.CategoriaProducto)
            .WithMany(x => x.Productos)
            .HasForeignKey(x => x.IdCategoriaProducto)
            .OnDelete(DeleteBehavior.Restrict);


        // ==========================================
        // MOVIMIENTOS DE INVENTARIO
        // ==========================================

        builder.HasMany(x => x.MovimientosInventario)
            .WithOne(x => x.Producto)
            .HasForeignKey(x => x.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);
    }
}