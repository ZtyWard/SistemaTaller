using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class Marca
{
    public int IdMarca { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();
    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
