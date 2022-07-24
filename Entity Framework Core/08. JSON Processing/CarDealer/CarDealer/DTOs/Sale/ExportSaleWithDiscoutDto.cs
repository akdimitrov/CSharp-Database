using CarDealer.DTOs.Car;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Sale
{
    [JsonObject]
    public class ExportSaleWithDiscoutDto
    {
        [JsonProperty("car")]
        public ExportCarDto Car { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        public string Discount { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("priceWithDiscount")]
        public string PriceWithDiscount { get; set; }
    }
}
