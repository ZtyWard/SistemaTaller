using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IRolService
{
    Task<IEnumerable<RolAdministracionDto>>
        ObtenerTodosAsync();

    Task<RolAdministracionDto?>
        ObtenerPorIdAsync(string id);

    Task<(bool Exitoso, string Mensaje)>
        CrearAsync(
            RolAdministracionDto dto,
            bool administrarPermisos);

    Task<(bool Exitoso, string Mensaje)>
        ActualizarAsync(
            RolAdministracionDto dto,
            bool administrarPermisos);

    Task<(bool Exitoso, string Mensaje)>
        EliminarAsync(string id);

    Task<IEnumerable<string>>
        ObtenerPermisosDisponiblesAsync();
}