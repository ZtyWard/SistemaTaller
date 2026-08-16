namespace Negocios.DTOs;

public class AuditoriaDto
{
    public long IdAuditoria { get; set; }

    public string? UsuarioId { get; set; }

    public DateTime Fecha { get; set; }

    public string Modulo { get; set; } = string.Empty;

    public string Accion { get; set; } = string.Empty;

    public string? RegistroId { get; set; }

    public string? Descripcion { get; set; }

    public string? Ip { get; set; }
}