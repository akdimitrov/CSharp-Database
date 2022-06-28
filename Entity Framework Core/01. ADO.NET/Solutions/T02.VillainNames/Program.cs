using System;
using System.Text;
using Microsoft.Data.SqlClient;

namespace T02.VillainNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
                ("Server=.;Database=MinionsDB;Integrated Security=true;Encrypt=false");
            connection.Open();
            string result = GetVillainNamesWithMinionsCount(connection);
            Console.WriteLine(result);
            connection.Close();
        }

        private static string GetVillainNamesWithMinionsCount(SqlConnection connection)
        {
            StringBuilder output = new StringBuilder();

            string villainNamesQuery = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                          FROM Villains AS v 
                                          JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                      GROUP BY v.Id, v.Name 
                                        HAVING COUNT(mv.VillainId) > 3 
                                      ORDER BY COUNT(mv.VillainId)";
            var villainNamesCmd = new SqlCommand(villainNamesQuery, connection);

            using SqlDataReader reader = villainNamesCmd.ExecuteReader();
            while (reader.Read())
            {
                output.AppendLine($"{reader["Name"]} - {reader["MinionsCount"]}");
            }

            reader.Close();
            return output.ToString().TrimEnd();
        }
    }
}
