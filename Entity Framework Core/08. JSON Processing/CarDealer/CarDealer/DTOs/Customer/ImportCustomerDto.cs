using System;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Customer
{
    [JsonObject]
    public class ImportCustomerDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
