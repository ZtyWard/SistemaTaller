using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class RecepcionDto
{
    public int IdRecepcion { get; set; }

    public int IdVehiculo { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string VehiculoDescripcion { get; set; } = string.Empty;

    public int IdEmpleado { get; set; }
    public string EmpleadoNombre { get; set; } = string.Empty;

    public DateTime FechaRecepcion { get; set; }
    public int Kilometraje { get; set; }
    public string? NivelCombustible { get; set; }
    public string? Observaciones { get; set; }
    public string Estado { get; set; } = string.Empty;
}
