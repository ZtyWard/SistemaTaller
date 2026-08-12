using System;
using System.Collections.Generic;

namespace Datos.Models;

public class Compra
{
    public int IdCompra { get; set; }

    public int IdProveedor { get; set; }

    public DateTime FechaCompra { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = "Pendiente";

    // =====================================================
    // CAMPOS EXISTENTES EN SQL SERVER
    // =====================================================

    public string? NumeroFacturaProveedor { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public string? FormaPago { get; set; }

    public string? UsuarioId { get; set; }

    // =====================================================
    // NAVEGACIONES
    // =====================================================

    public Proveedor? Proveedor { get; set; }

    public ICollection<DetalleCompra>
        Detalles
    { get; set; }
        = new List<DetalleCompra>();
}