namespace Negocios.DTOs;

public class PagoDto
{
    public int IdPago { get; set; }

    public int IdFactura { get; set; }

    public string NumeroFactura { get; set; }
        = string.Empty;

    public string? Cliente { get; set; }

    public decimal Monto { get; set; }

    public string FormaPago { get; set; }
        = string.Empty;

    public string? NumeroReferencia { get; set; }

    public DateTime FechaPago { get; set; }

    public string? UsuarioId { get; set; }

    public string? Observaciones { get; set; }

    public decimal TotalFactura { get; set; }

    public decimal TotalPagado { get; set; }

    public decimal SaldoPendiente { get; set; }

    public string EstadoFactura { get; set; }
        = string.Empty;
}