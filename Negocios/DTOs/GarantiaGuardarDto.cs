using System.ComponentModel.DataAnnotations;

namespace Negocios.DTOs;

public class GarantiaGuardarDto
{
    public int? IdOrdenTrabajo { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public int? IdServicio { get; set; }

    [Required]
    public DateTime FechaInicio { get; set; }
        = DateTime.Today;

    [Required]
    public DateTime FechaVencimiento { get; set; }
        = DateTime.Today;

    public string? Condiciones { get; set; }

    public string? Observaciones { get; set; }
}