using Microsoft.AspNetCore.Mvc;
using Negocios.IntegracionesExternas.Vehiculos.Interfaces;

namespace SistemaTaller.Controllers
{
    public class CatalogoVehiculoApiController : Controller
    {
        private readonly IVehiculoApiService _vehiculoApiService;

        public CatalogoVehiculoApiController(
            IVehiculoApiService vehiculoApiService)
        {
            _vehiculoApiService = vehiculoApiService;
        }

        // =====================================================
        // BUSCAR MODELOS POR MARCA
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> BuscarModelos(
            string consulta,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(consulta))
            {
                return Json(new
                {
                    success = true,
                    resultados = Array.Empty<object>()
                });
            }

            var resultados =
                await _vehiculoApiService.BuscarAsync(
                    consulta,
                    cancellationToken);

            return Json(new
            {
                success = true,
                resultados
            });
        }

        // =====================================================
        // BUSCAR MODELOS POR MARCA Y AÑO
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> BuscarModelosPorAnio(
            string marca,
            int año,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(marca))
            {
                return Json(new
                {
                    success = false,
                    mensaje = "Debe indicar una marca.",
                    resultados = Array.Empty<object>()
                });
            }

            if (año < 1900 || año > DateTime.UtcNow.Year + 1)
            {
                return Json(new
                {
                    success = false,
                    mensaje = "El año indicado no es válido.",
                    resultados = Array.Empty<object>()
                });
            }

            var resultados =
                await _vehiculoApiService.BuscarPorMarcaYAñoAsync(
                    marca,
                    año,
                    cancellationToken);

            return Json(new
            {
                success = true,
                resultados
            });
        }

        // =====================================================
        // OBTENER TIPOS DE VEHÍCULO POR MARCA
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> ObtenerTipos(
            string marca,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(marca))
            {
                return Json(new
                {
                    success = false,
                    mensaje = "Debe indicar una marca.",
                    resultados = Array.Empty<object>()
                });
            }

            var resultados =
                await _vehiculoApiService.ObtenerTiposPorMarcaAsync(
                    marca,
                    cancellationToken);

            return Json(new
            {
                success = true,
                resultados
            });
        }
    }
}