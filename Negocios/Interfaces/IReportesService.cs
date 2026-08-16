using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IReportesService
{
    Task<ReportesDto> ObtenerAsync(
        DateTime? desde,
        DateTime? hasta,
        string? placa);
}