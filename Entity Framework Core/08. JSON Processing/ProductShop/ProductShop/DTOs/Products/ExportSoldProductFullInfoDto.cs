using System.Linq;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Products
{
    [JsonObject]
    public class ExportSoldProductFullInfoDto
    {
        [JsonProperty("count")]
        public int Count => ProductsSold.Any() ? ProductsSold.Length : 0;

        [JsonProperty("products")]
        public ExportSoldProductInfoDto[] ProductsSold { get; set; }
    }
}
