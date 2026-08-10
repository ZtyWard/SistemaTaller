using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class Cotizacion
{
    public int IdCotizacion { get; set; }

    public int IdDiagnostico { get; set; }

    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = "Pendiente";

    public Diagnostico? Diagnostico { get; set; }

    public ICollection<OrdenTrabajo> OrdenesTrabajo { get; set; }
        = new List<OrdenTrabajo>();
}
