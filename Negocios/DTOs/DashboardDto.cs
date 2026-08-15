using System;
using System.Collections.Generic;

namespace Negocios.DTOs;

public class DashboardDto
{
    public int ClientesActivos { get; set; }

    public int VehiculosActivos { get; set; }

    public int OrdenesAbiertas { get; set; }

    public int ProductosStockBajo { get; set; }

    public int FacturasPendientes { get; set; }

    public decimal VentasMesActual { get; set; }

    public int VentasMesActualCantidad { get; set; }

    public string RolUsuario { get; set; } = string.Empty;

    public List<DashboardVentaMesDto> VentasPorMes { get; set; } = new();

    public List<DashboardOrdenEstadoDto> OrdenesPorEstado { get; set; } = new();

    public List<DashboardProductoStockDto> ProductosStock { get; set; } = new();
}

public class DashboardVentaMesDto
{
    public string Mes { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public int Cantidad { get; set; }
}

public class DashboardOrdenEstadoDto
{
    public string Estado { get; set; } = string.Empty;

    public int Cantidad { get; set; }
}

public class DashboardProductoStockDto
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int Stock { get; set; }

    public int StockMinimo { get; set; }
}