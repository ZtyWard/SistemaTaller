using System.Text.Json.Serialization;

namespace Negocios.IntegracionesExternas.Vehiculos.DTOs
{
    public class VehiculoApiResultadoDto
    {
        [JsonPropertyName("Make_ID")]
        public int MakeId { get; set; }

        [JsonPropertyName("Make_Name")]
        public string MakeName { get; set; } = string.Empty;

        [JsonPropertyName("Model_ID")]
        public int ModelId { get; set; }

        [JsonPropertyName("Model_Name")]
        public string ModelName { get; set; } = string.Empty;

        public int? ModelYear { get; set; }

        public int? VehicleTypeId { get; set; }

        public string? VehicleTypeName { get; set; }
    }
}