namespace Negocios.DTOs;

public class DetalleCompraGuardarDto
{
    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }
}