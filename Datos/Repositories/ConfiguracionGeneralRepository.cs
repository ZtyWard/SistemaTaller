using Datos.Context;
using Datos.Interfaces;
using Datos.Models;

namespace Datos.Repositories;

public class ConfiguracionGeneralRepository
    : Repository<ConfiguracionGeneral>,
      IConfiguracionGeneralRepository
{
    public ConfiguracionGeneralRepository(
        SistemaTallerDbContext context)
        : base(context)
    {
    }
}