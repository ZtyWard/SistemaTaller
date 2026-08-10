using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class CompraConfiguration : IEntityTypeConfiguration<Compra>
{
    public void Configure(EntityTypeBuilder<Compra> builder)
    {
        builder.ToTable("Compra");

        builder.HasKey(x => x.IdCompra);

        builder.Property(x => x.FechaCompra)
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.Total)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Estado)
            .IsRequired()
            .HasDefaultValue("Pendiente");

        builder.HasOne(x => x.Proveedor)
            .WithMany(x => x.Compras)
            .HasForeignKey(x => x.IdProveedor)
            .OnDelete(DeleteBehavior.Restrict);
    }
}