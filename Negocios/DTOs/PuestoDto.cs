namespace Negocios.DTOs;

public class PuestoDto
{
    public int IdPuesto { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}