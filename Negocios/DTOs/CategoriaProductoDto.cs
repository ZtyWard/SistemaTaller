namespace Negocios.DTOs;

public class CategoriaProductoDto
{
    public int IdCategoriaProducto { get; set; }

    public string Nombre { get; set; } =
        string.Empty;

    public bool Activo { get; set; }
}