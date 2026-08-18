using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class ConfiguracionGeneralConfiguration
    : IEntityTypeConfiguration<ConfiguracionGeneral>
{
    public void Configure(
        EntityTypeBuilder<ConfiguracionGeneral> builder)
    {
        builder.ToTable("ConfiguracionGeneral");

        builder.HasKey(x => x.IdConfiguracion);

        builder.Property(x => x.NombreTaller)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.IdentificacionJuridica)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Direccion)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.Telefono)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Correo)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.LogoUrl)
            .HasMaxLength(500);

        builder.Property(x => x.ImpuestoPorcentaje)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.Moneda)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.LimiteDescuentoPorcentaje)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.PrefijoRecepcion)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.PrefijoCotizacion)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.PrefijoOrdenTrabajo)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.PrefijoVenta)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.PrefijoFactura)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.HoraApertura)
            .IsRequired();

        builder.Property(x => x.HoraCierre)
            .IsRequired();

        builder.Property(x => x.DiasGarantia)
            .IsRequired();

        builder.Property(x => x.ExistenciaMinimaPredeterminada)
            .IsRequired();

        builder.Property(x => x.EstadosProceso)
            .HasMaxLength(1000)
            .IsRequired();
    }
}