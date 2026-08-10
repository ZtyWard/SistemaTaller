using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Negocios.DTOs;

public class CotizacionGuardarDto
{
    public int IdDiagnostico { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = "Pendiente";
}
