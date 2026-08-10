namespace Negocios.DTOs;

public class CotizacionDto
{
    public int IdCotizacion { get; set; }

    public int IdDiagnostico { get; set; }
    public string Placa { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;

    public bool TieneOrdenTrabajo { get; set; }
}