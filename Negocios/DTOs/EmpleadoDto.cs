using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class EmpleadoDto
{
    public int IdEmpleado { get; set; }

    public string Cedula { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public int IdPuesto { get; set; }

    public string Puesto { get; set; } = string.Empty;

    public int IdEspecialidad { get; set; }

    public string Especialidad { get; set; } = string.Empty;

    public decimal? Salario { get; set; }

    public bool Activo { get; set; }
}
