using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class ServicioConfiguration : IEntityTypeConfiguration<Servicio>
{
    public void Configure(EntityTypeBuilder<Servicio> builder)
    {
        builder.ToTable("Servicio");

        builder.HasKey(x => x.IdServicio);

        builder.Property(x => x.Nombre)
            .IsRequired();

        builder.Property(x => x.Descripcion);

        builder.Property(x => x.Precio)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.DuracionEstimada);

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);
    }
}