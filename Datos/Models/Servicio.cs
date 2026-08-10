using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class Servicio
{
    public int IdServicio { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int? DuracionEstimada { get; set; }
    public bool Activo { get; set; } = true;
}
