namespace Negocios.DTOs;

public class ModeloGuardarDto
{
    public int IdMarca { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}