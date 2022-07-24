using Newtonsoft.Json;

namespace CarDealer.DTOs.Part
{
    [JsonObject]
    public class ExportPartDto
    {
        public string Name { get; set; }

        public string Price { get; set; }
    }
}
