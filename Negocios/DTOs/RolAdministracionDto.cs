namespace Negocios.DTOs;

public class RolAdministracionDto
{
    public string Id { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int CantidadUsuarios { get; set; }

    public List<string> Permisos { get; set; }
        = new();

    public List<string> PermisosDisponibles { get; set; }
        = new();
}