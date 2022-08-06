namespace VaporStore.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                .ToList()
                .Where(x => genreNames.Contains(x.Name))
                .Select(x => new
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games.Select(g => new
                    {
                        Id = g.Id,
                        Title = g.Name,
                        Developer = g.Developer.Name,
                        Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
                        Players = g.Purchases.Count
                    })
                        .Where(g => g.Players > 0)
                        .OrderByDescending(x => x.Players)
                        .ThenBy(x => x.Id)
                        .ToList(),
                    TotalPlayers = x.Games.Sum(x => x.Purchases.Count())
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToList();

            string json = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return json;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var users = context.Users
                .ToList()
                .Where(x => x.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == storeType)))
                .Select(x => new ExportUserXmlDto
                {
                    Username = x.Username,
                    TotalSpent = x.Cards
                        .Sum(c => c.Purchases.Where(p => p.Type.ToString() == storeType)
                        .Sum(p => p.Game.Price)),
                    Purchases = x.Cards.SelectMany(c => c.Purchases)
                        .Where(x => x.Type.ToString() == storeType)
                        .OrderBy(x => x.Date)
                        .Select(p => new ExportPurchaseXmlDto
                        {
                            CardNumber = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameXmlDto
                            {
                                Name = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        }).ToArray()
                })
                .OrderByDescending(x => x.TotalSpent)
                .ThenBy(x => x.Username)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportUserXmlDto[]), new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            serializer.Serialize(writer, users, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}