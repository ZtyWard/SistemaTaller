using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class MovimientoInventarioConfiguration : IEntityTypeConfiguration<MovimientoInventario>
{
    public void Configure(EntityTypeBuilder<MovimientoInventario> builder)
    {
        builder.ToTable("MovimientoInventario");

        builder.HasKey(x => x.IdMovimiento);

        builder.Property(x => x.TipoMovimiento)
            .IsRequired();

        builder.Property(x => x.Cantidad)
            .IsRequired();

        builder.Property(x => x.FechaMovimiento)
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.Observacion);

        builder.HasOne(x => x.Producto)
            .WithMany(x => x.MovimientosInventario)
            .HasForeignKey(x => x.IdProducto)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.IdProducto,
            x.FechaMovimiento
        });
    }
}