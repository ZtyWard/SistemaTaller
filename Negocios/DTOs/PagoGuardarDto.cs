using System.ComponentModel.DataAnnotations;

namespace Negocios.DTOs;

public class PagoGuardarDto
{
    [Required]
    public int IdFactura { get; set; }

    public decimal Monto { get; set; }

    [Required]
    [StringLength(30)]
    public string FormaPago { get; set; }
        = string.Empty;

    [StringLength(100)]
    public string? NumeroReferencia { get; set; }

    [StringLength(500)]
    public string? Observaciones { get; set; }
}