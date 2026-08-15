using System.ComponentModel.DataAnnotations;

namespace Datos.Models;

public class Entrega
{
    public int IdEntrega { get; set; }

    public int IdOrdenTrabajo { get; set; }

    public DateTime FechaEntrega { get; set; }

    public int KilometrajeSalida { get; set; }

    [MaxLength(200)]
    public string PersonaRecibe { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Observaciones { get; set; }

    [MaxLength(1000)]
    public string? Recomendaciones { get; set; }

    public bool TieneGarantia { get; set; }

    [MaxLength(30)]
    public string EstadoPago { get; set; } = "Pendiente";

    public bool Aceptacion { get; set; }

    [MaxLength(200)]
    public string? FirmaNombre { get; set; }

    public DateTime? FechaAceptacion { get; set; }

    // =====================================================
    // NAVEGACIÓN
    // =====================================================

    public OrdenTrabajo? OrdenTrabajo { get; set; }
}