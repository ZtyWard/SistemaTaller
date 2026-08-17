using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface INotificacionService
{
    Task<IEnumerable<NotificacionDto>> ObtenerTodasAsync();
}