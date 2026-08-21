using System.Text.Json;
using Negocios.IntegracionesExternas.Vehiculos.DTOs;
using Negocios.IntegracionesExternas.Vehiculos.Interfaces;

namespace Negocios.IntegracionesExternas.Vehiculos.Services
{
    public class VehiculoApiService : IVehiculoApiService
    {
        private readonly HttpClient _httpClient;

        private const string BaseUrl =
            "https://vpic.nhtsa.dot.gov/api/vehicles/";

        private readonly JsonSerializerOptions _jsonOptions =
            new()
            {
                PropertyNameCaseInsensitive = true
            };

        public VehiculoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // =====================================================
        // BUSCAR MODELOS POR MARCA
        // =====================================================

        public async Task<IEnumerable<VehiculoApiResultadoDto>> BuscarAsync(
            string consulta,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(consulta))
            {
                return Enumerable.Empty<VehiculoApiResultadoDto>();
            }

            var url =
                $"{BaseUrl}GetModelsForMake/" +
                $"{Uri.EscapeDataString(consulta)}" +
                "?format=json";

            using var response =
                await _httpClient.GetAsync(
                    url,
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<VehiculoApiResultadoDto>();
            }

            var contenido =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            var resultado =
                JsonSerializer.Deserialize<VehiculoApiRespuestaDto>(
                    contenido,
                    _jsonOptions);

            return resultado?.Results
                ?? Enumerable.Empty<VehiculoApiResultadoDto>();
        }

        // =====================================================
        // BUSCAR MODELOS POR MARCA Y AÑO
        // =====================================================

        public async Task<IEnumerable<VehiculoApiResultadoDto>>
            BuscarPorMarcaYAñoAsync(
                string marca,
                int año,
                CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(marca))
            {
                return Enumerable.Empty<VehiculoApiResultadoDto>();
            }

            if (año < 1900 || año > DateTime.UtcNow.Year + 1)
            {
                return Enumerable.Empty<VehiculoApiResultadoDto>();
            }

            var url =
                $"{BaseUrl}GetModelsForMakeYear/" +
                $"make/{Uri.EscapeDataString(marca)}/" +
                $"modelyear/{año}" +
                "?format=json";

            using var response =
                await _httpClient.GetAsync(
                    url,
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<VehiculoApiResultadoDto>();
            }

            var contenido =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            var resultado =
                JsonSerializer.Deserialize<VehiculoApiRespuestaDto>(
                    contenido,
                    _jsonOptions);

            if (resultado?.Results == null)
            {
                return Enumerable.Empty<VehiculoApiResultadoDto>();
            }

            foreach (var modelo in resultado.Results)
            {
                modelo.ModelYear = año;
            }

            return resultado.Results;
        }

        // =====================================================
        // OBTENER TIPOS DE VEHÍCULO POR MARCA
        // =====================================================

        public async Task<IEnumerable<VehiculoApiTipoDto>>
            ObtenerTiposPorMarcaAsync(
                string marca,
                CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(marca))
            {
                return Enumerable.Empty<VehiculoApiTipoDto>();
            }

            var url =
                $"{BaseUrl}GetVehicleTypesForMake/" +
                $"{Uri.EscapeDataString(marca)}" +
                "?format=json";

            using var response =
                await _httpClient.GetAsync(
                    url,
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Enumerable.Empty<VehiculoApiTipoDto>();
            }

            var contenido =
                await response.Content.ReadAsStringAsync(
                    cancellationToken);

            var resultado =
                JsonSerializer.Deserialize<VehiculoApiTiposRespuestaDto>(
                    contenido,
                    _jsonOptions);

            return resultado?.Results
                ?? Enumerable.Empty<VehiculoApiTipoDto>();
        }
    }
}