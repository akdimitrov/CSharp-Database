﻿using Newtonsoft.Json;

namespace ProductShop.DTOs.CategoryProduct
{
    [JsonObject]
    public class ImportCategoryProductDto
    {
        public int CategoryId { get; set; }

        public int ProductId { get; set; }
    }
}
