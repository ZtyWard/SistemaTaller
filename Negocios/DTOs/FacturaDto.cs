namespace Negocios.DTOs;

public class FacturaDto
{
    public int IdFactura { get; set; }

    public string NumeroFactura { get; set; } = string.Empty;

    public int? IdCliente { get; set; }

    public string? Cliente { get; set; }

    public int? IdOrdenTrabajo { get; set; }

    public int? IdVenta { get; set; }

    public DateTime FechaEmision { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = string.Empty;

    public decimal TotalPagado { get; set; }

    public decimal SaldoPendiente { get; set; }
}