using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Cliente");

        builder.HasKey(x => x.IdCliente);

        builder.Property(x => x.Cedula)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Apellido1)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Apellido2)
            .HasMaxLength(100);

        builder.Property(x => x.Telefono)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Correo);

        builder.Property(x => x.Direccion);

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        builder.Property(x => x.FechaRegistro)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(x => x.Cedula)
            .IsUnique();

        builder.HasMany(x => x.Vehiculos)
            .WithOne(x => x.Cliente)
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);
    }
}