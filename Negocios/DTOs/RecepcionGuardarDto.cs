using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class RecepcionGuardarDto
{
    public int IdVehiculo { get; set; }
    public int IdEmpleado { get; set; }

    public int Kilometraje { get; set; }
    public string? NivelCombustible { get; set; }
    public string? Observaciones { get; set; }
}
