namespace Negocios.Seguridad;

public static class PermisosPorRol
{
    public static IReadOnlyDictionary<string, IReadOnlyList<string>>
        Obtener()
    {
        return new Dictionary<string, IReadOnlyList<string>>
        {
            // =================================================
            // ADMINISTRADOR
            // =================================================

            ["Administrador"] =
                PermisosCatalogo.Todos,


            // =================================================
            // RECEPCIONISTA
            // =================================================

            ["Recepcionista"] =
                new[]
                {
                    Permisos.ClientesVer,
                    Permisos.ClientesCrear,
                    Permisos.ClientesEditar,

                    Permisos.VehiculosVer,
                    Permisos.VehiculosCrear,
                    Permisos.VehiculosEditar,

                    Permisos.RecepcionesVer,
                    Permisos.RecepcionesCrear,
                    Permisos.RecepcionesEditar,

                    Permisos.CotizacionesVer,
                    Permisos.CotizacionesCrear,

                    Permisos.OrdenesVer,

                    Permisos.ServiciosVer
                },


            // =================================================
            // MECÁNICO
            // =================================================

            ["Mecanico"] =
                new[]
                {
                    Permisos.ClientesVer,

                    Permisos.VehiculosVer,

                    Permisos.RecepcionesVer,

                    Permisos.DiagnosticosVer,
                    Permisos.DiagnosticosCrear,
                    Permisos.DiagnosticosEditar,
                    Permisos.DiagnosticosAprobar,

                    Permisos.CotizacionesVer,

                    Permisos.OrdenesVer,
                    Permisos.OrdenesEditar,
                    Permisos.OrdenesAprobar,

                    Permisos.ProductosVer,

                    Permisos.ServiciosVer
                },


            // =================================================
            // ENCARGADO DE INVENTARIO
            // =================================================

            ["EncargadoInventario"] =
                new[]
                {
                    Permisos.ProductosVer,
                    Permisos.ProductosCrear,
                    Permisos.ProductosEditar,
                    Permisos.ProductosDesactivar,

                    Permisos.MovimientosInventarioVer,
                    Permisos.MovimientosInventarioCrear,

                    Permisos.ProveedoresVer,
                    Permisos.ProveedoresCrear,
                    Permisos.ProveedoresEditar,

                    Permisos.ComprasVer,
                    Permisos.ComprasCrear,
                    Permisos.ComprasEditar
                },


            // =================================================
            // VENDEDOR
            // =================================================

            ["Vendedor"] =
                new[]
                {
                    Permisos.ClientesVer,

                    Permisos.VehiculosVer,

                    Permisos.ServiciosVer,

                    Permisos.ProductosVer,

                    Permisos.CotizacionesVer,
                    Permisos.CotizacionesCrear,
                    Permisos.CotizacionesEditar,

                    Permisos.VentasVer,
                    Permisos.VentasCrear,

                    Permisos.DescuentosAplicar
                },


            // =================================================
            // CAJERO
            // =================================================

            ["Cajero"] =
                new[]
                {
                    Permisos.ClientesVer,

                    Permisos.VehiculosVer,

                    Permisos.CotizacionesVer,

                    Permisos.OrdenesVer,

                    Permisos.VentasVer,
                    Permisos.VentasCrear,

                    Permisos.FacturacionVer,
                    Permisos.FacturacionCrear,

                    Permisos.PagosVer,
                    Permisos.PagosRegistrar
                },


            // =================================================
            // SUPERVISOR
            // =================================================

            ["Supervisor"] =
                new[]
                {
                    Permisos.ClientesVer,

                    Permisos.VehiculosVer,

                    Permisos.RecepcionesVer,

                    Permisos.DiagnosticosVer,
                    Permisos.DiagnosticosAprobar,

                    Permisos.CotizacionesVer,
                    Permisos.CotizacionesAprobar,

                    Permisos.OrdenesVer,
                    Permisos.OrdenesAprobar,

                    Permisos.ProductosVer,

                    Permisos.MovimientosInventarioVer,

                    Permisos.ProveedoresVer,

                    Permisos.ComprasVer,

                    Permisos.EmpleadosVer,

                    Permisos.ServiciosVer,

                    Permisos.VentasVer,

                    Permisos.FacturacionVer,

                    Permisos.PagosVer,

                    Permisos.ReportesVer,

                    Permisos.AuditoriaVer
                }
        };
    }
}