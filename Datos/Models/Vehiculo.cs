using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Models;

public class Vehiculo
{
    public int IdVehiculo { get; set; }

    public int IdCliente { get; set; }
    public int IdMarca { get; set; }
    public int IdModelo { get; set; }
    public int IdTipoVehiculo { get; set; }
    public int IdTipoCombustible { get; set; }

    public string Placa { get; set; } = string.Empty;
    public string? VIN { get; set; }
    public string? Color { get; set; }
    public int? Anio { get; set; }
    public int? Kilometraje { get; set; }
    public bool Activo { get; set; } = true;

    public Cliente? Cliente { get; set; }
    public Marca? Marca { get; set; }
    public Modelo? Modelo { get; set; }
    public TipoVehiculo? TipoVehiculo { get; set; }
    public TipoCombustible? TipoCombustible { get; set; }

    public ICollection<Recepcion> Recepciones { get; set; } = new List<Recepcion>();
}
