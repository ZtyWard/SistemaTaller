using Datos.Interfaces;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _repository;

    public AuditoriaService(
        IAuditoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AuditoriaDto>> ObtenerAsync(
        string? usuarioId = null,
        string? modulo = null,
        string? accion = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null,
        string? registroId = null)
    {
        var registros =
            await _repository.ObtenerAsync(
                usuarioId,
                modulo,
                accion,
                fechaDesde,
                fechaHasta,
                registroId);

        return registros.Select(x => new AuditoriaDto
        {
            IdAuditoria = x.IdAuditoria,
            UsuarioId = x.UsuarioId,
            Fecha = x.Fecha,
            Modulo = x.Modulo,
            Accion = x.Accion,
            RegistroId = x.RegistroId,
            Descripcion = x.Descripcion,
            Ip = x.Ip
        });
    }
}