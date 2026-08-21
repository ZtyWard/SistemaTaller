using System.Collections.Generic;

namespace Datos.Models;

public class Modelo
{
    public int IdModelo { get; set; }

    public int IdMarca { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? ImagenUrl { get; set; }

    public string? FuenteImagen { get; set; }

    public bool Activo { get; set; } = true;

    public Marca? Marca { get; set; }

    public ICollection<Vehiculo> Vehiculos { get; set; }
        = new List<Vehiculo>();
}