




namespace Negocios.DTOs;

public class ModeloGuardarDto
{
    // =====================================================
    // MARCA
    // =====================================================

    public int IdMarca { get; set; }


    // =====================================================
    // INFORMACIÓN DEL MODELO
    // =====================================================

    public string Nombre { get; set; } = string.Empty;

    public int? AnioVehiculo { get; set; }


    // =====================================================
    // INFORMACIÓN DE LA API
    // =====================================================

    public int? IdModeloApi { get; set; }

    public int? IdTipoVehiculoApi { get; set; }

    public string? NombreTipoVehiculoApi { get; set; }


    // =====================================================
    // IMAGEN
    // =====================================================

    public string? ImagenUrl { get; set; }

    public string? FuenteImagen { get; set; }


    // =====================================================
    // ESTADO
    // =====================================================

    public bool Activo { get; set; } = true;
}