namespace Negocios.DTOs;

public class CompraGuardarDto
{
    public int IdProveedor { get; set; }

    public DateTime FechaCompra { get; set; }

    // =====================================================
    // COMPATIBILIDAD CON LAS VISTAS ACTUALES
    // =====================================================
    //
    // Este campo todavía es utilizado por Create.cshtml,
    // Edit.cshtml y CompraController.
    //
    // IMPORTANTE:
    // El CompraService NO utiliza este valor para calcular
    // la compra. El total real se calcula en el backend
    // a partir de los detalles.
    //
    public decimal Total { get; set; }

    public string Estado { get; set; } =
        "Pendiente";

    // =====================================================
    // DATOS ADICIONALES DE COMPRA
    // =====================================================

    public string? NumeroFacturaProveedor { get; set; }

    public string? FormaPago { get; set; }

    public string? UsuarioId { get; set; }

    // =====================================================
    // DETALLES
    // =====================================================

    public List<DetalleCompraGuardarDto> Detalles { get; set; }
        = new List<DetalleCompraGuardarDto>();
}