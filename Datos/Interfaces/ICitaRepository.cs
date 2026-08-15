using Datos.Models;

namespace Datos.Interfaces;

public interface ICitaRepository : IRepository<Cita>
{
    Task<IEnumerable<Cita>> ObtenerAgendaAsync(
        DateTime? fechaInicio,
        DateTime? fechaFin);

    Task<Cita?> ObtenerCompletaAsync(
        int id);

    Task<bool> ExisteConflictoAsync(
        DateTime fechaInicio,
        DateTime fechaFin,
        int? idEmpleado,
        string? area,
        int? idCitaExcluir = null);
}