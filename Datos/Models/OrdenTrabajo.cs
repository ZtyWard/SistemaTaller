using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class OrdenTrabajo
{
    public int IdOrdenTrabajo { get; set; }

    public int IdCotizacion { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string Estado { get; set; } = "Abierta";
    public string? Observaciones { get; set; }

    public Cotizacion? Cotizacion { get; set; }
}
