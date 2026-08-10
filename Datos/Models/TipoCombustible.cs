using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class TipoCombustible
{
    public int IdTipoCombustible { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
