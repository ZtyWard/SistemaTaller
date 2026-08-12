using System;
using System.Collections.Generic;

namespace Datos.Models;

public class Venta
{
    public int IdVenta { get; set; }

    public string NumeroVenta { get; set; } = string.Empty;

    public int? IdCliente { get; set; }

    public int IdVendedor { get; set; }

    public int? IdCajero { get; set; }

    public DateTime FechaVenta { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public string? FormaPago { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public string? UsuarioId { get; set; }

    // =====================================================
    // NAVEGACIONES
    // =====================================================

    public Cliente? Cliente { get; set; }

    public ICollection<DetalleVenta>
        Detalles
    { get; set; }
        = new List<DetalleVenta>();
}