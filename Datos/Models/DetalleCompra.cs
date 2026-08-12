using System;

namespace Datos.Models;

public class DetalleCompra
{
    public int IdDetalleCompra { get; set; }

    public int IdCompra { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Subtotal { get; set; }

    // =====================================================
    // NAVEGACIONES
    // =====================================================

    public Compra? Compra { get; set; }

    public Producto? Producto { get; set; }
}