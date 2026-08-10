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

    public CompraService(
        ICompraRepository repository,
        IProveedorRepository proveedorRepository)
    {
        _repository = repository;
        _proveedorRepository =
            proveedorRepository;
    }

    public async Task<IEnumerable<CompraDto>>
        ObtenerTodosAsync()
    {
        var compras =
            await _repository.ObtenerTodosAsync();

        return compras.Select(MapearDto);
    }

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

    public async Task<CompraDto?>
        ObtenerPorIdAsync(int id)
    {
        var compra =
            await _repository
                .ObtenerPorIdAsync(id);

        return compra == null
            ? null
            : MapearDto(compra);
    }

    public async Task CrearAsync(
        CompraGuardarDto dto)
    {
        Validar(dto);

        await ValidarProveedorAsync(
            dto.IdProveedor);

        var compra = new Compra
        {
            IdProveedor =
                dto.IdProveedor,

            FechaCompra =
                dto.FechaCompra == default
                    ? DateTime.Now
                    : dto.FechaCompra,

            Total =
                dto.Total,

            Estado =
                NormalizarEstado(
                    dto.Estado)
        };

        await _repository.AgregarAsync(
            compra);

        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        CompraGuardarDto dto)
    {
        Validar(dto);

        await ValidarProveedorAsync(
            dto.IdProveedor);

        var compra =
            await _repository
                .ObtenerPorIdAsync(id);

        if (compra == null)
            return false;

        compra.IdProveedor =
            dto.IdProveedor;

        compra.FechaCompra =
            dto.FechaCompra;

        compra.Total =
            dto.Total;

        compra.Estado =
            NormalizarEstado(
                dto.Estado);

        _repository.Actualizar(
            compra);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool>
        CambiarEstadoAsync(
            int id,
            string estado)
    {
        var compra =
            await _repository
                .ObtenerPorIdAsync(id);

        if (compra == null)
            return false;

        compra.Estado =
            NormalizarEstado(estado);

        _repository.Actualizar(
            compra);

        await _repository.GuardarCambiosAsync();

        return true;
    }

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

    private static void Validar(
        CompraGuardarDto dto)
    {
        if (dto.IdProveedor <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar un proveedor.");
        }

        if (dto.FechaCompra == default)
        {
            throw new ArgumentException(
                "La fecha de compra es obligatoria.");
        }

        if (dto.Total < 0)
        {
            throw new ArgumentException(
                "El total no puede ser negativo.");
        }

        NormalizarEstado(dto.Estado);
    }

    private static string
        NormalizarEstado(
            string estado)
    {
        var valor =
            estado.Trim()
                .ToLowerInvariant();

        return valor switch
        {
            "pendiente" => "Pendiente",
            "completada" => "Completada",
            "cancelada" => "Cancelada",

            _ => throw new ArgumentException(
                "El estado debe ser Pendiente, Completada o Cancelada.")
        };
    }

    private static CompraDto MapearDto(
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

            Total =
                compra.Total,

            Estado =
                compra.Estado
        };
    }
}