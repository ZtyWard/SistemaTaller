namespace Negocios.DTOs;

public class CategoriaProductoGuardarDto
{
    public string Nombre { get; set; } =
        string.Empty;

    public bool Activo { get; set; } = true;
}