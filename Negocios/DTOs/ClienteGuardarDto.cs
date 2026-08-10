using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class ClienteGuardarDto
{
    public string Cedula { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido1 { get; set; } = string.Empty;
    public string? Apellido2 { get; set; }
    public string Telefono { get; set; } = string.Empty;
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
}
