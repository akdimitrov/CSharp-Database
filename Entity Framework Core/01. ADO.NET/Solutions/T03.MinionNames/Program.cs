using System;
using System.Text;
using Microsoft.Data.SqlClient;

namespace T03.MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
                ("Server=.;Database=MinionsDB;Integrated Security=true;Encrypt=false");
            connection.Open();
            int villainId = int.Parse(Console.ReadLine());
            string result = GetAllMinionNamesAndAges(connection, villainId);
            Console.WriteLine(result);
            connection.Close();
        }

        private static string GetAllMinionNamesAndAges(SqlConnection connection, int villainId)
        {
            string villainNameQuery = @"SELECT Name FROM Villains WHERE Id = @Id";
            var villainNameCmd = new SqlCommand(villainNameQuery, connection);
            villainNameCmd.Parameters.AddWithValue("@Id", villainId);
            string villainName = (string)villainNameCmd.ExecuteScalar();
            if (villainName == null)
            {
                return $"No villain with ID {villainId} exists in the database.";
            }

            StringBuilder output = new StringBuilder();
            output.AppendLine($"Villain: {villainName}");

            string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                              m.Name, 
                                              m.Age
                                         FROM MinionsVillains AS mv
                                         JOIN Minions As m ON mv.MinionId = m.Id
                                        WHERE mv.VillainId = @Id
                                     ORDER BY m.Name";
            var minionsCmd = new SqlCommand(minionsQuery, connection);
            minionsCmd.Parameters.AddWithValue("@Id", villainId);

            using SqlDataReader reader = minionsCmd.ExecuteReader();
            if (!reader.HasRows)
            {
                output.AppendLine($"(no minions)");
            }
            else
            {
                while (reader.Read())
                {
                    output.AppendLine($"{reader["RowNum"]}. {reader["Name"]} {reader["Age"]}");
                }
            }

            reader.Close();
            return output.ToString().TrimEnd();
        }
    }
}
