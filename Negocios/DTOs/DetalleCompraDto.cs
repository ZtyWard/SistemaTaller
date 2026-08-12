namespace Negocios.DTOs;

public class DetalleCompraDto
{
    public int IdDetalleCompra { get; set; }

    public int IdCompra { get; set; }

    public int IdProducto { get; set; }

    public string Producto { get; set; } =
        string.Empty;

    public int Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Subtotal { get; set; }
}