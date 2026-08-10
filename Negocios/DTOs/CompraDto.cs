namespace Negocios.DTOs;

public class CompraDto
{
    public int IdCompra { get; set; }

    public int IdProveedor { get; set; }

    public string Proveedor { get; set; } =
        string.Empty;

    public DateTime FechaCompra { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } =
        string.Empty;
}