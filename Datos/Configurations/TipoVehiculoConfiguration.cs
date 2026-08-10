using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class TipoVehiculoConfiguration : IEntityTypeConfiguration<TipoVehiculo>
{
    public void Configure(EntityTypeBuilder<TipoVehiculo> builder)
    {
        builder.ToTable("TipoVehiculo");

        builder.HasKey(x => x.IdTipoVehiculo);

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Nombre)
            .IsUnique();

        builder.HasMany(x => x.Vehiculos)
            .WithOne(x => x.TipoVehiculo)
            .HasForeignKey(x => x.IdTipoVehiculo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}