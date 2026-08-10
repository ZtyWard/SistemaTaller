using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.ToTable("Auditoria");

        builder.HasKey(x => x.IdAuditoria);

        builder.Property(x => x.UsuarioId);

        builder.Property(x => x.Fecha)
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.Modulo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Accion)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.RegistroId);

        builder.Property(x => x.Descripcion);

        builder.Property(x => x.Ip);

        builder.HasIndex(x => x.Fecha)
            .HasDatabaseName("IX_Auditoria_Fecha")
            .IsDescending();
    }
}