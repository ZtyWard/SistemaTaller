using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface ICitaService
{
    Task<IEnumerable<CitaDto>> ObtenerAgendaAsync(
        DateTime? fechaInicio,
        DateTime? fechaFin);

    Task<CitaDto?> ObtenerPorIdAsync(int id);

    Task<(bool Exitoso, string Mensaje)>
        CrearAsync(CitaFormularioDto dto);

    Task<(bool Exitoso, string Mensaje)>
        ActualizarAsync(
            int id,
            CitaFormularioDto dto);

    Task<(bool Exitoso, string Mensaje)>
        CancelarAsync(int id);
}