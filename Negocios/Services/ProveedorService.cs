using Datos.Interfaces;
using Datos.Models;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class ProveedorService : IProveedorService
{
    private readonly IProveedorRepository _repository;

    public ProveedorService(
        IProveedorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProveedorDto>>
        ObtenerTodosAsync()
    {
        var proveedores =
            await _repository.ObtenerTodosAsync();

        return proveedores.Select(MapearDto);
    }

    public async Task<IEnumerable<ProveedorDto>>
        ObtenerActivosAsync()
    {
        var proveedores =
            await _repository.ObtenerActivosAsync();

        return proveedores.Select(MapearDto);
    }

    public async Task<ProveedorDto?>
        ObtenerPorIdAsync(int id)
    {
        var proveedor =
            await _repository.ObtenerPorIdAsync(id);

        return proveedor == null
            ? null
            : MapearDto(proveedor);
    }

    public async Task CrearAsync(
        ProveedorGuardarDto dto)
    {
        Validar(dto);

        if (!string.IsNullOrWhiteSpace(
                dto.CedulaJuridica))
        {
            var existente =
                await _repository
                    .ObtenerPorCedulaJuridicaAsync(
                        dto.CedulaJuridica.Trim());

            if (existente != null)
            {
                throw new ArgumentException(
                    "Ya existe un proveedor con esa cédula jurídica.");
            }
        }

        var proveedor = new Proveedor
        {
            Nombre = dto.Nombre.Trim(),

            CedulaJuridica =
                dto.CedulaJuridica?.Trim(),

            Telefono =
                dto.Telefono?.Trim(),

            Correo =
                dto.Correo?.Trim(),

            Direccion =
                dto.Direccion?.Trim(),

            Activo = dto.Activo
        };

        await _repository.AgregarAsync(
            proveedor);

        await _repository.GuardarCambiosAsync();
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ProveedorGuardarDto dto)
    {
        Validar(dto);

        var proveedor =
            await _repository.ObtenerPorIdAsync(id);

        if (proveedor == null)
            return false;

        if (!string.IsNullOrWhiteSpace(
                dto.CedulaJuridica))
        {
            var existente =
                await _repository
                    .ObtenerPorCedulaJuridicaAsync(
                        dto.CedulaJuridica.Trim());

            if (existente != null &&
                existente.IdProveedor != id)
            {
                throw new ArgumentException(
                    "Ya existe otro proveedor con esa cédula jurídica.");
            }
        }

        proveedor.Nombre =
            dto.Nombre.Trim();

        proveedor.CedulaJuridica =
            dto.CedulaJuridica?.Trim();

        proveedor.Telefono =
            dto.Telefono?.Trim();

        proveedor.Correo =
            dto.Correo?.Trim();

        proveedor.Direccion =
            dto.Direccion?.Trim();

        proveedor.Activo =
            dto.Activo;

        _repository.Actualizar(proveedor);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> CambiarEstadoAsync(
        int id,
        bool activo)
    {
        var proveedor =
            await _repository.ObtenerPorIdAsync(id);

        if (proveedor == null)
            return false;

        proveedor.Activo = activo;

        _repository.Actualizar(proveedor);

        await _repository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(
        ProveedorGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(
                dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre del proveedor es obligatorio.");
        }
    }

    private static ProveedorDto MapearDto(
        Proveedor proveedor)
    {
        return new ProveedorDto
        {
            IdProveedor =
                proveedor.IdProveedor,

            Nombre =
                proveedor.Nombre,

            CedulaJuridica =
                proveedor.CedulaJuridica,

            Telefono =
                proveedor.Telefono,

            Correo =
                proveedor.Correo,

            Direccion =
                proveedor.Direccion,

            Activo =
                proveedor.Activo
        };
    }
}