namespace Negocios.DTOs;

public class ProductoGuardarDto
{
    public int IdCategoriaProducto { get; set; }

    public string Codigo { get; set; } =
        string.Empty;

    public string? CodigoBarras { get; set; }

    public string Nombre { get; set; } =
        string.Empty;

    public string? Descripcion { get; set; }

    public string? ImagenUrl { get; set; }

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int Stock { get; set; }

    public int StockMinimo { get; set; }

    public bool Activo { get; set; } = true;
}