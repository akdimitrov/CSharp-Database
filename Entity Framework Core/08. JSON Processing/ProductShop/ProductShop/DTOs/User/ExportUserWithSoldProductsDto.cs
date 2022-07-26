using Newtonsoft.Json;
using ProductShop.DTOs.Products;

namespace ProductShop.DTOs.User
{
    [JsonObject]
    public class ExportUserWithSoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ExportUserSoldProductsDto[] SoldProducts { get; set; }
    }
}
