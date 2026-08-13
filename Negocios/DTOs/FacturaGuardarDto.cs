using System.ComponentModel.DataAnnotations;

namespace Negocios.DTOs;

public class FacturaGuardarDto
{
    [Required]
    [StringLength(30)]
    public string NumeroFactura { get; set; }
        = string.Empty;

    public int? IdCliente { get; set; }

    public int? IdOrdenTrabajo { get; set; }

    public int? IdVenta { get; set; }

    [Required]
    public DateTime FechaEmision { get; set; }
        = DateTime.Now;

    [Range(0, double.MaxValue)]
    public decimal Subtotal { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Impuesto { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Descuento { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Total { get; set; }

    public string Estado { get; set; }
        = "Pendiente";
}