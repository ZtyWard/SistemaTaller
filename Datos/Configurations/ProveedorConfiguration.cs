using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        builder.ToTable("Proveedor");

        builder.HasKey(x => x.IdProveedor);

        builder.Property(x => x.Nombre)
            .IsRequired();

        builder.Property(x => x.CedulaJuridica);

        builder.Property(x => x.Telefono);

        builder.Property(x => x.Correo);

        builder.Property(x => x.Direccion);

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.HasMany(x => x.Compras)
            .WithOne(x => x.Proveedor)
            .HasForeignKey(x => x.IdProveedor)
            .OnDelete(DeleteBehavior.Restrict);
    }
}