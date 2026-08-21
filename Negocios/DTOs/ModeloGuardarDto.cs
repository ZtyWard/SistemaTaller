namespace Negocios.DTOs;

public class ModeloGuardarDto
{
    public int IdMarca { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? ImagenUrl { get; set; }

    public string? FuenteImagen { get; set; }

    public bool Activo { get; set; } = true;
}