namespace Footballers.DataProcessor
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
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var output = new StringBuilder();
            var serializer = new XmlSerializer(typeof(ImportCoachXmlDto[]), new XmlRootAttribute("Coaches"));
            var importCoaches = serializer.Deserialize(new StringReader(xmlString)) as ImportCoachXmlDto[];

            foreach (var importCoach in importCoaches)
            {
                if (!IsValid(importCoach))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                var coach = new Coach
                {
                    Name = importCoach.Name,
                    Nationality = importCoach.Nationality
                };

                foreach (var importFootballer in importCoach.Footballers)
                {
                    var isStartDateValid = DateTime.TryParseExact(importFootballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDate);
                    var isEndDateValid = DateTime.TryParseExact(importFootballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate);

                    if (!IsValid(importFootballer) || !isStartDateValid || !isEndDateValid || startDate > endDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    coach.Footballers.Add(new Footballer
                    {
                        Name = importFootballer.Name,
                        ContractStartDate = startDate,
                        ContractEndDate = endDate,
                        BestSkillType = (BestSkillType)importFootballer.BestSkillType,
                        PositionType = (PositionType)importFootballer.PositionType
                    });
                }

                context.Coaches.Add(coach);
                context.SaveChanges();

                output.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            return output.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var output = new StringBuilder();
            var importTeams = JsonConvert.DeserializeObject<IEnumerable<ImportTeamJsonDto>>(jsonString);

            foreach (var importTeam in importTeams)
            {
                if (!IsValid(importTeam))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                int trophies = int.Parse(importTeam.Trophies);
                if (trophies == 0)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                var team = new Team
                {
                    Name = importTeam.Name,
                    Nationality = importTeam.Nationality,
                    Trophies = trophies
                };

                foreach (var importFootballer in importTeam.Footballers.Distinct())
                {
                    var footballer = context.Footballers.FirstOrDefault(x => x.Id == importFootballer);
                    if (footballer == null)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    team.TeamsFootballers.Add(new TeamFootballer { Footballer = footballer });
                }

                context.Teams.Add(team);
                context.SaveChanges();

                output.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            return output.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
