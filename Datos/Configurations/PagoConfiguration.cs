using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class PagoConfiguration
    : IEntityTypeConfiguration<Pago>
{
    public void Configure(
        EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("Pago");

        builder.HasKey(x => x.IdPago);

        builder.Property(x => x.Monto)
            .HasColumnType("decimal(12,2)")
            .IsRequired();

        builder.Property(x => x.FormaPago)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.NumeroReferencia)
            .HasMaxLength(100);

        builder.Property(x => x.FechaPago)
            .IsRequired();

        builder.Property(x => x.UsuarioId)
            .HasMaxLength(450);

        builder.Property(x => x.Observaciones)
            .HasMaxLength(500);

        builder.HasOne(x => x.Factura)
            .WithMany(x => x.Pagos)
            .HasForeignKey(x => x.IdFactura)
            .OnDelete(DeleteBehavior.Restrict);
    }
}