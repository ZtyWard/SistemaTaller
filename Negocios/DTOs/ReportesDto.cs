namespace Negocios.DTOs;

public class ReportesDto
{
    public DateTime Desde { get; set; }

    public DateTime Hasta { get; set; }

    public string? Placa { get; set; }

    public decimal Ingresos { get; set; }

    public decimal Compras { get; set; }

    public decimal Utilidad { get; set; }

    public int FacturasPendientes { get; set; }

    public int OrdenesPendientes { get; set; }

    public int OrdenesAtrasadas { get; set; }

    public int OrdenesFinalizadas { get; set; }

    public int TrabajosPeriodo { get; set; }

    public List<ReporteVentaMesDto> VentasPorMes { get; set; }
        = new();

    public List<ReporteOrdenEstadoDto> OrdenesPorEstado { get; set; }
        = new();

    public List<ReporteProductoVendidoDto> ProductosMasVendidos { get; set; }
        = new();

    public List<ReporteMecanicoDto> TrabajosPorMecanico { get; set; }
        = new();

    public List<ReporteServicioDto> ServiciosMasSolicitados { get; set; }
        = new();

    public List<ReporteCompraMesDto> ComprasPorMes { get; set; }
        = new();

    public List<ReporteStockDto> StockBajo { get; set; }
        = new();

    public List<ReporteFacturaPendienteDto> FacturasPendientesLista { get; set; }
        = new();

    public List<ReporteVehiculoHistorialDto> HistorialVehiculos { get; set; }
        = new();
}

public class ReporteVentaMesDto
{
    public string Mes { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public int Cantidad { get; set; }
}

public class ReporteOrdenEstadoDto
{
    public string Estado { get; set; } = string.Empty;

    public int Cantidad { get; set; }
}

public class ReporteProductoVendidoDto
{
    public string Nombre { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public decimal Total { get; set; }
}

public class ReporteMecanicoDto
{
    public string Mecanico { get; set; } = string.Empty;

    public int Trabajos { get; set; }

    public decimal Horas { get; set; }
}

public class ReporteServicioDto
{
    public string Servicio { get; set; } = string.Empty;

    public int Cantidad { get; set; }
}

public class ReporteCompraMesDto
{
    public string Mes { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public int Cantidad { get; set; }
}

public class ReporteStockDto
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int Stock { get; set; }

    public int StockMinimo { get; set; }
}

public class ReporteFacturaPendienteDto
{
    public int IdFactura { get; set; }

    public string NumeroFactura { get; set; } = string.Empty;

    public DateTime FechaEmision { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public decimal TotalPagado { get; set; }

    public decimal SaldoPendiente { get; set; }

    public string Estado { get; set; } = string.Empty;
}

public class ReporteVehiculoHistorialDto
{
    public string Placa { get; set; } = string.Empty;

    public string Cliente { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }

    public int IdOrdenTrabajo { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string? Observaciones { get; set; }
}