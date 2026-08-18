using Negocios.DTOs;

namespace Negocios.Interfaces;

public interface IConfiguracionGeneralService
{
    Task<ConfiguracionGeneralDto?> ObtenerAsync();

    Task GuardarAsync(
        ConfiguracionGeneralDto dto);
}