using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Negocios.DTOs;

public class MovimientoInventarioGuardarDto
{
    public int IdProducto { get; set; }

    public string TipoMovimiento { get; set; } =
        string.Empty;

    public int Cantidad { get; set; }

    public string? Observacion { get; set; }
}
