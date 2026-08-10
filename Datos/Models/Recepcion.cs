using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class Recepcion
{
    public int IdRecepcion { get; set; }

    public int IdVehiculo { get; set; }
    public int IdEmpleado { get; set; }

    public DateTime FechaRecepcion { get; set; }
    public int Kilometraje { get; set; }
    public string? NivelCombustible { get; set; }
    public string? Observaciones { get; set; }
    public string Estado { get; set; } = "Recibido";

    public Vehiculo? Vehiculo { get; set; }
    public Empleado? Empleado { get; set; }

    public ICollection<Diagnostico> Diagnosticos { get; set; }
        = new List<Diagnostico>();
}
