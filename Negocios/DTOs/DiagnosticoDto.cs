using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class DiagnosticoDto
{
    public int IdDiagnostico { get; set; }

    public int IdRecepcion { get; set; }
    public string Placa { get; set; } = string.Empty;

    public int IdEmpleado { get; set; }
    public string EmpleadoNombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaDiagnostico { get; set; }

    public string EstadoRecepcion { get; set; } = string.Empty;
}
