namespace Negocios.DTOs;

public class ProductoDto
{
    public int IdProducto { get; set; }

    public int IdCategoriaProducto { get; set; }

    public string Categoria { get; set; } =
        string.Empty;

    public string Codigo { get; set; } =
        string.Empty;

    public string Nombre { get; set; } =
        string.Empty;

    public string? Descripcion { get; set; }

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int Stock { get; set; }

    public int StockMinimo { get; set; }

    public bool Activo { get; set; }

    // =====================================================
    // CÓDIGO DE BARRAS
    // =====================================================

    public string? CodigoBarras { get; set; }

    // =====================================================
    // IMAGEN
    // =====================================================

    public string? ImagenUrl { get; set; }
}