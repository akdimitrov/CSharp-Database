using CarDealer.DTOs.Part;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Car
{
    [JsonObject]
    public class ExportCarWithPartsDto
    {
        [JsonProperty("car")]
        public ExportCarDto Car { get; set; }

        [JsonProperty("parts")]
        public ExportPartDto[] Parts { get; set; }
    }
}
