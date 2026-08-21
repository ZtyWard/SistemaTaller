using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocios.Interfaces;
using Negocios.Seguridad;

namespace SistemaTaller.Controllers;

[Authorize(Policy = Permisos.ProductosVer)]
public class ScannerController : Controller
{
    private readonly IProductoService _productoService;

    public ScannerController(
        IProductoService productoService)
    {
        _productoService = productoService;
    }

    // =====================================================
    // SCANNER
    // =====================================================

    [HttpGet]
    public IActionResult Index()
    {
        ViewData["Title"] = "Scanner de productos";

        return View();
    }

    // =====================================================
    // BUSCAR PRODUCTO POR CÓDIGO DE BARRAS / QR
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> Buscar(string? codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
        {
            return BadRequest(new
            {
                encontrado = false,
                mensaje = "No se recibió ningún código."
            });
        }

        codigo = codigo.Trim();

        if (codigo.Length > 100)
        {
            return BadRequest(new
            {
                encontrado = false,
                mensaje = "El código recibido no es válido."
            });
        }

        var producto =
            await _productoService
                .ObtenerPorCodigoBarrasAsync(codigo);

        if (producto == null)
        {
            return NotFound(new
            {
                encontrado = false,
                mensaje = "No se encontró ningún producto con ese código."
            });
        }

        return Json(new
        {
            encontrado = true,

            idProducto = producto.IdProducto,

            codigo = producto.Codigo,

            codigoBarras = producto.CodigoBarras,

            nombre = producto.Nombre,

            descripcion = producto.Descripcion,

            categoria = producto.Categoria,

            precioCompra = producto.PrecioCompra,

            precioVenta = producto.PrecioVenta,

            stock = producto.Stock,

            stockMinimo = producto.StockMinimo,

            activo = producto.Activo,

            imagenUrl = producto.ImagenUrl
        });
    }
}