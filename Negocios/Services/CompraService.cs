using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class CompraService : ICompraService
{
    private readonly ICompraRepository _repository;

    private readonly IProveedorRepository
        _proveedorRepository;

    private readonly IProductoRepository
        _productoRepository;

    public CompraService(
        ICompraRepository repository,
        IProveedorRepository proveedorRepository,
        IProductoRepository productoRepository)
    {
        _repository =
            repository;

        _proveedorRepository =
            proveedorRepository;

        _productoRepository =
            productoRepository;
    }

    // =====================================================
    // OBTENER TODOS
    // =====================================================

    public async Task<IEnumerable<CompraDto>>
        ObtenerTodosAsync()
    {
        var compras =
            await _repository
                .ObtenerTodosAsync();

        return compras.Select(MapearDto);
    }

    // =====================================================
    // POR PROVEEDOR
    // =====================================================

    public async Task<IEnumerable<CompraDto>>
        ObtenerPorProveedorAsync(
            int idProveedor)
    {
        var compras =
            await _repository
                .ObtenerPorProveedorAsync(
                    idProveedor);

        return compras.Select(MapearDto);
    }

    // =====================================================
    // POR ESTADO
    // =====================================================

    public async Task<IEnumerable<CompraDto>>
        ObtenerPorEstadoAsync(
            string estado)
    {
        var compras =
            await _repository
                .ObtenerPorEstadoAsync(
                    estado);

        return compras.Select(MapearDto);
    }

    // =====================================================
    // RECIENTES
    // =====================================================

    public async Task<IEnumerable<CompraDto>>
        ObtenerRecientesAsync(
            int cantidad = 10)
    {
        var compras =
            await _repository
                .ObtenerRecientesAsync(
                    cantidad);

        return compras.Select(MapearDto);
    }

    // =====================================================
    // POR ID
    // =====================================================

    public async Task<CompraDto?>
        ObtenerPorIdAsync(int id)
    {
        var compra =
            await _repository
                .ObtenerPorIdConDetallesAsync(
                    id);

        return compra == null
            ? null
            : MapearDto(compra);
    }

    // =====================================================
    // CREAR COMPRA
    // =====================================================

    public async Task CrearAsync(
        CompraGuardarDto dto)
    {
        await ValidarCompraAsync(dto);

        var calculo =
            CalcularTotales(dto.Detalles);

        var compra = new Compra
        {
            IdProveedor =
                dto.IdProveedor,

            FechaCompra =
                dto.FechaCompra == default
                    ? DateTime.Now
                    : dto.FechaCompra,

            Subtotal =
                calculo.Subtotal,

            Impuesto =
                calculo.Impuesto,

            Descuento =
                calculo.Descuento,

            Total =
                calculo.Total,

            Estado =
                NormalizarEstado(
                    dto.Estado),

            NumeroFacturaProveedor =
                dto.NumeroFacturaProveedor
                    ?.Trim(),

            FormaPago =
                dto.FormaPago
                    ?.Trim(),

            UsuarioId =
                dto.UsuarioId
        };

        foreach (var detalleDto
            in dto.Detalles)
        {
            var subtotal =
                (detalleDto.Cantidad *
                 detalleDto.CostoUnitario)
                + detalleDto.Impuesto
                - detalleDto.Descuento;

            compra.Detalles.Add(
                new DetalleCompra
                {
                    IdProducto =
                        detalleDto.IdProducto,

                    Cantidad =
                        detalleDto.Cantidad,

                    CostoUnitario =
                        detalleDto.CostoUnitario,

                    Impuesto =
                        detalleDto.Impuesto,

                    Descuento =
                        detalleDto.Descuento,

                    Subtotal =
                        subtotal
                });
        }

        await _repository.AgregarAsync(
            compra);

        await _repository
            .GuardarCambiosAsync();
    }

    // =====================================================
    // ACTUALIZAR
    // =====================================================

    public async Task<bool>
        ActualizarAsync(
            int id,
            CompraGuardarDto dto)
    {
        await ValidarCompraAsync(dto);

        var compra =
            await _repository
                .ObtenerPorIdConDetallesAsync(
                    id);

        if (compra == null)
            return false;

        if (compra.Estado == "Completada")
        {
            throw new InvalidOperationException(
                "No se puede modificar una compra completada.");
        }

        if (compra.Estado == "Cancelada")
        {
            throw new InvalidOperationException(
                "No se puede modificar una compra cancelada.");
        }

        var calculo =
            CalcularTotales(dto.Detalles);

        var nuevoEstado =
            NormalizarEstado(dto.Estado);

        // =================================================
        // SI SE QUIERE COMPLETAR DESDE EDICIÓN
        // =================================================

        var completarDespuesDeActualizar =
            nuevoEstado == "Completada";

        compra.IdProveedor =
            dto.IdProveedor;

        compra.FechaCompra =
            dto.FechaCompra;

        compra.Subtotal =
            calculo.Subtotal;

        compra.Impuesto =
            calculo.Impuesto;

        compra.Descuento =
            calculo.Descuento;

        compra.Total =
            calculo.Total;

        /*
         * Si el usuario seleccionó "Completada",
         * primero guardamos la compra como Pendiente.
         *
         * Después llamamos a CompletarCompraAsync(),
         * que es el método encargado de:
         *
         * 1. Registrar las entradas de inventario.
         * 2. Actualizar el stock.
         * 3. Cambiar la compra a Completada.
         *
         * Así evitamos simplemente cambiar el estado
         * sin ejecutar la integración con inventario.
         */

        compra.Estado =
            completarDespuesDeActualizar
                ? "Pendiente"
                : nuevoEstado;

        compra.NumeroFacturaProveedor =
            dto.NumeroFacturaProveedor
                ?.Trim();

        compra.FormaPago =
            dto.FormaPago
                ?.Trim();

        compra.UsuarioId =
            dto.UsuarioId;

        // =================================================
        // REEMPLAZAR DETALLES
        // =================================================

        compra.Detalles.Clear();

        foreach (var detalleDto
            in dto.Detalles)
        {
            var subtotal =
                (detalleDto.Cantidad *
                 detalleDto.CostoUnitario)
                + detalleDto.Impuesto
                - detalleDto.Descuento;

            compra.Detalles.Add(
                new DetalleCompra
                {
                    IdProducto =
                        detalleDto.IdProducto,

                    Cantidad =
                        detalleDto.Cantidad,

                    CostoUnitario =
                        detalleDto.CostoUnitario,

                    Impuesto =
                        detalleDto.Impuesto,

                    Descuento =
                        detalleDto.Descuento,

                    Subtotal =
                        subtotal
                });
        }

        _repository.Actualizar(
            compra);

        await _repository
            .GuardarCambiosAsync();

        // =================================================
        // COMPLETAR DESDE EDICIÓN
        // =================================================

        if (completarDespuesDeActualizar)
        {
            await _repository
                .CompletarCompraAsync(id);
        }

        return true;
    }

    // =====================================================
    // CAMBIAR ESTADO
    // =====================================================

    public async Task<bool>
        CambiarEstadoAsync(
            int id,
            string estado)
    {
        var nuevoEstado =
            NormalizarEstado(estado);

        var compra =
            await _repository
                .ObtenerPorIdConDetallesAsync(
                    id);

        if (compra == null)
            return false;

        // =================================================
        // COMPLETAR COMPRA
        // =================================================

        if (nuevoEstado == "Completada")
        {
            if (compra.Estado ==
                "Completada")
            {
                throw new InvalidOperationException(
                    "La compra ya está completada.");
            }

            await _repository
                .CompletarCompraAsync(id);

            return true;
        }

        // =================================================
        // CANCELAR
        // =================================================

        if (nuevoEstado == "Cancelada")
        {
            if (compra.Estado ==
                "Completada")
            {
                throw new InvalidOperationException(
                    "No se puede cancelar una compra completada.");
            }
        }

        // =================================================
        // PENDIENTE / CANCELADA
        // =================================================

        compra.Estado =
            nuevoEstado;

        _repository.Actualizar(
            compra);

        await _repository
            .GuardarCambiosAsync();

        return true;
    }

    // =====================================================
    // VALIDAR COMPRA
    // =====================================================

    private async Task
        ValidarCompraAsync(
            CompraGuardarDto dto)
    {
        if (dto.IdProveedor <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar un proveedor.");
        }

        await ValidarProveedorAsync(
            dto.IdProveedor);

        if (dto.FechaCompra == default)
        {
            dto.FechaCompra =
                DateTime.Now;
        }

        NormalizarEstado(
            dto.Estado);

        if (dto.Detalles == null ||
            dto.Detalles.Count == 0)
        {
            throw new ArgumentException(
                "La compra debe tener al menos un detalle.");
        }

        foreach (var detalle
            in dto.Detalles)
        {
            if (detalle.IdProducto <= 0)
            {
                throw new ArgumentException(
                    "Todos los detalles deben tener un producto.");
            }

            if (detalle.Cantidad <= 0)
            {
                throw new ArgumentException(
                    "La cantidad debe ser mayor que cero.");
            }

            if (detalle.CostoUnitario < 0)
            {
                throw new ArgumentException(
                    "El costo unitario no puede ser negativo.");
            }

            if (detalle.Impuesto < 0)
            {
                throw new ArgumentException(
                    "El impuesto no puede ser negativo.");
            }

            if (detalle.Descuento < 0)
            {
                throw new ArgumentException(
                    "El descuento no puede ser negativo.");
            }

            var producto =
                await _productoRepository
                    .ObtenerPorIdAsync(
                        detalle.IdProducto);

            if (producto == null)
            {
                throw new ArgumentException(
                    $"El producto {detalle.IdProducto} no existe.");
            }

            if (!producto.Activo)
            {
                throw new ArgumentException(
                    $"El producto '{producto.Nombre}' está inactivo.");
            }
        }
    }

    // =====================================================
    // VALIDAR PROVEEDOR
    // =====================================================

    private async Task
        ValidarProveedorAsync(
            int idProveedor)
    {
        if (idProveedor <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar un proveedor.");
        }

        var proveedor =
            await _proveedorRepository
                .ObtenerPorIdAsync(
                    idProveedor);

        if (proveedor == null)
        {
            throw new ArgumentException(
                "El proveedor seleccionado no existe.");
        }

        if (!proveedor.Activo)
        {
            throw new ArgumentException(
                "El proveedor seleccionado está inactivo.");
        }
    }

    // =====================================================
    // CALCULAR TOTALES
    // =====================================================

    private static (
        decimal Subtotal,
        decimal Impuesto,
        decimal Descuento,
        decimal Total)
        CalcularTotales(
            IEnumerable<DetalleCompraGuardarDto>
                detalles)
    {
        decimal subtotal = 0;
        decimal impuesto = 0;
        decimal descuento = 0;

        foreach (var detalle in detalles)
        {
            subtotal +=
                detalle.Cantidad *
                detalle.CostoUnitario;

            impuesto +=
                detalle.Impuesto;

            descuento +=
                detalle.Descuento;
        }

        var total =
            subtotal +
            impuesto -
            descuento;

        if (total < 0)
        {
            throw new ArgumentException(
                "El total de la compra no puede ser negativo.");
        }

        return (
            subtotal,
            impuesto,
            descuento,
            total);
    }

    // =====================================================
    // NORMALIZAR ESTADO
    // =====================================================

    private static string
        NormalizarEstado(
            string estado)
    {
        var valor =
            (estado ?? string.Empty)
                .Trim()
                .ToLowerInvariant();

        return valor switch
        {
            "pendiente" =>
                "Pendiente",

            "completada" =>
                "Completada",

            "cancelada" =>
                "Cancelada",

            _ =>
                throw new ArgumentException(
                    "El estado debe ser Pendiente, Completada o Cancelada.")
        };
    }

    // =====================================================
    // MAPEAR DTO
    // =====================================================

    private static CompraDto
        MapearDto(
            Compra compra)
    {
        return new CompraDto
        {
            IdCompra =
                compra.IdCompra,

            IdProveedor =
                compra.IdProveedor,

            Proveedor =
                compra.Proveedor?.Nombre
                ?? string.Empty,

            FechaCompra =
                compra.FechaCompra,

            Subtotal =
                compra.Subtotal,

            Impuesto =
                compra.Impuesto,

            Descuento =
                compra.Descuento,

            Total =
                compra.Total,

            Estado =
                compra.Estado,

            NumeroFacturaProveedor =
                compra.NumeroFacturaProveedor,

            FormaPago =
                compra.FormaPago,

            UsuarioId =
                compra.UsuarioId,

            Detalles =
                compra.Detalles
                    .Select(x =>
                        new DetalleCompraDto
                        {
                            IdDetalleCompra =
                                x.IdDetalleCompra,

                            IdCompra =
                                x.IdCompra,

                            IdProducto =
                                x.IdProducto,

                            Producto =
                                x.Producto?.Nombre
                                ?? string.Empty,

                            Cantidad =
                                x.Cantidad,

                            CostoUnitario =
                                x.CostoUnitario,

                            Impuesto =
                                x.Impuesto,

                            Descuento =
                                x.Descuento,

                            Subtotal =
                                x.Subtotal
                        })
                    .ToList()
        };
    }
}