namespace Negocios.DTOs;

public class OrdenTrabajoDto
{
    public int IdOrdenTrabajo { get; set; }

    public int IdCotizacion { get; set; }

    public string Placa { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public string Estado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }

    public decimal TotalCotizacion { get; set; }
}