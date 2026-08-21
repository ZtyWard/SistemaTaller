using System.Net.Http.Json;
using System.Text.Json;
using Negocios.DTOs;
using Negocios.Interfaces;

namespace Negocios.Services;

public class VehiculoCatalogoApiService
    : IVehiculoCatalogoApiService
{
    private readonly HttpClient _httpClient;

    public VehiculoCatalogoApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<string>> BuscarMarcasAsync(
        string texto,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return Enumerable.Empty<string>();

        try
        {
            var url =
                "https://vpic.nhtsa.dot.gov/api/vehicles/" +
                "GetMakesForVehicleType/car?format=json";

            var respuesta =
                await _httpClient.GetFromJsonAsync<NhtsaResponse>(
                    url,
                    cancellationToken);

            if (respuesta?.Results == null)
                return Enumerable.Empty<string>();

            return respuesta.Results
                .Select(x => x.MakeName)
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x) &&
                    x.Contains(
                        texto.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .Take(10)
                .ToList();
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    public async Task<IEnumerable<VehiculoApiResultadoDto>>
        BuscarModelosAsync(
            string marca,
            string texto,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(marca) ||
            string.IsNullOrWhiteSpace(texto))
        {
            return Enumerable.Empty<VehiculoApiResultadoDto>();
        }

        try
        {
            var marcaEncoded =
                Uri.EscapeDataString(marca.Trim());

            var url =
                $"https://vpic.nhtsa.dot.gov/api/vehicles/" +
                $"GetModelsForMake/{marcaEncoded}?format=json";

            var respuesta =
                await _httpClient.GetFromJsonAsync<NhtsaResponse>(
                    url,
                    cancellationToken);

            if (respuesta?.Results == null)
                return Enumerable.Empty<VehiculoApiResultadoDto>();

            var modelos =
                respuesta.Results
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x.ModelName) &&
                        x.ModelName.Contains(
                            texto.Trim(),
                            StringComparison.OrdinalIgnoreCase))
                    .GroupBy(
                        x => x.ModelName.Trim(),
                        StringComparer.OrdinalIgnoreCase)
                    .Select(x => x.First())
                    .Take(15)
                    .ToList();

            var resultados =
                new List<VehiculoApiResultadoDto>();

            foreach (var modelo in modelos)
            {
                var nombreModelo =
                    modelo.ModelName.Trim();

                var imagen =
                    await BuscarImagenWikimediaAsync(
                        marca.Trim(),
                        nombreModelo,
                        cancellationToken);

                resultados.Add(
                    new VehiculoApiResultadoDto
                    {
                        Marca = marca.Trim(),
                        Modelo = nombreModelo,
                        ImagenUrl = imagen,
                        FuenteImagen =
                            imagen == null
                                ? null
                                : "Wikimedia Commons",
                        Descripcion =
                            $"Modelo {marca.Trim()} {nombreModelo}"
                    });
            }

            return resultados;
        }
        catch
        {
            return Enumerable.Empty<VehiculoApiResultadoDto>();
        }
    }

    private async Task<string?> BuscarImagenWikimediaAsync(
        string marca,
        string modelo,
        CancellationToken cancellationToken)
    {
        try
        {
            var consulta =
                Uri.EscapeDataString(
                    $"{marca} {modelo} car");

            var url =
                "https://en.wikipedia.org/w/rest.php/v1/" +
                $"search/page?q={consulta}&limit=3";

            using var request =
                new HttpRequestMessage(
                    HttpMethod.Get,
                    url);

            request.Headers.UserAgent.ParseAdd(
                "AXIS-Taller/1.0");

            using var response =
                await _httpClient.SendAsync(
                    request,
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
                return null;

            await using var stream =
                await response.Content.ReadAsStreamAsync(
                    cancellationToken);

            var resultado =
                await JsonSerializer.DeserializeAsync<
                    WikimediaSearchResponse>(
                    stream,
                    cancellationToken: cancellationToken);

            return resultado?.Pages?
                .Where(x => x.Thumbnail != null)
                .Select(x => x.Thumbnail!.Url)
                .FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    private sealed class NhtsaResponse
    {
        public List<NhtsaResult>? Results { get; set; }
    }

    private sealed class NhtsaResult
    {
        public string MakeName { get; set; } = string.Empty;

        public string ModelName { get; set; } = string.Empty;
    }

    private sealed class WikimediaSearchResponse
    {
        public List<WikimediaPage>? Pages { get; set; }
    }

    private sealed class WikimediaPage
    {
        public WikimediaThumbnail? Thumbnail { get; set; }
    }

    private sealed class WikimediaThumbnail
    {
        public string? Url { get; set; }
    }
}