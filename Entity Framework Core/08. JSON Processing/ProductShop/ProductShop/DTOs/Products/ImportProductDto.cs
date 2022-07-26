using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Products
{
    [JsonObject]
    public class ImportProductDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public float Price { get; set; }

        public int SellerId { get; set; }

        public int? BuyerId { get; set; }
    }
}
