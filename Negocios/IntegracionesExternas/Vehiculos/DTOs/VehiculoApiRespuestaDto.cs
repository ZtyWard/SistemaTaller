namespace Negocios.IntegracionesExternas.Vehiculos.DTOs
{
    public class VehiculoApiRespuestaDto
    {
        public int Count { get; set; }

        public string Message { get; set; } = string.Empty;

        public string SearchCriteria { get; set; } = string.Empty;

        public List<VehiculoApiResultadoDto> Results { get; set; }
            = new();
    }
}