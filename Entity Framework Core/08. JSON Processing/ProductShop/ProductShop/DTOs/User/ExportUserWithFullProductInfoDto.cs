using System.Linq;
using Newtonsoft.Json;

namespace ProductShop.DTOs.User
{
    [JsonObject]
    public class ExportUserWithFullProductInfoDto
    {
        [JsonProperty("usersCount")]
        public int UsersCount => Users.Any() ? Users.Length : 0;

        [JsonProperty("users")]
        public ExportUserInfoDto[] Users { get; set; }
    }
}
