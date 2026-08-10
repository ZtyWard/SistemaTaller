using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class Compra
{
    public int IdCompra { get; set; }

    public int IdProveedor { get; set; }

    public DateTime FechaCompra { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = "Pendiente";

    public Proveedor? Proveedor { get; set; }
}
