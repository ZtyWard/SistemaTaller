namespace Negocios.DTOs;

public class TipoCombustibleDto
{
    public int IdTipoCombustible { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; }
}