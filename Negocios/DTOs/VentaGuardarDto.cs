namespace Negocios.DTOs;

public class VentaGuardarDto
{
    public string NumeroVenta { get; set; } = string.Empty;

    public int? IdCliente { get; set; }

    public int IdVendedor { get; set; }

    public int? IdCajero { get; set; }

    public DateTime FechaVenta { get; set; }

    // =====================================================
    // COMPATIBILIDAD CON LAS VISTAS FUTURAS
    // =====================================================

    public decimal Total { get; set; }

    public string Estado { get; set; } =
        "Pendiente";

    // =====================================================
    // DATOS ADICIONALES
    // =====================================================

    public string? FormaPago { get; set; }

    public string? UsuarioId { get; set; }

    // =====================================================
    // DETALLES
    // =====================================================

    public List<DetalleVentaGuardarDto> Detalles { get; set; }
        = new List<DetalleVentaGuardarDto>();
}