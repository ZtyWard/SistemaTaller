namespace Negocios.DTOs;

public class VehiculoApiResultadoDto
{
    public string Marca { get; set; } = string.Empty;

    public string Modelo { get; set; } = string.Empty;

    public string? ImagenUrl { get; set; }

    public string? FuenteImagen { get; set; }

    public string? Descripcion { get; set; }
}