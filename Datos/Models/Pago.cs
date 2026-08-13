using System;

namespace Datos.Models;

public class Pago
{
    public int IdPago { get; set; }

    public int IdFactura { get; set; }

    public decimal Monto { get; set; }

    public string FormaPago { get; set; } = string.Empty;

    public string? NumeroReferencia { get; set; }

    public DateTime FechaPago { get; set; }

    public string? UsuarioId { get; set; }

    public string? Observaciones { get; set; }

    // =====================================================
    // NAVEGACIÓN
    // =====================================================

    public Factura? Factura { get; set; }
}