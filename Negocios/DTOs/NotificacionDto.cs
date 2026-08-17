namespace Negocios.DTOs;

public class NotificacionDto
{
    public string Tipo { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string Mensaje { get; set; } = string.Empty;

    public string Severidad { get; set; } = "info";

    public string Icono { get; set; } = "◐";

    public DateTime Fecha { get; set; }

    public string Url { get; set; } = "/";
}