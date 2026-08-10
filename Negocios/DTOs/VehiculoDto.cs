using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class VehiculoDto
{
    public int IdVehiculo { get; set; }

    public int IdCliente { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;

    public int IdMarca { get; set; }
    public string MarcaNombre { get; set; } = string.Empty;

    public int IdModelo { get; set; }
    public string ModeloNombre { get; set; } = string.Empty;

    public int IdTipoVehiculo { get; set; }
    public string TipoVehiculoNombre { get; set; } = string.Empty;

    public int IdTipoCombustible { get; set; }
    public string TipoCombustibleNombre { get; set; } = string.Empty;

    public string Placa { get; set; } = string.Empty;
    public string? VIN { get; set; }
    public string? Color { get; set; }
    public int? Anio { get; set; }
    public int? Kilometraje { get; set; }
    public bool Activo { get; set; }
}
