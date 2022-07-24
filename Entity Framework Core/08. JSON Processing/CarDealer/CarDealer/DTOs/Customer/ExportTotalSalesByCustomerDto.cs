using Newtonsoft.Json;

namespace CarDealer.DTOs.Customer
{
    [JsonObject]
    public class ExportTotalSalesByCustomerDto
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("boughtCars")]
        public int BoughtCars { get; set; }

        [JsonProperty("spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}
