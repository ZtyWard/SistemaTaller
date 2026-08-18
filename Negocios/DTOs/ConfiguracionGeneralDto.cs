using System.ComponentModel.DataAnnotations;

namespace Negocios.DTOs;

public class ConfiguracionGeneralDto
{
    public int IdConfiguracion { get; set; }

    [Required]
    [Display(Name = "Nombre del taller")]
    public string NombreTaller { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Identificación jurídica")]
    public string IdentificacionJuridica { get; set; } = string.Empty;

    [Required]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Display(Name = "Logo")]
    public string? LogoUrl { get; set; }

    [Range(0, 100)]
    [Display(Name = "Impuesto (%)")]
    public decimal ImpuestoPorcentaje { get; set; }

    [Required]
    public string Moneda { get; set; } = "CRC";

    [Range(0, 100)]
    [Display(Name = "Límite de descuento (%)")]
    public decimal LimiteDescuentoPorcentaje { get; set; }

    [Required]
    [Display(Name = "Prefijo de recepción")]
    public string PrefijoRecepcion { get; set; } = "REC";

    public int SiguienteRecepcion { get; set; }

    [Required]
    [Display(Name = "Prefijo de cotización")]
    public string PrefijoCotizacion { get; set; } = "COT";

    public int SiguienteCotizacion { get; set; }

    [Required]
    [Display(Name = "Prefijo de orden de trabajo")]
    public string PrefijoOrdenTrabajo { get; set; } = "OT";

    public int SiguienteOrdenTrabajo { get; set; }

    [Required]
    [Display(Name = "Prefijo de venta")]
    public string PrefijoVenta { get; set; } = "VEN";

    public int SiguienteVenta { get; set; }

    [Required]
    [Display(Name = "Prefijo de factura")]
    public string PrefijoFactura { get; set; } = "FAC";

    public int SiguienteFactura { get; set; }

    [Display(Name = "Hora de apertura")]
    public TimeSpan HoraApertura { get; set; }

    [Display(Name = "Hora de cierre")]
    public TimeSpan HoraCierre { get; set; }

    [Range(0, 3650)]
    [Display(Name = "Días de garantía")]
    public int DiasGarantia { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "Existencia mínima")]
    public int ExistenciaMinimaPredeterminada { get; set; }

    [Required]
    [Display(Name = "Estados de proceso")]
    public string EstadosProceso { get; set; } = string.Empty;
}