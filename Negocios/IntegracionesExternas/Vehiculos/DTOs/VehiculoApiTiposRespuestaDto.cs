namespace Negocios.IntegracionesExternas.Vehiculos.DTOs
{
    public class VehiculoApiTiposRespuestaDto
    {
        public int Count { get; set; }

        public string Message { get; set; } = string.Empty;

        public string SearchCriteria { get; set; } = string.Empty;

        public List<VehiculoApiTipoDto> Results { get; set; }
            = new();
    }
}