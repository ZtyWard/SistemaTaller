namespace Negocios.DTOs;

public class DetalleVentaGuardarDto
{
    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }
}