using Datos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Configurations;

public class ModeloConfiguration : IEntityTypeConfiguration<Modelo>
{
    public void Configure(EntityTypeBuilder<Modelo> builder)
    {
        // =====================================================
        // TABLA
        // =====================================================

        builder.ToTable("Modelo");


        // =====================================================
        // CLAVE PRIMARIA
        // =====================================================

        builder.HasKey(x => x.IdModelo);


        // =====================================================
        // MARCA
        // =====================================================

        builder.Property(x => x.IdMarca)
            .IsRequired();


        // =====================================================
        // NOMBRE DEL MODELO
        // =====================================================

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();


        // =====================================================
        // AÑO DEL VEHÍCULO
        // =====================================================

        builder.Property(x => x.AnioVehiculo)
            .IsRequired(false);


        // =====================================================
        // INFORMACIÓN API NHTSA
        // =====================================================

        builder.Property(x => x.IdModeloApi)
            .IsRequired(false);

        builder.Property(x => x.IdTipoVehiculoApi)
            .IsRequired(false);

        builder.Property(x => x.NombreTipoVehiculoApi)
            .HasMaxLength(100)
            .IsRequired(false);


        // =====================================================
        // IMAGEN
        // =====================================================

        builder.Property(x => x.ImagenUrl)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.FuenteImagen)
            .HasMaxLength(500)
            .IsRequired(false);


        // =====================================================
        // ESTADO
        // =====================================================

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);


        // =====================================================
        // ÍNDICE ÚNICO
        // =====================================================
        //
        // Un mismo modelo puede existir para diferentes años.
        //
        // Toyota | Corolla | 2020
        // Toyota | Corolla | 2021
        //
        // son registros válidos.
        //
        // =====================================================

        builder.HasIndex(x => new
        {
            x.IdMarca,
            x.Nombre,
            x.AnioVehiculo
        })
        .IsUnique();


        // =====================================================
        // RELACIÓN CON MARCA
        // =====================================================

        builder.HasOne(x => x.Marca)
            .WithMany(x => x.Modelos)
            .HasForeignKey(x => x.IdMarca)
            .OnDelete(DeleteBehavior.Restrict);


        // =====================================================
        // RELACIÓN CON VEHÍCULOS
        // =====================================================

        builder.HasMany(x => x.Vehiculos)
            .WithOne(x => x.Modelo)
            .HasForeignKey(x => x.IdModelo)
            .OnDelete(DeleteBehavior.Restrict);
    }
}