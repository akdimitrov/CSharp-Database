namespace Footballers.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Where(x => x.Footballers.Any())
                .ToArray()
                .Select(x => new ExportCoachXmlDto
                {
                    FottballersCount = x.Footballers.Count,
                    CoachName = x.Name,
                    Footballers = x.Footballers.Select(f => new ExportFootballerXmlDto
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(x => x.Name)
                    .ToArray()
                })
                .OrderByDescending(x => x.FottballersCount)
                .ThenBy(x => x.CoachName)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCoachXmlDto[]), new XmlRootAttribute("Coaches"));
            var nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);

            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            serializer.Serialize(writer, coaches, nameSpaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(x => x.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .OrderByDescending(x => x.TeamsFootballers.Count(f => f.Footballer.ContractStartDate >= date))
                .ThenBy(x => x.Name)
                .Take(5)
                .ToList()
                .Select(x => new
                {
                    Name = x.Name,
                    Footballers = x.TeamsFootballers.Where(f => f.Footballer.ContractStartDate >= date)
                        .OrderByDescending(x => x.Footballer.ContractEndDate)
                        .ThenBy(x => x.Footballer.Name)
                        .Select(x => new
                        {
                            FootballerName = x.Footballer.Name,
                            ContractStartDate = x.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                            ContractEndDate = x.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                            BestSkillType = x.Footballer.BestSkillType.ToString(),
                            PositionType = x.Footballer.PositionType.ToString()
                        })
                        .ToList()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(teams, Formatting.Indented);

            return json;
        }
    }
}
