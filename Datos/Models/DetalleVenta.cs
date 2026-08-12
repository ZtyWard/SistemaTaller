using System;

namespace Datos.Models;

public class DetalleVenta
{
    public int IdDetalleVenta { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Subtotal { get; set; }

    // =====================================================
    // NAVEGACIONES
    // =====================================================

    public Venta? Venta { get; set; }

    public Producto? Producto { get; set; }
}