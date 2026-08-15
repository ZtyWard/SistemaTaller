using System.ComponentModel.DataAnnotations;

namespace Negocios.DTOs;

public class EntregaDto
{
    public int IdEntrega { get; set; }

    public int IdOrdenTrabajo { get; set; }

    public string Placa { get; set; } = string.Empty;

    public string ClienteNombre { get; set; } = string.Empty;

    public DateTime FechaEntrega { get; set; }

    public int KilometrajeSalida { get; set; }

    public string PersonaRecibe { get; set; } = string.Empty;

    public string? Observaciones { get; set; }

    public string? Recomendaciones { get; set; }

    public bool TieneGarantia { get; set; }

    public string EstadoPago { get; set; } = "Pendiente";

    public bool Aceptacion { get; set; }

    public string? FirmaNombre { get; set; }

    public DateTime? FechaAceptacion { get; set; }
}

// =====================================================
// DTO PARA CREAR / EDITAR ENTREGA
// =====================================================

public class EntregaGuardarDto
{
    [Required(ErrorMessage = "La orden de trabajo es obligatoria.")]
    [Display(Name = "Orden de trabajo")]
    public int IdOrdenTrabajo { get; set; }

    [Range(
        0,
        int.MaxValue,
        ErrorMessage = "El kilometraje no puede ser negativo.")]
    [Display(Name = "Kilometraje de salida")]
    public int KilometrajeSalida { get; set; }

    [Required(ErrorMessage = "Debe indicar quién recibe el vehículo.")]
    [StringLength(
        200,
        ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
    [Display(Name = "Persona que recibe")]
    public string PersonaRecibe { get; set; } = string.Empty;

    [StringLength(
        500,
        ErrorMessage = "Las observaciones no pueden superar los 500 caracteres.")]
    public string? Observaciones { get; set; }

    [StringLength(
        1000,
        ErrorMessage = "Las recomendaciones no pueden superar los 1000 caracteres.")]
    public string? Recomendaciones { get; set; }

    [Display(Name = "Tiene garantía")]
    public bool TieneGarantia { get; set; }

    [Required(ErrorMessage = "Debe indicar el estado de pago.")]
    [StringLength(30)]
    [Display(Name = "Estado de pago")]
    public string EstadoPago { get; set; } = "Pendiente";

    [Display(Name = "Aceptación de entrega")]
    public bool Aceptacion { get; set; }

    [StringLength(
        200,
        ErrorMessage = "El nombre de firma no puede superar los 200 caracteres.")]
    [Display(Name = "Nombre de quien acepta")]
    public string? FirmaNombre { get; set; }
}