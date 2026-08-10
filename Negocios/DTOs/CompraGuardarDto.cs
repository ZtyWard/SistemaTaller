namespace Negocios.DTOs;

public class CompraGuardarDto
{
    public int IdProveedor { get; set; }

    public DateTime FechaCompra { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } =
        "Pendiente";
}