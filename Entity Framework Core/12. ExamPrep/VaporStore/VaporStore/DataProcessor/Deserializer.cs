namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var gameDtos = JsonConvert.DeserializeObject<IEnumerable<ImportGameDto>>(jsonString);

            foreach (var gameDto in gameDtos)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var developer = context.Developers.FirstOrDefault(x => x.Name == gameDto.Developer)
                    ?? new Developer { Name = gameDto.Developer };

                var genre = context.Genres.FirstOrDefault(x => x.Name == gameDto.Genre)
                    ?? new Genre { Name = gameDto.Genre };

                var game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = gameDto.ReleaseDate.Value,
                    Developer = developer,
                    Genre = genre
                };

                foreach (var jsonTag in gameDto.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(x => x.Name == jsonTag)
                    ?? new Tag { Name = jsonTag };

                    game.GameTags.Add(new GameTag { Tag = tag });
                }

                context.Games.Add(game);
                context.SaveChanges();

                sb.AppendLine($"Added {gameDto.Name} ({gameDto.Genre}) with {gameDto.Tags.Count()} tags");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var jsonUsers = JsonConvert.DeserializeObject<IEnumerable<ImportUserDto>>(jsonString);

            foreach (var jsonUser in jsonUsers)
            {
                if (!IsValid(jsonUser) || !jsonUser.Cards.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User
                {
                    Age = jsonUser.Age,
                    Email = jsonUser.Email,
                    FullName = jsonUser.FullName,
                    Username = jsonUser.Username,
                    Cards = jsonUser.Cards.Select(x => new Card
                    {
                        Number = x.Number,
                        Cvc = x.CVC,
                        Type = x.Type.Value
                    }).ToList()
                };

                context.Users.Add(user);
                context.SaveChanges();

                sb.AppendLine($"Imported {jsonUser.Username} with {jsonUser.Cards.Count()} cards");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(ImportPurchaseDto[]), new XmlRootAttribute("Purchases"));
            var xmlPurchases = serializer.Deserialize(new StringReader(xmlString)) as ImportPurchaseDto[];

            foreach (var xmlPurchase in xmlPurchases)
            {
                var game = context.Games.FirstOrDefault(x => x.Name == xmlPurchase.GameName);
                var card = context.Cards.FirstOrDefault(x => x.Number == xmlPurchase.CardNumber);
                var parsedDate = DateTime.TryParseExact(xmlPurchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture , DateTimeStyles.None, out var date);
                if (!IsValid(xmlPurchase) || game == null || card == null || date == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase
                {
                    Type = xmlPurchase.Type.Value,
                    ProductKey = xmlPurchase.ProductKey,
                    Date = date,
                    Game = game,
                    Card = card
                };

                context.Purchases.Add(purchase);
                context.SaveChanges();

                sb.AppendLine($"Imported {xmlPurchase.GameName} for {purchase.Card.User.Username}");
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}