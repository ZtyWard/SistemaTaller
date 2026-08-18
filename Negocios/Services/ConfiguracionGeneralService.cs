using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class ConfiguracionGeneralService
    : IConfiguracionGeneralService
{
    private readonly IConfiguracionGeneralRepository _repository;

    public ConfiguracionGeneralService(
        IConfiguracionGeneralRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConfiguracionGeneralDto?> ObtenerAsync()
    {
        var configuracion =
            (await _repository.ObtenerTodosAsync())
            .FirstOrDefault();

        return configuracion == null
            ? null
            : MapearDto(configuracion);
    }

    public async Task GuardarAsync(
        ConfiguracionGeneralDto dto)
    {
        Validar(dto);

        var configuracion =
            (await _repository.ObtenerTodosAsync())
            .FirstOrDefault();

        if (configuracion == null)
        {
            configuracion = new ConfiguracionGeneral();

            MapearEntidad(
                configuracion,
                dto);

            await _repository.AgregarAsync(
                configuracion);
        }
        else
        {
            MapearEntidad(
                configuracion,
                dto);

            _repository.Actualizar(
                configuracion);
        }

        await _repository.GuardarCambiosAsync();
    }

    private static void Validar(
        ConfiguracionGeneralDto dto)
    {
        if (string.IsNullOrWhiteSpace(
                dto.NombreTaller))
        {
            throw new ArgumentException(
                "El nombre del taller es obligatorio.");
        }

        if (dto.HoraCierre <= dto.HoraApertura)
        {
            throw new ArgumentException(
                "La hora de cierre debe ser posterior a la hora de apertura.");
        }

        if (dto.DiasGarantia < 0)
        {
            throw new ArgumentException(
                "Los días de garantía no pueden ser negativos.");
        }
    }

    private static ConfiguracionGeneralDto
        MapearDto(
            ConfiguracionGeneral entidad)
    {
        return new ConfiguracionGeneralDto
        {
            IdConfiguracion =
                entidad.IdConfiguracion,

            NombreTaller =
                entidad.NombreTaller,

            IdentificacionJuridica =
                entidad.IdentificacionJuridica,

            Direccion =
                entidad.Direccion,

            Telefono =
                entidad.Telefono,

            Correo =
                entidad.Correo,

            LogoUrl =
                entidad.LogoUrl,

            ImpuestoPorcentaje =
                entidad.ImpuestoPorcentaje,

            Moneda =
                entidad.Moneda,

            LimiteDescuentoPorcentaje =
                entidad.LimiteDescuentoPorcentaje,

            PrefijoRecepcion =
                entidad.PrefijoRecepcion,

            SiguienteRecepcion =
                entidad.SiguienteRecepcion,

            PrefijoCotizacion =
                entidad.PrefijoCotizacion,

            SiguienteCotizacion =
                entidad.SiguienteCotizacion,

            PrefijoOrdenTrabajo =
                entidad.PrefijoOrdenTrabajo,

            SiguienteOrdenTrabajo =
                entidad.SiguienteOrdenTrabajo,

            PrefijoVenta =
                entidad.PrefijoVenta,

            SiguienteVenta =
                entidad.SiguienteVenta,

            PrefijoFactura =
                entidad.PrefijoFactura,

            SiguienteFactura =
                entidad.SiguienteFactura,

            HoraApertura =
                entidad.HoraApertura,

            HoraCierre =
                entidad.HoraCierre,

            DiasGarantia =
                entidad.DiasGarantia,

            ExistenciaMinimaPredeterminada =
                entidad.ExistenciaMinimaPredeterminada,

            EstadosProceso =
                entidad.EstadosProceso
        };
    }

    private static void MapearEntidad(
        ConfiguracionGeneral entidad,
        ConfiguracionGeneralDto dto)
    {
        entidad.NombreTaller =
            dto.NombreTaller.Trim();

        entidad.IdentificacionJuridica =
            dto.IdentificacionJuridica.Trim();

        entidad.Direccion =
            dto.Direccion.Trim();

        entidad.Telefono =
            dto.Telefono.Trim();

        entidad.Correo =
            dto.Correo.Trim();

        entidad.LogoUrl =
            string.IsNullOrWhiteSpace(dto.LogoUrl)
                ? null
                : dto.LogoUrl.Trim();

        entidad.ImpuestoPorcentaje =
            dto.ImpuestoPorcentaje;

        entidad.Moneda =
            dto.Moneda.Trim().ToUpperInvariant();

        entidad.LimiteDescuentoPorcentaje =
            dto.LimiteDescuentoPorcentaje;

        entidad.PrefijoRecepcion =
            dto.PrefijoRecepcion.Trim().ToUpperInvariant();

        entidad.SiguienteRecepcion =
            dto.SiguienteRecepcion;

        entidad.PrefijoCotizacion =
            dto.PrefijoCotizacion.Trim().ToUpperInvariant();

        entidad.SiguienteCotizacion =
            dto.SiguienteCotizacion;

        entidad.PrefijoOrdenTrabajo =
            dto.PrefijoOrdenTrabajo.Trim().ToUpperInvariant();

        entidad.SiguienteOrdenTrabajo =
            dto.SiguienteOrdenTrabajo;

        entidad.PrefijoVenta =
            dto.PrefijoVenta.Trim().ToUpperInvariant();

        entidad.SiguienteVenta =
            dto.SiguienteVenta;

        entidad.PrefijoFactura =
            dto.PrefijoFactura.Trim().ToUpperInvariant();

        entidad.SiguienteFactura =
            dto.SiguienteFactura;

        entidad.HoraApertura =
            dto.HoraApertura;

        entidad.HoraCierre =
            dto.HoraCierre;

        entidad.DiasGarantia =
            dto.DiasGarantia;

        entidad.ExistenciaMinimaPredeterminada =
            dto.ExistenciaMinimaPredeterminada;

        entidad.EstadosProceso =
            dto.EstadosProceso.Trim();
    }
}