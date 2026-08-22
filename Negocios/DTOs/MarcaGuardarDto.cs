using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Negocios.DTOs;

public class MarcaGuardarDto
{
    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}
