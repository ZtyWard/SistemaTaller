namespace Negocios.DTOs;

public class ServicioGuardarDto
{
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int? DuracionEstimada { get; set; }

    public bool Activo { get; set; } = true;
}