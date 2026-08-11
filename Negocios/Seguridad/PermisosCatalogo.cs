using System.Collections.Generic;

namespace Negocios.Seguridad;

public static class PermisosCatalogo
{
    public static IReadOnlyList<string> Todos =>
        new[]
        {
            // =================================================
            // CLIENTES
            // =================================================

            Permisos.ClientesVer,
            Permisos.ClientesCrear,
            Permisos.ClientesEditar,
            Permisos.ClientesDesactivar,

            // =================================================
            // VEHÍCULOS
            // =================================================

            Permisos.VehiculosVer,
            Permisos.VehiculosCrear,
            Permisos.VehiculosEditar,
            Permisos.VehiculosDesactivar,

            // =================================================
            // RECEPCIONES
            // =================================================

            Permisos.RecepcionesVer,
            Permisos.RecepcionesCrear,
            Permisos.RecepcionesEditar,
            Permisos.RecepcionesDesactivar,

            // =================================================
            // DIAGNÓSTICOS
            // =================================================

            Permisos.DiagnosticosVer,
            Permisos.DiagnosticosCrear,
            Permisos.DiagnosticosEditar,
            Permisos.DiagnosticosAprobar,

            // =================================================
            // COTIZACIONES
            // =================================================

            Permisos.CotizacionesVer,
            Permisos.CotizacionesCrear,
            Permisos.CotizacionesEditar,
            Permisos.CotizacionesAprobar,

            // =================================================
            // ÓRDENES DE TRABAJO
            // =================================================

            Permisos.OrdenesVer,
            Permisos.OrdenesCrear,
            Permisos.OrdenesEditar,
            Permisos.OrdenesAprobar,
            Permisos.OrdenesDesactivar,

            // =================================================
            // PRODUCTOS
            // =================================================

            Permisos.ProductosVer,
            Permisos.ProductosCrear,
            Permisos.ProductosEditar,
            Permisos.ProductosDesactivar,

            // =================================================
            // MOVIMIENTOS DE INVENTARIO
            // =================================================

            Permisos.MovimientosInventarioVer,
            Permisos.MovimientosInventarioCrear,

            // =================================================
            // PROVEEDORES
            // =================================================

            Permisos.ProveedoresVer,
            Permisos.ProveedoresCrear,
            Permisos.ProveedoresEditar,
            Permisos.ProveedoresDesactivar,

            // =================================================
            // COMPRAS
            // =================================================

            Permisos.ComprasVer,
            Permisos.ComprasCrear,
            Permisos.ComprasEditar,
            Permisos.ComprasAnular,

            // =================================================
            // EMPLEADOS
            // =================================================

            Permisos.EmpleadosVer,
            Permisos.EmpleadosCrear,
            Permisos.EmpleadosEditar,
            Permisos.EmpleadosDesactivar,

            // =================================================
            // SERVICIOS
            // =================================================

            Permisos.ServiciosVer,
            Permisos.ServiciosCrear,
            Permisos.ServiciosEditar,
            Permisos.ServiciosDesactivar,

            // =================================================
            // VENTAS
            // =================================================

            Permisos.VentasVer,
            Permisos.VentasCrear,
            Permisos.VentasAnular,

            // =================================================
            // FACTURACIÓN
            // =================================================

            Permisos.FacturacionVer,
            Permisos.FacturacionCrear,
            Permisos.FacturacionAnular,

            // =================================================
            // PAGOS
            // =================================================

            Permisos.PagosVer,
            Permisos.PagosRegistrar,

            // =================================================
            // DESCUENTOS
            // =================================================

            Permisos.DescuentosAplicar,

            // =================================================
            // REPORTES
            // =================================================

            Permisos.ReportesVer,

            // =================================================
            // USUARIOS
            // =================================================

            Permisos.UsuariosVer,
            Permisos.UsuariosCrear,
            Permisos.UsuariosEditar,
            Permisos.UsuariosDesactivar,

            // =================================================
            // ROLES
            // =================================================

            Permisos.RolesVer,
            Permisos.RolesAdministrar,

            // =================================================
            // PERMISOS
            // =================================================

            Permisos.PermisosAdministrar,

            // =================================================
            // AUDITORÍA
            // =================================================

            Permisos.AuditoriaVer
        };
}