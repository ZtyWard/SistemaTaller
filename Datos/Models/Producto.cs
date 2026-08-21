using System;
using System.Collections.Generic;

namespace Datos.Models;

public class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoriaProducto { get; set; }

    public string Codigo { get; set; } =
        string.Empty;

    public string Nombre { get; set; } =
        string.Empty;

    public string? Descripcion { get; set; }

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int Stock { get; set; }

    public int StockMinimo { get; set; } = 5;

    public bool Activo { get; set; } = true;

    // =====================================================
    // CÓDIGO DE BARRAS
    // =====================================================

    public string? CodigoBarras { get; set; }

    // =====================================================
    // IMAGEN DEL PRODUCTO
    // =====================================================

    public string? ImagenUrl { get; set; }

    // =====================================================
    // RELACIONES
    // =====================================================

    public CategoriaProducto? CategoriaProducto { get; set; }

    public ICollection<MovimientoInventario>
        MovimientosInventario
    { get; set; }
        = new List<MovimientoInventario>();
}