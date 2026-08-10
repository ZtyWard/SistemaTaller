using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> builder)
    {
        builder.ToTable("Vehiculo");

        builder.HasKey(x => x.IdVehiculo);

        builder.Property(x => x.Placa)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.VIN);

        builder.Property(x => x.Color);

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.Placa)
            .IsUnique();

        builder.HasOne(x => x.Cliente)
            .WithMany(x => x.Vehiculos)
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Marca)
            .WithMany(x => x.Vehiculos)
            .HasForeignKey(x => x.IdMarca)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Modelo)
            .WithMany(x => x.Vehiculos)
            .HasForeignKey(x => x.IdModelo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TipoVehiculo)
            .WithMany(x => x.Vehiculos)
            .HasForeignKey(x => x.IdTipoVehiculo)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TipoCombustible)
            .WithMany(x => x.Vehiculos)
            .HasForeignKey(x => x.IdTipoCombustible)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Recepciones)
            .WithOne(x => x.Vehiculo)
            .HasForeignKey(x => x.IdVehiculo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}