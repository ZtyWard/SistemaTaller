using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios.DTOs;

public class VehiculoGuardarDto
{
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
}
