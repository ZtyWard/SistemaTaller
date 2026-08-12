namespace Negocios.DTOs;

public class DetalleVentaDto
{
    public int IdDetalleVenta { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public string Producto { get; set; } =
        string.Empty;

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Subtotal { get; set; }
}