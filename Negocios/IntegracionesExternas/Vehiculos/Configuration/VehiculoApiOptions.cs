namespace Negocios.IntegracionesExternas.Vehiculos.Configuration
{
    public class VehiculoApiOptions
    {
        public string BaseUrl { get; set; } =
            "https://vpic.nhtsa.dot.gov/api/vehicles/";
    }
}