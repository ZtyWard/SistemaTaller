namespace Negocios.DTOs;

public class GarantiaDto
{
    public int IdGarantia { get; set; }

    public int? IdOrdenTrabajo { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public int? IdServicio { get; set; }

    public string? ProductoNombre { get; set; }

    public string? ServicioNombre { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string? Condiciones { get; set; }

    public string? Observaciones { get; set; }

    public string? MotivoReclamo { get; set; }

    public DateTime? FechaReclamo { get; set; }

    public string? Resolucion { get; set; }

    public DateTime? FechaResolucion { get; set; }

    public bool EstaVigente =>
        Estado == "Vigente" &&
        FechaVencimiento.Date >= DateTime.Today;

    public bool EstaVencida =>
        FechaVencimiento.Date < DateTime.Today &&
        Estado == "Vigente";
}