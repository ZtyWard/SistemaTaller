using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class ProveedorGuardarDto
{
    public string Nombre { get; set; } =
        string.Empty;

    public string? CedulaJuridica { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public string? Direccion { get; set; }

    public bool Activo { get; set; } = true;
}
