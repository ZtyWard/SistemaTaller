using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class TipoCombustibleConfiguration : IEntityTypeConfiguration<TipoCombustible>
{
    public void Configure(EntityTypeBuilder<TipoCombustible> builder)
    {
        builder.ToTable("TipoCombustible");

        builder.HasKey(x => x.IdTipoCombustible);

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Nombre)
            .IsUnique();

        builder.HasMany(x => x.Vehiculos)
            .WithOne(x => x.TipoCombustible)
            .HasForeignKey(x => x.IdTipoCombustible)
            .OnDelete(DeleteBehavior.Restrict);
    }
}