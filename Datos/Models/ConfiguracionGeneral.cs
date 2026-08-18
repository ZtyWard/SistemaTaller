namespace Datos.Models;

public class ConfiguracionGeneral
{
    public int IdConfiguracion { get; set; }

    // =====================================================
    // INFORMACIÓN DEL TALLER
    // =====================================================

    public string NombreTaller { get; set; } = string.Empty;

    public string IdentificacionJuridica { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }


    // =====================================================
    // PARÁMETROS COMERCIALES
    // =====================================================

    public decimal ImpuestoPorcentaje { get; set; }

    public string Moneda { get; set; } = "CRC";

    public decimal LimiteDescuentoPorcentaje { get; set; }


    // =====================================================
    // NUMERACIÓN
    // =====================================================

    public string PrefijoRecepcion { get; set; } = "REC";

    public int SiguienteRecepcion { get; set; } = 1;


    public string PrefijoCotizacion { get; set; } = "COT";

    public int SiguienteCotizacion { get; set; } = 1;


    public string PrefijoOrdenTrabajo { get; set; } = "OT";

    public int SiguienteOrdenTrabajo { get; set; } = 1;


    public string PrefijoVenta { get; set; } = "VEN";

    public int SiguienteVenta { get; set; } = 1;


    public string PrefijoFactura { get; set; } = "FAC";

    public int SiguienteFactura { get; set; } = 1;


    // =====================================================
    // OPERACIÓN
    // =====================================================

    public TimeSpan HoraApertura { get; set; } =
        new(8, 0, 0);

    public TimeSpan HoraCierre { get; set; } =
        new(17, 0, 0);


    // =====================================================
    // GARANTÍAS
    // =====================================================

    public int DiasGarantia { get; set; } = 30;


    // =====================================================
    // INVENTARIO
    // =====================================================

    public int ExistenciaMinimaPredeterminada { get; set; } = 1;


    // =====================================================
    // ESTADOS DE PROCESOS
    // =====================================================

    public string EstadosProceso { get; set; } =
        "Pendiente,En proceso,Completado,Cancelado";
}