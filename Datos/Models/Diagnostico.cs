using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class Diagnostico
{
    public int IdDiagnostico { get; set; }

    public int IdRecepcion { get; set; }
    public int IdEmpleado { get; set; }

    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaDiagnostico { get; set; }

    public Recepcion? Recepcion { get; set; }
    public Empleado? Empleado { get; set; }

    public ICollection<Cotizacion> Cotizaciones { get; set; }
        = new List<Cotizacion>();
}
