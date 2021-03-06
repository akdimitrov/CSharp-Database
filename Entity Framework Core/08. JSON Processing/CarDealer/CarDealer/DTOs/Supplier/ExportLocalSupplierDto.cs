using Newtonsoft.Json;

namespace CarDealer.DTOs.Supplier
{
    [JsonObject]
    public class ExportLocalSupplierDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PartsCount { get; set; }
    }
}
