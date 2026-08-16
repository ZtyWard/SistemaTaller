namespace Datos.Models;

public class ReporteResumenData
{
    public decimal Ingresos { get; set; }

    public decimal Compras { get; set; }

    public decimal Utilidad { get; set; }

    public int FacturasPendientes { get; set; }

    public int OrdenesPendientes { get; set; }

    public int OrdenesAtrasadas { get; set; }

    public int OrdenesFinalizadas { get; set; }

    public int TrabajosPeriodo { get; set; }
}

public class ReporteVentaMesData
{
    public string Mes { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public int Cantidad { get; set; }
}

public class ReporteOrdenEstadoData
{
    public string Estado { get; set; } = string.Empty;

    public int Cantidad { get; set; }
}

public class ReporteProductoVendidoData
{
    public string Nombre { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public decimal Total { get; set; }
}

public class ReporteMecanicoData
{
    public string Mecanico { get; set; } = string.Empty;

    public int Trabajos { get; set; }

    public decimal Horas { get; set; }
}

public class ReporteServicioData
{
    public string Servicio { get; set; } = string.Empty;

    public int Cantidad { get; set; }
}

public class ReporteCompraMesData
{
    public string Mes { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public int Cantidad { get; set; }
}

public class ReporteStockData
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int Stock { get; set; }

    public int StockMinimo { get; set; }
}

public class ReporteFacturaPendienteData
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

public class ReporteVehiculoHistorialData
{
    public string Placa { get; set; } = string.Empty;

    public string Cliente { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }

    public int IdOrdenTrabajo { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string? Observaciones { get; set; }
}