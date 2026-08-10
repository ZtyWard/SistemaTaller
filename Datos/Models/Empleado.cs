using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class Empleado
{
    public int IdEmpleado { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido1 { get; set; } = string.Empty;
    public string? Apellido2 { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }

    public int IdPuesto { get; set; }
    public int IdEspecialidad { get; set; }

    public decimal? Salario { get; set; }
    public bool Activo { get; set; } = true;

    public Puesto? Puesto { get; set; }
    public Especialidad? Especialidad { get; set; }

    public ICollection<Recepcion> Recepciones { get; set; } = new List<Recepcion>();
    public ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();
}
