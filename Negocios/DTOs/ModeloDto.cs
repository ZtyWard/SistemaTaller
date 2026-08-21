namespace Negocios.DTOs;

public class ModeloDto
{
    public int IdModelo { get; set; }

    public int IdMarca { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string MarcaNombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}