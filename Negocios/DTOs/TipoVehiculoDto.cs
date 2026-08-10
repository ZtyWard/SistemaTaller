namespace Negocios.DTOs;

public class TipoVehiculoDto
{
    public int IdTipoVehiculo { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}