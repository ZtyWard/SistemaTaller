using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> ObtenerDashboardAsync(
        string rolUsuario);
}