using System.ComponentModel.DataAnnotations;

namespace Datos.Models;

public class Garantia
{
    public int IdGarantia { get; set; }

    public int? IdOrdenTrabajo { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public int? IdServicio { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaVencimiento { get; set; }

    [MaxLength(30)]
    public string Estado { get; set; } = "Vigente";

    public string? Condiciones { get; set; }

    public string? Observaciones { get; set; }

    // =====================================================
    // RECLAMO
    // =====================================================

    public string? MotivoReclamo { get; set; }

    public DateTime? FechaReclamo { get; set; }

    // =====================================================
    // RESOLUCIÓN
    // =====================================================

    public string? Resolucion { get; set; }

    public DateTime? FechaResolucion { get; set; }

    // =====================================================
    // NAVEGACIÓN
    // =====================================================

    public OrdenTrabajo? OrdenTrabajo { get; set; }

    public Venta? Venta { get; set; }

    public Producto? Producto { get; set; }

    public Servicio? Servicio { get; set; }
}