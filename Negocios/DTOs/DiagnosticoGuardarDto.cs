using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class DiagnosticoGuardarDto
{
    public int IdRecepcion { get; set; }
    public int IdEmpleado { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}
