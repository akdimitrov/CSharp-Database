using Newtonsoft.Json;

namespace CarDealer.DTOs.Car
{
    [JsonObject]
    public class ExportCarDto
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }
    }
}
