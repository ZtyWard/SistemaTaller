USE [master]
GO
/****** Object:  Database [SistemaTallerDB]    Script Date: 11/8/2026 12:58:53 ******/
CREATE DATABASE [SistemaTallerDB]
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SistemaTallerDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SistemaTallerDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SistemaTallerDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SistemaTallerDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SistemaTallerDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SistemaTallerDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SistemaTallerDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET RECOVERY FULL 
GO
ALTER DATABASE [SistemaTallerDB] SET  MULTI_USER 
GO
ALTER DATABASE [SistemaTallerDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SistemaTallerDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SistemaTallerDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SistemaTallerDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SistemaTallerDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SistemaTallerDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SistemaTallerDB', N'ON'
GO
ALTER DATABASE [SistemaTallerDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [SistemaTallerDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SistemaTallerDB]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_SaldoFactura]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   9. FUNCIÓN — SALDO DE FACTURA
   ============================================================= */

CREATE   FUNCTION [dbo].[fn_SaldoFactura]
(
    @IdFactura INT
)
RETURNS DECIMAL(12,2)
AS
BEGIN

    DECLARE @Total DECIMAL(12,2);
    DECLARE @Pagado DECIMAL(12,2);

    SELECT
        @Total = Total
    FROM dbo.Factura
    WHERE IdFactura = @IdFactura;

    SELECT
        @Pagado = ISNULL(SUM(Monto), 0)
    FROM dbo.Pago
    WHERE IdFactura = @IdFactura;

    RETURN ISNULL(@Total, 0) - ISNULL(@Pagado, 0);

END;
GO
/****** Object:  Table [dbo].[CategoriaProducto]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriaProducto](
	[IdCategoriaProducto] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_CategoriaProducto] PRIMARY KEY CLUSTERED 
(
	[IdCategoriaProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_CategoriaProducto_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[IdProducto] [int] IDENTITY(1,1) NOT NULL,
	[IdCategoriaProducto] [int] NOT NULL,
	[Codigo] [nvarchar](450) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[PrecioCompra] [decimal](18, 2) NOT NULL,
	[PrecioVenta] [decimal](18, 2) NOT NULL,
	[Stock] [int] NOT NULL,
	[StockMinimo] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED 
(
	[IdProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Producto_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_InventarioEstado]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   5. VISTA — INVENTARIO
   ============================================================= */

CREATE   VIEW [dbo].[vw_InventarioEstado]
AS
SELECT
    p.IdProducto,
    p.Codigo,
    p.Nombre,
    cp.Nombre AS Categoria,
    p.PrecioCompra,
    p.PrecioVenta,
    p.Stock,
    p.StockMinimo,
    CASE
        WHEN p.Stock = 0 THEN 'AGOTADO'
        WHEN p.Stock <= p.StockMinimo THEN 'STOCK BAJO'
        ELSE 'NORMAL'
    END AS EstadoInventario,
    p.Activo
FROM dbo.Producto p
INNER JOIN dbo.CategoriaProducto cp
    ON cp.IdCategoriaProducto = p.IdCategoriaProducto;
GO
/****** Object:  Table [dbo].[Pago]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pago](
	[IdPago] [int] IDENTITY(1,1) NOT NULL,
	[IdFactura] [int] NOT NULL,
	[Monto] [decimal](12, 2) NOT NULL,
	[FormaPago] [nvarchar](30) NOT NULL,
	[NumeroReferencia] [nvarchar](100) NULL,
	[FechaPago] [datetime2](7) NOT NULL,
	[UsuarioId] [nvarchar](450) NULL,
	[Observaciones] [nvarchar](500) NULL,
 CONSTRAINT [PK_Pago] PRIMARY KEY CLUSTERED 
(
	[IdPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Cedula] [nvarchar](20) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido1] [nvarchar](100) NOT NULL,
	[Apellido2] [nvarchar](100) NULL,
	[Telefono] [nvarchar](20) NOT NULL,
	[Correo] [nvarchar](max) NULL,
	[Direccion] [nvarchar](max) NULL,
	[Activo] [bit] NOT NULL,
	[FechaRegistro] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Cliente_Cedula] UNIQUE NONCLUSTERED 
(
	[Cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Factura]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factura](
	[IdFactura] [int] IDENTITY(1,1) NOT NULL,
	[NumeroFactura] [nvarchar](30) NOT NULL,
	[IdCliente] [int] NULL,
	[IdOrdenTrabajo] [int] NULL,
	[IdVenta] [int] NULL,
	[FechaEmision] [datetime2](7) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
	[Impuesto] [decimal](12, 2) NOT NULL,
	[Descuento] [decimal](12, 2) NOT NULL,
	[Total] [decimal](12, 2) NOT NULL,
	[Estado] [nvarchar](30) NOT NULL,
	[UsuarioId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Factura] PRIMARY KEY CLUSTERED 
(
	[IdFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Factura_Numero] UNIQUE NONCLUSTERED 
(
	[NumeroFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_FacturasPendientes]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   6. VISTA — FACTURAS PENDIENTES
   ============================================================= */

CREATE   VIEW [dbo].[vw_FacturasPendientes]
AS
SELECT
    f.IdFactura,
    f.NumeroFactura,
    f.FechaEmision,

    c.IdCliente,
    CONCAT(
        c.Nombre,
        ' ',
        c.Apellido1,
        ' ',
        ISNULL(c.Apellido2, '')
    ) AS Cliente,

    f.Total,

    ISNULL(
        (
            SELECT SUM(p.Monto)
            FROM dbo.Pago p
            WHERE p.IdFactura = f.IdFactura
        ),
        0
    ) AS TotalPagado,

    f.Total -
    ISNULL(
        (
            SELECT SUM(p.Monto)
            FROM dbo.Pago p
            WHERE p.IdFactura = f.IdFactura
        ),
        0
    ) AS SaldoPendiente,

    f.Estado

FROM dbo.Factura f
LEFT JOIN dbo.Cliente c
    ON c.IdCliente = f.IdCliente

WHERE f.Estado IN
(
    'Pendiente',
    'Parcialmente pagada'
);
GO
/****** Object:  Table [dbo].[OrdenTrabajo]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenTrabajo](
	[IdOrdenTrabajo] [int] IDENTITY(1,1) NOT NULL,
	[IdCotizacion] [int] NOT NULL,
	[FechaInicio] [datetime2](7) NOT NULL,
	[FechaFin] [datetime2](7) NULL,
	[Estado] [nvarchar](50) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[IdVehiculo] [int] NULL,
	[NumeroOrden] [nvarchar](30) NULL,
 CONSTRAINT [PK_OrdenTrabajo] PRIMARY KEY CLUSTERED 
(
	[IdOrdenTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenTrabajoEmpleado]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenTrabajoEmpleado](
	[IdOrdenTrabajo] [int] NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[FechaAsignacion] [datetime2](7) NOT NULL,
	[HorasTrabajadas] [decimal](8, 2) NULL,
	[Observaciones] [nvarchar](500) NULL,
 CONSTRAINT [PK_OrdenTrabajoEmpleado] PRIMARY KEY CLUSTERED 
(
	[IdOrdenTrabajo] ASC,
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenTrabajoServicio]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenTrabajoServicio](
	[IdOrdenTrabajoServicio] [int] IDENTITY(1,1) NOT NULL,
	[IdOrdenTrabajo] [int] NOT NULL,
	[IdServicio] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](12, 2) NOT NULL,
	[Descuento] [decimal](12, 2) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_OrdenTrabajoServicio] PRIMARY KEY CLUSTERED 
(
	[IdOrdenTrabajoServicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdenTrabajoProducto]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdenTrabajoProducto](
	[IdOrdenTrabajoProducto] [int] IDENTITY(1,1) NOT NULL,
	[IdOrdenTrabajo] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](12, 2) NOT NULL,
	[Descuento] [decimal](12, 2) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_OrdenTrabajoProducto] PRIMARY KEY CLUSTERED 
(
	[IdOrdenTrabajoProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehiculo]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehiculo](
	[IdVehiculo] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdMarca] [int] NOT NULL,
	[IdModelo] [int] NOT NULL,
	[IdTipoVehiculo] [int] NOT NULL,
	[IdTipoCombustible] [int] NOT NULL,
	[Placa] [nvarchar](450) NOT NULL,
	[VIN] [nvarchar](max) NULL,
	[Color] [nvarchar](max) NULL,
	[Anio] [int] NULL,
	[Kilometraje] [int] NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Vehiculo] PRIMARY KEY CLUSTERED 
(
	[IdVehiculo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Vehiculo_Placa] UNIQUE NONCLUSTERED 
(
	[Placa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_OrdenesTrabajoResumen]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   7. VISTA — ÓRDENES DE TRABAJO
   ============================================================= */

CREATE   VIEW [dbo].[vw_OrdenesTrabajoResumen]
AS
SELECT
    ot.IdOrdenTrabajo,
    ot.NumeroOrden,
    ot.FechaInicio,
    ot.FechaFin,
    ot.Estado,

    ot.IdVehiculo,

    v.Placa,
    v.VIN,
    v.Color,

    c.IdCliente,

    CONCAT(
        c.Nombre,
        ' ',
        c.Apellido1,
        ' ',
        ISNULL(c.Apellido2, '')
    ) AS Cliente,

    ISNULL(
        (
            SELECT COUNT(*)
            FROM dbo.OrdenTrabajoEmpleado ote
            WHERE ote.IdOrdenTrabajo = ot.IdOrdenTrabajo
        ),
        0
    ) AS CantidadMecanicos,

    ISNULL(
        (
            SELECT COUNT(*)
            FROM dbo.OrdenTrabajoServicio ots
            WHERE ots.IdOrdenTrabajo = ot.IdOrdenTrabajo
        ),
        0
    ) AS CantidadServicios,

    ISNULL(
        (
            SELECT COUNT(*)
            FROM dbo.OrdenTrabajoProducto otp
            WHERE otp.IdOrdenTrabajo = ot.IdOrdenTrabajo
        ),
        0
    ) AS CantidadProductos

FROM dbo.OrdenTrabajo ot

LEFT JOIN dbo.Vehiculo v
    ON v.IdVehiculo = ot.IdVehiculo

LEFT JOIN dbo.Cliente c
    ON c.IdCliente = v.IdCliente;
GO
/****** Object:  Table [dbo].[Venta]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Venta](
	[IdVenta] [int] IDENTITY(1,1) NOT NULL,
	[NumeroVenta] [nvarchar](30) NOT NULL,
	[IdCliente] [int] NULL,
	[IdVendedor] [int] NOT NULL,
	[IdCajero] [int] NULL,
	[FechaVenta] [datetime2](7) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
	[Impuesto] [decimal](12, 2) NOT NULL,
	[Descuento] [decimal](12, 2) NOT NULL,
	[Total] [decimal](12, 2) NOT NULL,
	[FormaPago] [nvarchar](30) NULL,
	[Estado] [nvarchar](30) NOT NULL,
	[UsuarioId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Venta] PRIMARY KEY CLUSTERED 
(
	[IdVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Venta_Numero] UNIQUE NONCLUSTERED 
(
	[NumeroVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[IdEmpleado] [int] IDENTITY(1,1) NOT NULL,
	[Cedula] [nvarchar](max) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Apellido1] [nvarchar](max) NOT NULL,
	[Apellido2] [nvarchar](max) NULL,
	[Telefono] [nvarchar](max) NULL,
	[Correo] [nvarchar](max) NULL,
	[IdPuesto] [int] NOT NULL,
	[IdEspecialidad] [int] NOT NULL,
	[Salario] [decimal](18, 2) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_VentasResumen]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   8. VISTA — RESUMEN DE VENTAS
   ============================================================= */

CREATE   VIEW [dbo].[vw_VentasResumen]
AS
SELECT
    v.IdVenta,
    v.NumeroVenta,
    v.FechaVenta,

    v.IdCliente,

    CASE
        WHEN v.IdCliente IS NULL
            THEN 'Cliente general'
        ELSE
            CONCAT(
                c.Nombre,
                ' ',
                c.Apellido1,
                ' ',
                ISNULL(c.Apellido2, '')
            )
    END AS Cliente,

    CONCAT(
        e.Nombre,
        ' ',
        e.Apellido1
    ) AS Vendedor,

    v.Subtotal,
    v.Impuesto,
    v.Descuento,
    v.Total,
    v.FormaPago,
    v.Estado

FROM dbo.Venta v

LEFT JOIN dbo.Cliente c
    ON c.IdCliente = v.IdCliente

INNER JOIN dbo.Empleado e
    ON e.IdEmpleado = v.IdVendedor;
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Auditoria]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auditoria](
	[IdAuditoria] [bigint] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [nvarchar](max) NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Modulo] [nvarchar](100) NOT NULL,
	[Accion] [nvarchar](50) NOT NULL,
	[RegistroId] [nvarchar](max) NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Ip] [nvarchar](max) NULL,
	[ValoresAnteriores] [nvarchar](max) NULL,
	[ValoresNuevos] [nvarchar](max) NULL,
 CONSTRAINT [PK_Auditoria] PRIMARY KEY CLUSTERED 
(
	[IdAuditoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cita]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cita](
	[IdCita] [int] IDENTITY(1,1) NOT NULL,
	[NumeroCita] [nvarchar](30) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdVehiculo] [int] NOT NULL,
	[IdServicio] [int] NULL,
	[IdEmpleado] [int] NULL,
	[FechaInicio] [datetime2](7) NOT NULL,
	[FechaFin] [datetime2](7) NOT NULL,
	[Estado] [nvarchar](30) NOT NULL,
	[Observaciones] [nvarchar](500) NULL,
 CONSTRAINT [PK_Cita] PRIMARY KEY CLUSTERED 
(
	[IdCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Cita_Numero] UNIQUE NONCLUSTERED 
(
	[NumeroCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compra]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compra](
	[IdCompra] [int] IDENTITY(1,1) NOT NULL,
	[IdProveedor] [int] NOT NULL,
	[FechaCompra] [datetime2](7) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Estado] [nvarchar](max) NOT NULL,
	[NumeroFacturaProveedor] [nvarchar](50) NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
	[Impuesto] [decimal](12, 2) NOT NULL,
	[Descuento] [decimal](12, 2) NOT NULL,
	[FormaPago] [nvarchar](30) NULL,
	[UsuarioId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Compra] PRIMARY KEY CLUSTERED 
(
	[IdCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cotizacion]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cotizacion](
	[IdCotizacion] [int] IDENTITY(1,1) NOT NULL,
	[IdDiagnostico] [int] NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Estado] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Cotizacion] PRIMARY KEY CLUSTERED 
(
	[IdCotizacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleCompra]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleCompra](
	[IdDetalleCompra] [int] IDENTITY(1,1) NOT NULL,
	[IdCompra] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[CostoUnitario] [decimal](12, 2) NOT NULL,
	[Impuesto] [decimal](12, 2) NOT NULL,
	[Descuento] [decimal](12, 2) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_DetalleCompra] PRIMARY KEY CLUSTERED 
(
	[IdDetalleCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleFactura]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleFactura](
	[IdDetalleFactura] [int] IDENTITY(1,1) NOT NULL,
	[IdFactura] [int] NOT NULL,
	[IdProducto] [int] NULL,
	[IdServicio] [int] NULL,
	[Descripcion] [nvarchar](500) NOT NULL,
	[Cantidad] [decimal](12, 2) NOT NULL,
	[PrecioUnitario] [decimal](12, 2) NOT NULL,
	[Impuesto] [decimal](12, 2) NOT NULL,
	[Descuento] [decimal](12, 2) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_DetalleFactura] PRIMARY KEY CLUSTERED 
(
	[IdDetalleFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleVenta]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleVenta](
	[IdDetalleVenta] [int] IDENTITY(1,1) NOT NULL,
	[IdVenta] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](12, 2) NOT NULL,
	[Impuesto] [decimal](12, 2) NOT NULL,
	[Descuento] [decimal](12, 2) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_DetalleVenta] PRIMARY KEY CLUSTERED 
(
	[IdDetalleVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Diagnostico]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diagnostico](
	[IdDiagnostico] [int] IDENTITY(1,1) NOT NULL,
	[IdRecepcion] [int] NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
	[FechaDiagnostico] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Diagnostico] PRIMARY KEY CLUSTERED 
(
	[IdDiagnostico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntregaVehiculo]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntregaVehiculo](
	[IdEntrega] [int] IDENTITY(1,1) NOT NULL,
	[IdOrdenTrabajo] [int] NOT NULL,
	[FechaEntrega] [datetime2](7) NOT NULL,
	[KilometrajeSalida] [int] NULL,
	[PersonaRecibe] [nvarchar](200) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[Recomendaciones] [nvarchar](max) NULL,
	[ProximaFechaServicio] [date] NULL,
	[AceptacionCliente] [bit] NOT NULL,
	[EstadoPago] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_EntregaVehiculo] PRIMARY KEY CLUSTERED 
(
	[IdEntrega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EntregaVehiculo_Orden] UNIQUE NONCLUSTERED 
(
	[IdOrdenTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Especialidad]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Especialidad](
	[IdEspecialidad] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Especialidad] PRIMARY KEY CLUSTERED 
(
	[IdEspecialidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Especialidad_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Garantia]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garantia](
	[IdGarantia] [int] IDENTITY(1,1) NOT NULL,
	[IdOrdenTrabajo] [int] NULL,
	[IdVenta] [int] NULL,
	[IdProducto] [int] NULL,
	[IdServicio] [int] NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaVencimiento] [date] NOT NULL,
	[Condiciones] [nvarchar](max) NULL,
	[Estado] [nvarchar](30) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
 CONSTRAINT [PK_Garantia] PRIMARY KEY CLUSTERED 
(
	[IdGarantia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistorialOrdenTrabajo]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistorialOrdenTrabajo](
	[IdHistorial] [bigint] IDENTITY(1,1) NOT NULL,
	[IdOrdenTrabajo] [int] NOT NULL,
	[EstadoAnterior] [nvarchar](50) NULL,
	[EstadoNuevo] [nvarchar](50) NOT NULL,
	[FechaCambio] [datetime2](7) NOT NULL,
	[UsuarioId] [nvarchar](450) NULL,
	[Observaciones] [nvarchar](500) NULL,
 CONSTRAINT [PK_HistorialOrdenTrabajo] PRIMARY KEY CLUSTERED 
(
	[IdHistorial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityUsuarios]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityUsuarios](
	[Id] [nvarchar](450) NOT NULL,
	[NombreCompleto] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_IdentityUsuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Marca]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marca](
	[IdMarca] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Marca] PRIMARY KEY CLUSTERED 
(
	[IdMarca] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Marca_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modelo]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modelo](
	[IdModelo] [int] IDENTITY(1,1) NOT NULL,
	[IdMarca] [int] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Modelo] PRIMARY KEY CLUSTERED 
(
	[IdModelo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Modelo_Marca_Nombre] UNIQUE NONCLUSTERED 
(
	[IdMarca] ASC,
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovimientoInventario]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovimientoInventario](
	[IdMovimiento] [int] IDENTITY(1,1) NOT NULL,
	[IdProducto] [int] NOT NULL,
	[TipoMovimiento] [nvarchar](max) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[FechaMovimiento] [datetime2](7) NOT NULL,
	[Observacion] [nvarchar](max) NULL,
 CONSTRAINT [PK_MovimientoInventario] PRIMARY KEY CLUSTERED 
(
	[IdMovimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notificacion]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notificacion](
	[IdNotificacion] [bigint] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [nvarchar](450) NOT NULL,
	[Tipo] [nvarchar](50) NOT NULL,
	[Titulo] [nvarchar](200) NOT NULL,
	[Mensaje] [nvarchar](1000) NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
	[FechaLectura] [datetime2](7) NULL,
	[Leida] [bit] NOT NULL,
 CONSTRAINT [PK_Notificacion] PRIMARY KEY CLUSTERED 
(
	[IdNotificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedor]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedor](
	[IdProveedor] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[CedulaJuridica] [nvarchar](max) NULL,
	[Telefono] [nvarchar](max) NULL,
	[Correo] [nvarchar](max) NULL,
	[Direccion] [nvarchar](max) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Proveedor] PRIMARY KEY CLUSTERED 
(
	[IdProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Puesto]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Puesto](
	[IdPuesto] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Puesto] PRIMARY KEY CLUSTERED 
(
	[IdPuesto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Puesto_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recepcion]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recepcion](
	[IdRecepcion] [int] IDENTITY(1,1) NOT NULL,
	[IdVehiculo] [int] NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[FechaRecepcion] [datetime2](7) NOT NULL,
	[Kilometraje] [int] NOT NULL,
	[NivelCombustible] [nvarchar](max) NULL,
	[Observaciones] [nvarchar](max) NULL,
	[Estado] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Recepcion] PRIMARY KEY CLUSTERED 
(
	[IdRecepcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servicio]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicio](
	[IdServicio] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Precio] [decimal](18, 2) NOT NULL,
	[DuracionEstimada] [int] NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Servicio] PRIMARY KEY CLUSTERED 
(
	[IdServicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoCombustible]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoCombustible](
	[IdTipoCombustible] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_TipoCombustible] PRIMARY KEY CLUSTERED 
(
	[IdTipoCombustible] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_TipoCombustible_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoVehiculo]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoVehiculo](
	[IdTipoVehiculo] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_TipoVehiculo] PRIMARY KEY CLUSTERED 
(
	[IdTipoVehiculo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_TipoVehiculo_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 11/8/2026 12:58:54 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Auditoria_Fecha]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Auditoria_Fecha] ON [dbo].[Auditoria]
(
	[Fecha] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Auditoria_Fecha_Modulo]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Auditoria_Fecha_Modulo] ON [dbo].[Auditoria]
(
	[Fecha] ASC,
	[Modulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cita_Fecha]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Cita_Fecha] ON [dbo].[Cita]
(
	[FechaInicio] ASC,
	[FechaFin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Compra_FechaCompra]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Compra_FechaCompra] ON [dbo].[Compra]
(
	[FechaCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Cotizacion_Diagnostico]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Cotizacion_Diagnostico] ON [dbo].[Cotizacion]
(
	[IdDiagnostico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleCompra_Compra]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleCompra_Compra] ON [dbo].[DetalleCompra]
(
	[IdCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleCompra_Producto]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleCompra_Producto] ON [dbo].[DetalleCompra]
(
	[IdProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleVenta_Producto]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleVenta_Producto] ON [dbo].[DetalleVenta]
(
	[IdProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Diagnostico_Recepcion]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Diagnostico_Recepcion] ON [dbo].[Diagnostico]
(
	[IdRecepcion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Empleado_Puesto]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Empleado_Puesto] ON [dbo].[Empleado]
(
	[IdPuesto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Factura_Cliente]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Factura_Cliente] ON [dbo].[Factura]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Factura_Estado]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Factura_Estado] ON [dbo].[Factura]
(
	[Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Factura_Fecha]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Factura_Fecha] ON [dbo].[Factura]
(
	[FechaEmision] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[IdentityUsuarios]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 11/8/2026 12:58:54 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[IdentityUsuarios]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimientoInventario_Fecha]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_MovimientoInventario_Fecha] ON [dbo].[MovimientoInventario]
(
	[FechaMovimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MovimientoInventario_Producto_Fecha]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_MovimientoInventario_Producto_Fecha] ON [dbo].[MovimientoInventario]
(
	[IdProducto] ASC,
	[FechaMovimiento] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Notificacion_Usuario]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Notificacion_Usuario] ON [dbo].[Notificacion]
(
	[UsuarioId] ASC,
	[Leida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_OrdenTrabajo_Estado]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajo_Estado] ON [dbo].[OrdenTrabajo]
(
	[Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_OrdenTrabajo_Estado_FechaInicio]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajo_Estado_FechaInicio] ON [dbo].[OrdenTrabajo]
(
	[Estado] ASC,
	[FechaInicio] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrdenTrabajo_Vehiculo]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajo_Vehiculo] ON [dbo].[OrdenTrabajo]
(
	[IdVehiculo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrdenTrabajoEmpleado_Empleado]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajoEmpleado_Empleado] ON [dbo].[OrdenTrabajoEmpleado]
(
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrdenTrabajoProducto_Producto]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajoProducto_Producto] ON [dbo].[OrdenTrabajoProducto]
(
	[IdProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrdenTrabajoServicio_Servicio]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_OrdenTrabajoServicio_Servicio] ON [dbo].[OrdenTrabajoServicio]
(
	[IdServicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pago_Factura]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Pago_Factura] ON [dbo].[Pago]
(
	[IdFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Producto_Categoria]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Producto_Categoria] ON [dbo].[Producto]
(
	[IdCategoriaProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Recepcion_Vehiculo]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Recepcion_Vehiculo] ON [dbo].[Recepcion]
(
	[IdVehiculo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vehiculo_Cliente]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Vehiculo_Cliente] ON [dbo].[Vehiculo]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Venta_Cliente]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Venta_Cliente] ON [dbo].[Venta]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Venta_Fecha]    Script Date: 11/8/2026 12:58:54 ******/
CREATE NONCLUSTERED INDEX [IX_Venta_Fecha] ON [dbo].[Venta]
(
	[FechaVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auditoria] ADD  DEFAULT (sysutcdatetime()) FOR [Fecha]
GO
ALTER TABLE [dbo].[CategoriaProducto] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Cita] ADD  CONSTRAINT [DF_Cita_Estado]  DEFAULT ('Programada') FOR [Estado]
GO
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT (sysutcdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Compra] ADD  DEFAULT (sysutcdatetime()) FOR [FechaCompra]
GO
ALTER TABLE [dbo].[Compra] ADD  DEFAULT (N'Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[Compra] ADD  CONSTRAINT [DF_Compra_Subtotal]  DEFAULT ((0)) FOR [Subtotal]
GO
ALTER TABLE [dbo].[Compra] ADD  CONSTRAINT [DF_Compra_Impuesto]  DEFAULT ((0)) FOR [Impuesto]
GO
ALTER TABLE [dbo].[Compra] ADD  CONSTRAINT [DF_Compra_Descuento]  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[Cotizacion] ADD  DEFAULT (sysutcdatetime()) FOR [Fecha]
GO
ALTER TABLE [dbo].[Cotizacion] ADD  DEFAULT ((0)) FOR [Total]
GO
ALTER TABLE [dbo].[Cotizacion] ADD  DEFAULT (N'Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[DetalleCompra] ADD  CONSTRAINT [DF_DetalleCompra_Impuesto]  DEFAULT ((0)) FOR [Impuesto]
GO
ALTER TABLE [dbo].[DetalleCompra] ADD  CONSTRAINT [DF_DetalleCompra_Descuento]  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[DetalleFactura] ADD  CONSTRAINT [DF_DetalleFactura_Cantidad]  DEFAULT ((1)) FOR [Cantidad]
GO
ALTER TABLE [dbo].[DetalleFactura] ADD  CONSTRAINT [DF_DetalleFactura_Impuesto]  DEFAULT ((0)) FOR [Impuesto]
GO
ALTER TABLE [dbo].[DetalleFactura] ADD  CONSTRAINT [DF_DetalleFactura_Descuento]  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[DetalleVenta] ADD  CONSTRAINT [DF_DetalleVenta_Impuesto]  DEFAULT ((0)) FOR [Impuesto]
GO
ALTER TABLE [dbo].[DetalleVenta] ADD  CONSTRAINT [DF_DetalleVenta_Descuento]  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[Diagnostico] ADD  DEFAULT (sysutcdatetime()) FOR [FechaDiagnostico]
GO
ALTER TABLE [dbo].[Empleado] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[EntregaVehiculo] ADD  CONSTRAINT [DF_EntregaVehiculo_Fecha]  DEFAULT (sysdatetime()) FOR [FechaEntrega]
GO
ALTER TABLE [dbo].[EntregaVehiculo] ADD  CONSTRAINT [DF_EntregaVehiculo_Aceptacion]  DEFAULT ((0)) FOR [AceptacionCliente]
GO
ALTER TABLE [dbo].[Especialidad] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Factura] ADD  CONSTRAINT [DF_Factura_Fecha]  DEFAULT (sysdatetime()) FOR [FechaEmision]
GO
ALTER TABLE [dbo].[Factura] ADD  CONSTRAINT [DF_Factura_Subtotal]  DEFAULT ((0)) FOR [Subtotal]
GO
ALTER TABLE [dbo].[Factura] ADD  CONSTRAINT [DF_Factura_Impuesto]  DEFAULT ((0)) FOR [Impuesto]
GO
ALTER TABLE [dbo].[Factura] ADD  CONSTRAINT [DF_Factura_Descuento]  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[Factura] ADD  CONSTRAINT [DF_Factura_Total]  DEFAULT ((0)) FOR [Total]
GO
ALTER TABLE [dbo].[Factura] ADD  CONSTRAINT [DF_Factura_Estado]  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[Garantia] ADD  CONSTRAINT [DF_Garantia_Estado]  DEFAULT ('Vigente') FOR [Estado]
GO
ALTER TABLE [dbo].[HistorialOrdenTrabajo] ADD  CONSTRAINT [DF_HistorialOrdenTrabajo_Fecha]  DEFAULT (sysdatetime()) FOR [FechaCambio]
GO
ALTER TABLE [dbo].[IdentityUsuarios] ADD  CONSTRAINT [DF_IdentityUsuarios_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Marca] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Modelo] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[MovimientoInventario] ADD  DEFAULT (sysutcdatetime()) FOR [FechaMovimiento]
GO
ALTER TABLE [dbo].[Notificacion] ADD  CONSTRAINT [DF_Notificacion_Fecha]  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Notificacion] ADD  CONSTRAINT [DF_Notificacion_Leida]  DEFAULT ((0)) FOR [Leida]
GO
ALTER TABLE [dbo].[OrdenTrabajo] ADD  DEFAULT (sysutcdatetime()) FOR [FechaInicio]
GO
ALTER TABLE [dbo].[OrdenTrabajo] ADD  DEFAULT (N'Abierta') FOR [Estado]
GO
ALTER TABLE [dbo].[OrdenTrabajoEmpleado] ADD  CONSTRAINT [DF_OrdenTrabajoEmpleado_Fecha]  DEFAULT (sysdatetime()) FOR [FechaAsignacion]
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto] ADD  CONSTRAINT [DF_OrdenTrabajoProducto_Descuento]  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio] ADD  CONSTRAINT [DF_OrdenTrabajoServicio_Cantidad]  DEFAULT ((1)) FOR [Cantidad]
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio] ADD  CONSTRAINT [DF_OrdenTrabajoServicio_Descuento]  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[Pago] ADD  CONSTRAINT [DF_Pago_Fecha]  DEFAULT (sysdatetime()) FOR [FechaPago]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((5)) FOR [StockMinimo]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Proveedor] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Puesto] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Recepcion] ADD  DEFAULT (sysutcdatetime()) FOR [FechaRecepcion]
GO
ALTER TABLE [dbo].[Recepcion] ADD  DEFAULT (N'Recibido') FOR [Estado]
GO
ALTER TABLE [dbo].[Servicio] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[TipoCombustible] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[TipoVehiculo] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Vehiculo] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_Fecha]  DEFAULT (sysdatetime()) FOR [FechaVenta]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_Subtotal]  DEFAULT ((0)) FOR [Subtotal]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_Impuesto]  DEFAULT ((0)) FOR [Impuesto]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_Descuento]  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_Total]  DEFAULT ((0)) FOR [Total]
GO
ALTER TABLE [dbo].[Venta] ADD  CONSTRAINT [DF_Venta_Estado]  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_IdentityUsuarios_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_IdentityUsuarios_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_IdentityUsuarios_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_IdentityUsuarios_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_IdentityUsuarios_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_IdentityUsuarios_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_IdentityUsuarios_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_IdentityUsuarios_UserId]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Cliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Cliente] ([IdCliente])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Cliente]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Empleado] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Empleado]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Servicio] FOREIGN KEY([IdServicio])
REFERENCES [dbo].[Servicio] ([IdServicio])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Servicio]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [FK_Cita_Vehiculo] FOREIGN KEY([IdVehiculo])
REFERENCES [dbo].[Vehiculo] ([IdVehiculo])
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [FK_Cita_Vehiculo]
GO
ALTER TABLE [dbo].[Compra]  WITH CHECK ADD  CONSTRAINT [FK_Compra_Proveedor] FOREIGN KEY([IdProveedor])
REFERENCES [dbo].[Proveedor] ([IdProveedor])
GO
ALTER TABLE [dbo].[Compra] CHECK CONSTRAINT [FK_Compra_Proveedor]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Diagnostico] FOREIGN KEY([IdDiagnostico])
REFERENCES [dbo].[Diagnostico] ([IdDiagnostico])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Diagnostico]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [FK_DetalleCompra_Compra] FOREIGN KEY([IdCompra])
REFERENCES [dbo].[Compra] ([IdCompra])
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [FK_DetalleCompra_Compra]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [FK_DetalleCompra_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [FK_DetalleCompra_Producto]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [FK_DetalleFactura_Factura] FOREIGN KEY([IdFactura])
REFERENCES [dbo].[Factura] ([IdFactura])
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [FK_DetalleFactura_Factura]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [FK_DetalleFactura_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [FK_DetalleFactura_Producto]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [FK_DetalleFactura_Servicio] FOREIGN KEY([IdServicio])
REFERENCES [dbo].[Servicio] ([IdServicio])
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [FK_DetalleFactura_Servicio]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [FK_DetalleVenta_Producto]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleVenta_Venta] FOREIGN KEY([IdVenta])
REFERENCES [dbo].[Venta] ([IdVenta])
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [FK_DetalleVenta_Venta]
GO
ALTER TABLE [dbo].[Diagnostico]  WITH CHECK ADD  CONSTRAINT [FK_Diagnostico_Empleado] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Diagnostico] CHECK CONSTRAINT [FK_Diagnostico_Empleado]
GO
ALTER TABLE [dbo].[Diagnostico]  WITH CHECK ADD  CONSTRAINT [FK_Diagnostico_Recepcion] FOREIGN KEY([IdRecepcion])
REFERENCES [dbo].[Recepcion] ([IdRecepcion])
GO
ALTER TABLE [dbo].[Diagnostico] CHECK CONSTRAINT [FK_Diagnostico_Recepcion]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK_Empleado_Especialidad] FOREIGN KEY([IdEspecialidad])
REFERENCES [dbo].[Especialidad] ([IdEspecialidad])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Especialidad]
GO
ALTER TABLE [dbo].[Empleado]  WITH CHECK ADD  CONSTRAINT [FK_Empleado_Puesto] FOREIGN KEY([IdPuesto])
REFERENCES [dbo].[Puesto] ([IdPuesto])
GO
ALTER TABLE [dbo].[Empleado] CHECK CONSTRAINT [FK_Empleado_Puesto]
GO
ALTER TABLE [dbo].[EntregaVehiculo]  WITH CHECK ADD  CONSTRAINT [FK_EntregaVehiculo_Orden] FOREIGN KEY([IdOrdenTrabajo])
REFERENCES [dbo].[OrdenTrabajo] ([IdOrdenTrabajo])
GO
ALTER TABLE [dbo].[EntregaVehiculo] CHECK CONSTRAINT [FK_EntregaVehiculo_Orden]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [FK_Factura_Cliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Cliente] ([IdCliente])
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [FK_Factura_Cliente]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [FK_Factura_Orden] FOREIGN KEY([IdOrdenTrabajo])
REFERENCES [dbo].[OrdenTrabajo] ([IdOrdenTrabajo])
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [FK_Factura_Orden]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [FK_Factura_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [FK_Factura_Usuario]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [FK_Factura_Venta] FOREIGN KEY([IdVenta])
REFERENCES [dbo].[Venta] ([IdVenta])
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [FK_Factura_Venta]
GO
ALTER TABLE [dbo].[Garantia]  WITH CHECK ADD  CONSTRAINT [FK_Garantia_Orden] FOREIGN KEY([IdOrdenTrabajo])
REFERENCES [dbo].[OrdenTrabajo] ([IdOrdenTrabajo])
GO
ALTER TABLE [dbo].[Garantia] CHECK CONSTRAINT [FK_Garantia_Orden]
GO
ALTER TABLE [dbo].[Garantia]  WITH CHECK ADD  CONSTRAINT [FK_Garantia_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[Garantia] CHECK CONSTRAINT [FK_Garantia_Producto]
GO
ALTER TABLE [dbo].[Garantia]  WITH CHECK ADD  CONSTRAINT [FK_Garantia_Servicio] FOREIGN KEY([IdServicio])
REFERENCES [dbo].[Servicio] ([IdServicio])
GO
ALTER TABLE [dbo].[Garantia] CHECK CONSTRAINT [FK_Garantia_Servicio]
GO
ALTER TABLE [dbo].[Garantia]  WITH CHECK ADD  CONSTRAINT [FK_Garantia_Venta] FOREIGN KEY([IdVenta])
REFERENCES [dbo].[Venta] ([IdVenta])
GO
ALTER TABLE [dbo].[Garantia] CHECK CONSTRAINT [FK_Garantia_Venta]
GO
ALTER TABLE [dbo].[HistorialOrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_HistorialOrdenTrabajo_Orden] FOREIGN KEY([IdOrdenTrabajo])
REFERENCES [dbo].[OrdenTrabajo] ([IdOrdenTrabajo])
GO
ALTER TABLE [dbo].[HistorialOrdenTrabajo] CHECK CONSTRAINT [FK_HistorialOrdenTrabajo_Orden]
GO
ALTER TABLE [dbo].[HistorialOrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_HistorialOrdenTrabajo_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
GO
ALTER TABLE [dbo].[HistorialOrdenTrabajo] CHECK CONSTRAINT [FK_HistorialOrdenTrabajo_Usuario]
GO
ALTER TABLE [dbo].[Modelo]  WITH CHECK ADD  CONSTRAINT [FK_Modelo_Marca] FOREIGN KEY([IdMarca])
REFERENCES [dbo].[Marca] ([IdMarca])
GO
ALTER TABLE [dbo].[Modelo] CHECK CONSTRAINT [FK_Modelo_Marca]
GO
ALTER TABLE [dbo].[MovimientoInventario]  WITH CHECK ADD  CONSTRAINT [FK_MovimientoInventario_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[MovimientoInventario] CHECK CONSTRAINT [FK_MovimientoInventario_Producto]
GO
ALTER TABLE [dbo].[Notificacion]  WITH CHECK ADD  CONSTRAINT [FK_Notificacion_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
GO
ALTER TABLE [dbo].[Notificacion] CHECK CONSTRAINT [FK_Notificacion_Usuario]
GO
ALTER TABLE [dbo].[OrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajo_Cotizacion] FOREIGN KEY([IdCotizacion])
REFERENCES [dbo].[Cotizacion] ([IdCotizacion])
GO
ALTER TABLE [dbo].[OrdenTrabajo] CHECK CONSTRAINT [FK_OrdenTrabajo_Cotizacion]
GO
ALTER TABLE [dbo].[OrdenTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajo_Vehiculo] FOREIGN KEY([IdVehiculo])
REFERENCES [dbo].[Vehiculo] ([IdVehiculo])
GO
ALTER TABLE [dbo].[OrdenTrabajo] CHECK CONSTRAINT [FK_OrdenTrabajo_Vehiculo]
GO
ALTER TABLE [dbo].[OrdenTrabajoEmpleado]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEmpleado_Empleado] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[OrdenTrabajoEmpleado] CHECK CONSTRAINT [FK_OrdenTrabajoEmpleado_Empleado]
GO
ALTER TABLE [dbo].[OrdenTrabajoEmpleado]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoEmpleado_Orden] FOREIGN KEY([IdOrdenTrabajo])
REFERENCES [dbo].[OrdenTrabajo] ([IdOrdenTrabajo])
GO
ALTER TABLE [dbo].[OrdenTrabajoEmpleado] CHECK CONSTRAINT [FK_OrdenTrabajoEmpleado_Orden]
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoProducto_Orden] FOREIGN KEY([IdOrdenTrabajo])
REFERENCES [dbo].[OrdenTrabajo] ([IdOrdenTrabajo])
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto] CHECK CONSTRAINT [FK_OrdenTrabajoProducto_Orden]
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoProducto_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto] CHECK CONSTRAINT [FK_OrdenTrabajoProducto_Producto]
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoServicio_Orden] FOREIGN KEY([IdOrdenTrabajo])
REFERENCES [dbo].[OrdenTrabajo] ([IdOrdenTrabajo])
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio] CHECK CONSTRAINT [FK_OrdenTrabajoServicio_Orden]
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio]  WITH CHECK ADD  CONSTRAINT [FK_OrdenTrabajoServicio_Servicio] FOREIGN KEY([IdServicio])
REFERENCES [dbo].[Servicio] ([IdServicio])
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio] CHECK CONSTRAINT [FK_OrdenTrabajoServicio_Servicio]
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Factura] FOREIGN KEY([IdFactura])
REFERENCES [dbo].[Factura] ([IdFactura])
GO
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Factura]
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
GO
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Usuario]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Categoria] FOREIGN KEY([IdCategoriaProducto])
REFERENCES [dbo].[CategoriaProducto] ([IdCategoriaProducto])
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Categoria]
GO
ALTER TABLE [dbo].[Recepcion]  WITH CHECK ADD  CONSTRAINT [FK_Recepcion_Empleado] FOREIGN KEY([IdEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Recepcion] CHECK CONSTRAINT [FK_Recepcion_Empleado]
GO
ALTER TABLE [dbo].[Recepcion]  WITH CHECK ADD  CONSTRAINT [FK_Recepcion_Vehiculo] FOREIGN KEY([IdVehiculo])
REFERENCES [dbo].[Vehiculo] ([IdVehiculo])
GO
ALTER TABLE [dbo].[Recepcion] CHECK CONSTRAINT [FK_Recepcion_Vehiculo]
GO
ALTER TABLE [dbo].[Vehiculo]  WITH CHECK ADD  CONSTRAINT [FK_Vehiculo_Cliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Cliente] ([IdCliente])
GO
ALTER TABLE [dbo].[Vehiculo] CHECK CONSTRAINT [FK_Vehiculo_Cliente]
GO
ALTER TABLE [dbo].[Vehiculo]  WITH CHECK ADD  CONSTRAINT [FK_Vehiculo_Marca] FOREIGN KEY([IdMarca])
REFERENCES [dbo].[Marca] ([IdMarca])
GO
ALTER TABLE [dbo].[Vehiculo] CHECK CONSTRAINT [FK_Vehiculo_Marca]
GO
ALTER TABLE [dbo].[Vehiculo]  WITH CHECK ADD  CONSTRAINT [FK_Vehiculo_Modelo] FOREIGN KEY([IdModelo])
REFERENCES [dbo].[Modelo] ([IdModelo])
GO
ALTER TABLE [dbo].[Vehiculo] CHECK CONSTRAINT [FK_Vehiculo_Modelo]
GO
ALTER TABLE [dbo].[Vehiculo]  WITH CHECK ADD  CONSTRAINT [FK_Vehiculo_TipoCombustible] FOREIGN KEY([IdTipoCombustible])
REFERENCES [dbo].[TipoCombustible] ([IdTipoCombustible])
GO
ALTER TABLE [dbo].[Vehiculo] CHECK CONSTRAINT [FK_Vehiculo_TipoCombustible]
GO
ALTER TABLE [dbo].[Vehiculo]  WITH CHECK ADD  CONSTRAINT [FK_Vehiculo_TipoVehiculo] FOREIGN KEY([IdTipoVehiculo])
REFERENCES [dbo].[TipoVehiculo] ([IdTipoVehiculo])
GO
ALTER TABLE [dbo].[Vehiculo] CHECK CONSTRAINT [FK_Vehiculo_TipoVehiculo]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Cajero] FOREIGN KEY([IdCajero])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Cajero]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Cliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Cliente] ([IdCliente])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Cliente]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[IdentityUsuarios] ([Id])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Usuario]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Vendedor] FOREIGN KEY([IdVendedor])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Vendedor]
GO
ALTER TABLE [dbo].[Cita]  WITH CHECK ADD  CONSTRAINT [CK_Cita_Fecha] CHECK  (([FechaFin]>[FechaInicio]))
GO
ALTER TABLE [dbo].[Cita] CHECK CONSTRAINT [CK_Cita_Fecha]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [CK_DetalleCompra_Cantidad] CHECK  (([Cantidad]>(0)))
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [CK_DetalleCompra_Cantidad]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [CK_DetalleCompra_Costo] CHECK  (([CostoUnitario]>=(0)))
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [CK_DetalleCompra_Costo]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [CK_DetalleCompra_Descuento] CHECK  (([Descuento]>=(0)))
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [CK_DetalleCompra_Descuento]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [CK_DetalleCompra_Impuesto] CHECK  (([Impuesto]>=(0)))
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [CK_DetalleCompra_Impuesto]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [CK_DetalleCompra_Subtotal] CHECK  (([Subtotal]>=(0)))
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [CK_DetalleCompra_Subtotal]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [CK_DetalleFactura_Cantidad] CHECK  (([Cantidad]>(0)))
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [CK_DetalleFactura_Cantidad]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [CK_DetalleFactura_Descuento] CHECK  (([Descuento]>=(0)))
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [CK_DetalleFactura_Descuento]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [CK_DetalleFactura_Impuesto] CHECK  (([Impuesto]>=(0)))
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [CK_DetalleFactura_Impuesto]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [CK_DetalleFactura_Precio] CHECK  (([PrecioUnitario]>=(0)))
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [CK_DetalleFactura_Precio]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [CK_DetalleFactura_Subtotal] CHECK  (([Subtotal]>=(0)))
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [CK_DetalleFactura_Subtotal]
GO
ALTER TABLE [dbo].[DetalleFactura]  WITH CHECK ADD  CONSTRAINT [CK_DetalleFactura_Tipo] CHECK  (([IdProducto] IS NOT NULL AND [IdServicio] IS NULL OR [IdProducto] IS NULL AND [IdServicio] IS NOT NULL))
GO
ALTER TABLE [dbo].[DetalleFactura] CHECK CONSTRAINT [CK_DetalleFactura_Tipo]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Cantidad] CHECK  (([Cantidad]>(0)))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Cantidad]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Descuento] CHECK  (([Descuento]>=(0)))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Descuento]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Impuesto] CHECK  (([Impuesto]>=(0)))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Impuesto]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Precio] CHECK  (([PrecioUnitario]>=(0)))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Precio]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [CK_DetalleVenta_Subtotal] CHECK  (([Subtotal]>=(0)))
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [CK_DetalleVenta_Subtotal]
GO
ALTER TABLE [dbo].[EntregaVehiculo]  WITH CHECK ADD  CONSTRAINT [CK_EntregaVehiculo_Kilometraje] CHECK  (([KilometrajeSalida] IS NULL OR [KilometrajeSalida]>=(0)))
GO
ALTER TABLE [dbo].[EntregaVehiculo] CHECK CONSTRAINT [CK_EntregaVehiculo_Kilometraje]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [CK_Factura_Descuento] CHECK  (([Descuento]>=(0)))
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [CK_Factura_Descuento]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [CK_Factura_Impuesto] CHECK  (([Impuesto]>=(0)))
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [CK_Factura_Impuesto]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [CK_Factura_Origen] CHECK  (([IdOrdenTrabajo] IS NOT NULL OR [IdVenta] IS NOT NULL))
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [CK_Factura_Origen]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [CK_Factura_Subtotal] CHECK  (([Subtotal]>=(0)))
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [CK_Factura_Subtotal]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [CK_Factura_Total] CHECK  (([Total]>=(0)))
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [CK_Factura_Total]
GO
ALTER TABLE [dbo].[Garantia]  WITH CHECK ADD  CONSTRAINT [CK_Garantia_Cobertura] CHECK  (([IdProducto] IS NOT NULL OR [IdServicio] IS NOT NULL))
GO
ALTER TABLE [dbo].[Garantia] CHECK CONSTRAINT [CK_Garantia_Cobertura]
GO
ALTER TABLE [dbo].[Garantia]  WITH CHECK ADD  CONSTRAINT [CK_Garantia_Fechas] CHECK  (([FechaVencimiento]>=[FechaInicio]))
GO
ALTER TABLE [dbo].[Garantia] CHECK CONSTRAINT [CK_Garantia_Fechas]
GO
ALTER TABLE [dbo].[Garantia]  WITH CHECK ADD  CONSTRAINT [CK_Garantia_Origen] CHECK  (([IdOrdenTrabajo] IS NOT NULL OR [IdVenta] IS NOT NULL))
GO
ALTER TABLE [dbo].[Garantia] CHECK CONSTRAINT [CK_Garantia_Origen]
GO
ALTER TABLE [dbo].[OrdenTrabajoEmpleado]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoEmpleado_Horas] CHECK  (([HorasTrabajadas] IS NULL OR [HorasTrabajadas]>=(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoEmpleado] CHECK CONSTRAINT [CK_OrdenTrabajoEmpleado_Horas]
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoProducto_Cantidad] CHECK  (([Cantidad]>(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto] CHECK CONSTRAINT [CK_OrdenTrabajoProducto_Cantidad]
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoProducto_Descuento] CHECK  (([Descuento]>=(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto] CHECK CONSTRAINT [CK_OrdenTrabajoProducto_Descuento]
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoProducto_Precio] CHECK  (([PrecioUnitario]>=(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto] CHECK CONSTRAINT [CK_OrdenTrabajoProducto_Precio]
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoProducto_Subtotal] CHECK  (([Subtotal]>=(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoProducto] CHECK CONSTRAINT [CK_OrdenTrabajoProducto_Subtotal]
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoServicio_Cantidad] CHECK  (([Cantidad]>(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio] CHECK CONSTRAINT [CK_OrdenTrabajoServicio_Cantidad]
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoServicio_Descuento] CHECK  (([Descuento]>=(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio] CHECK CONSTRAINT [CK_OrdenTrabajoServicio_Descuento]
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoServicio_Precio] CHECK  (([PrecioUnitario]>=(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio] CHECK CONSTRAINT [CK_OrdenTrabajoServicio_Precio]
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio]  WITH CHECK ADD  CONSTRAINT [CK_OrdenTrabajoServicio_Subtotal] CHECK  (([Subtotal]>=(0)))
GO
ALTER TABLE [dbo].[OrdenTrabajoServicio] CHECK CONSTRAINT [CK_OrdenTrabajoServicio_Subtotal]
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [CK_Pago_Monto] CHECK  (([Monto]>(0)))
GO
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [CK_Pago_Monto]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [CK_Producto_Stock] CHECK  (([Stock]>=(0)))
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [CK_Producto_Stock]
GO
ALTER TABLE [dbo].[Servicio]  WITH CHECK ADD  CONSTRAINT [CK_Servicio_Precio] CHECK  (([Precio]>=(0) AND [Precio]<=(999999)))
GO
ALTER TABLE [dbo].[Servicio] CHECK CONSTRAINT [CK_Servicio_Precio]
GO
ALTER TABLE [dbo].[Vehiculo]  WITH CHECK ADD  CONSTRAINT [CK_Vehiculo_Anio] CHECK  (([Anio] IS NULL OR [Anio]>=(1900) AND [Anio]<=(2100)))
GO
ALTER TABLE [dbo].[Vehiculo] CHECK CONSTRAINT [CK_Vehiculo_Anio]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [CK_Venta_Descuento] CHECK  (([Descuento]>=(0)))
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [CK_Venta_Descuento]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [CK_Venta_Impuesto] CHECK  (([Impuesto]>=(0)))
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [CK_Venta_Impuesto]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [CK_Venta_Subtotal] CHECK  (([Subtotal]>=(0)))
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [CK_Venta_Subtotal]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [CK_Venta_Total] CHECK  (([Total]>=(0)))
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [CK_Venta_Total]
GO
/****** Object:  StoredProcedure [dbo].[sp_CambiarEstadoOrden]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* =============================================================
   ACTUALIZAR PROCEDIMIENTO
   sp_CambiarEstadoOrden

   Objetivo:
   - Cambiar el estado de la orden
   - Registrar historial
   - Actualizar FechaFin cuando corresponda
   - Mantener la transacción
   - Permitir que el trigger genere la auditoría
   ============================================================= */

CREATE PROCEDURE [dbo].[sp_CambiarEstadoOrden]
(
    @IdOrdenTrabajo INT,
    @NuevoEstado NVARCHAR(50),
    @UsuarioId NVARCHAR(450) = NULL,
    @Observaciones NVARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        DECLARE @EstadoAnterior NVARCHAR(50);
        DECLARE @FechaFinAnterior DATETIME2;

        /* =====================================================
           OBTENER ESTADO ACTUAL
           ===================================================== */

        SELECT
            @EstadoAnterior = Estado,
            @FechaFinAnterior = FechaFin
        FROM dbo.OrdenTrabajo
        WHERE IdOrdenTrabajo = @IdOrdenTrabajo;


        /* =====================================================
           VALIDAR EXISTENCIA
           ===================================================== */

        IF @EstadoAnterior IS NULL
        BEGIN
            THROW 50010,
                  'La orden de trabajo no existe.',
                  1;
        END;


        /* =====================================================
           VALIDAR NUEVO ESTADO
           ===================================================== */

        IF NULLIF(LTRIM(RTRIM(@NuevoEstado)), '') IS NULL
        BEGIN
            THROW 50011,
                  'El nuevo estado es obligatorio.',
                  1;
        END;


        /* =====================================================
           ACTUALIZAR ORDEN

           Si queda Finalizada:
               FechaFin = fecha/hora actual

           Si pasa a cualquier otro estado:
               FechaFin = NULL
           ===================================================== */

        UPDATE dbo.OrdenTrabajo
        SET
            Estado = @NuevoEstado,
            FechaFin =
                CASE
                    WHEN @NuevoEstado = N'Finalizada'
                        THEN SYSDATETIME()
                    ELSE NULL
                END
        WHERE IdOrdenTrabajo = @IdOrdenTrabajo;


        /* =====================================================
           REGISTRAR HISTORIAL
           ===================================================== */

        INSERT INTO dbo.HistorialOrdenTrabajo
        (
            IdOrdenTrabajo,
            EstadoAnterior,
            EstadoNuevo,
            FechaCambio,
            UsuarioId,
            Observaciones
        )
        VALUES
        (
            @IdOrdenTrabajo,
            @EstadoAnterior,
            @NuevoEstado,
            SYSDATETIME(),
            @UsuarioId,
            @Observaciones
        );


        /* =====================================================
           CONFIRMAR TRANSACCIÓN
           ===================================================== */

        COMMIT TRANSACTION;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_RegistrarMovimientoInventario]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   10. PROCEDIMIENTO — REGISTRAR MOVIMIENTO DE INVENTARIO
   ============================================================= */

CREATE   PROCEDURE [dbo].[sp_RegistrarMovimientoInventario]
(
    @IdProducto INT,
    @TipoMovimiento NVARCHAR(20),
    @Cantidad INT,
    @Observacion NVARCHAR(MAX) = NULL
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        IF @Cantidad <= 0
            THROW 50001,
                  'La cantidad debe ser mayor que cero.',
                  1;

        IF @TipoMovimiento NOT IN
        (
            'Entrada',
            'Salida',
            'Ajuste'
        )
        BEGIN
            THROW 50002,
                  'Tipo de movimiento no válido.',
                  1;
        END;


        DECLARE @StockActual INT;

        SELECT
            @StockActual = Stock
        FROM dbo.Producto WITH (UPDLOCK, ROWLOCK)
        WHERE IdProducto = @IdProducto;

        IF @StockActual IS NULL
        BEGIN
            THROW 50003,
                  'El producto no existe.',
                  1;
        END;


        DECLARE @NuevoStock INT;

        SET @NuevoStock =
            CASE
                WHEN @TipoMovimiento = 'Entrada'
                    THEN @StockActual + @Cantidad

                WHEN @TipoMovimiento = 'Salida'
                    THEN @StockActual - @Cantidad

                WHEN @TipoMovimiento = 'Ajuste'
                    THEN @Cantidad
            END;


        IF @NuevoStock < 0
        BEGIN
            THROW 50004,
                  'El inventario no puede quedar negativo.',
                  1;
        END;


        UPDATE dbo.Producto
        SET Stock = @NuevoStock
        WHERE IdProducto = @IdProducto;


        INSERT INTO dbo.MovimientoInventario
        (
            IdProducto,
            TipoMovimiento,
            Cantidad,
            FechaMovimiento,
            Observacion
        )
        VALUES
        (
            @IdProducto,
            @TipoMovimiento,
            @Cantidad,
            SYSDATETIME(),
            @Observacion
        );


        COMMIT TRANSACTION;

        SELECT
            @IdProducto AS IdProducto,
            @StockActual AS StockAnterior,
            @NuevoStock AS StockNuevo,
            @TipoMovimiento AS TipoMovimiento,
            @Cantidad AS Cantidad;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;

END;
GO
/****** Object:  StoredProcedure [dbo].[sp_RegistrarPago]    Script Date: 11/8/2026 12:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   12. PROCEDIMIENTO — REGISTRAR PAGO
   ============================================================= */

CREATE   PROCEDURE [dbo].[sp_RegistrarPago]
(
    @IdFactura INT,
    @Monto DECIMAL(12,2),
    @FormaPago NVARCHAR(30),
    @NumeroReferencia NVARCHAR(100) = NULL,
    @UsuarioId NVARCHAR(450) = NULL,
    @Observaciones NVARCHAR(500) = NULL
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;


        IF @Monto <= 0
        BEGIN
            THROW 50020,
                  'El monto del pago debe ser mayor que cero.',
                  1;
        END;


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Factura
            WHERE IdFactura = @IdFactura
        )
        BEGIN
            THROW 50021,
                  'La factura no existe.',
                  1;
        END;


        DECLARE @Saldo DECIMAL(12,2);

        SET @Saldo =
            dbo.fn_SaldoFactura(@IdFactura);


        IF @Monto > @Saldo
        BEGIN
            THROW 50022,
                  'El pago no puede superar el saldo pendiente.',
                  1;
        END;


        INSERT INTO dbo.Pago
        (
            IdFactura,
            Monto,
            FormaPago,
            NumeroReferencia,
            FechaPago,
            UsuarioId,
            Observaciones
        )
        VALUES
        (
            @IdFactura,
            @Monto,
            @FormaPago,
            @NumeroReferencia,
            SYSDATETIME(),
            @UsuarioId,
            @Observaciones
        );


        DECLARE @NuevoSaldo DECIMAL(12,2);

        SET @NuevoSaldo =
            dbo.fn_SaldoFactura(@IdFactura);


        UPDATE dbo.Factura
        SET Estado =
            CASE
                WHEN @NuevoSaldo <= 0
                    THEN 'Pagada'

                WHEN @NuevoSaldo < Total
                    THEN 'Parcialmente pagada'

                ELSE 'Pendiente'
            END
        WHERE IdFactura = @IdFactura;


        COMMIT TRANSACTION;


        SELECT
            @IdFactura AS IdFactura,
            @Monto AS PagoRegistrado,
            @NuevoSaldo AS SaldoPendiente;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;

END;
GO
USE [master]
GO
ALTER DATABASE [SistemaTallerDB] SET  READ_WRITE 
GO
