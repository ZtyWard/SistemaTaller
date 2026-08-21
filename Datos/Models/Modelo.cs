using System.Collections.Generic;

namespace Datos.Models;

public class Modelo
{
    public int IdModelo { get; set; }

    // =====================================================
    // RELACIÓN CON MARCA
    // =====================================================

    public int IdMarca { get; set; }

    public Marca? Marca { get; set; }


    // =====================================================
    // INFORMACIÓN DEL MODELO
    // =====================================================

    public string Nombre { get; set; } = string.Empty;

    public int? AnioVehiculo { get; set; }


    // =====================================================
    // INFORMACIÓN DE LA API NHTSA
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


    // =====================================================
    // VEHÍCULOS
    // =====================================================

    public ICollection<Vehiculo> Vehiculos { get; set; }
        = new List<Vehiculo>();
}