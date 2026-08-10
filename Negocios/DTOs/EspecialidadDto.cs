namespace Negocios.DTOs;

public class EspecialidadDto
{
    public int IdEspecialidad { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}