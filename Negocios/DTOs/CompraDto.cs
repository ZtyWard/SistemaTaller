namespace Negocios.DTOs;

public class CompraDto
{
    public int IdCompra { get; set; }

    public int IdProveedor { get; set; }

    public string Proveedor { get; set; } =
        string.Empty;

    public DateTime FechaCompra { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } =
        string.Empty;

    public string? NumeroFacturaProveedor { get; set; }

    public string? FormaPago { get; set; }

    public string? UsuarioId { get; set; }

    public List<DetalleCompraDto> Detalles { get; set; }
        = new List<DetalleCompraDto>();
}