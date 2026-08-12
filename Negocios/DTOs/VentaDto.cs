namespace Negocios.DTOs;

public class VentaDto
{
    public int IdVenta { get; set; }

    public string NumeroVenta { get; set; } =
        string.Empty;

    public int? IdCliente { get; set; }

    public string Cliente { get; set; } =
        string.Empty;

    public int IdVendedor { get; set; }

    public int? IdCajero { get; set; }

    public DateTime FechaVenta { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public string? FormaPago { get; set; }

    public string Estado { get; set; } =
        string.Empty;

    public string? UsuarioId { get; set; }

    public List<DetalleVentaDto> Detalles { get; set; }
        = new List<DetalleVentaDto>();
}