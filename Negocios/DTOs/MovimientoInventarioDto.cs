namespace Negocios.DTOs;

public class MovimientoInventarioDto
{
    public int IdMovimiento { get; set; }

    public int IdProducto { get; set; }

    public string Producto { get; set; } =
        string.Empty;

    public string TipoMovimiento { get; set; } =
        string.Empty;

    public int Cantidad { get; set; }

    public DateTime FechaMovimiento { get; set; }

    public string? Observacion { get; set; }
}