using Azure;
using System;
using System.Collections.Generic;

namespace Datos.Models;

public class Factura
{
    public int IdFactura { get; set; }

    public string NumeroFactura { get; set; } = string.Empty;

    public int? IdCliente { get; set; }

    public int? IdOrdenTrabajo { get; set; }

    public int? IdVenta { get; set; }

    public DateTime FechaEmision { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public string? UsuarioId { get; set; }

    // =====================================================
    // NAVEGACIONES
    // =====================================================

    public Cliente? Cliente { get; set; }

    public OrdenTrabajo? OrdenTrabajo { get; set; }

    public Venta? Venta { get; set; }

    public ICollection<Pago> Pagos { get; set; }
        = new List<Pago>();
}