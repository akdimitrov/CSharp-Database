using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace T05.ChangeTownNamesCasing
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
                ("Server=.;Database=MinionsDB;Integrated Security=true;Encrypt=false");
            connection.Open();

            string country = Console.ReadLine();
            string result = ChangeTownNamesCasing(connection, country);
            Console.WriteLine(result);
            connection.Close();
        }

        private static string ChangeTownNamesCasing(SqlConnection connection, string country)
        {
            var updateTownNamesQuery = @"UPDATE Towns
                                  SET Name = UPPER(Name)
                                WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";
            var updateTownNamesCmd = new SqlCommand(updateTownNamesQuery, connection);
            updateTownNamesCmd.Parameters.AddWithValue("@countryName", country);

            int rowsAffected = updateTownNamesCmd.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                return "No town names were affected.";
            }

            var getTownsQuery = @"SELECT t.Name 
                                    FROM Towns as t
                                    JOIN Countries AS c ON c.Id = t.CountryCode
                                   WHERE c.Name = @countryName";
            var getTownsCmd = new SqlCommand(getTownsQuery, connection);
            getTownsCmd.Parameters.AddWithValue("@countryName", country);

            using SqlDataReader reader = getTownsCmd.ExecuteReader();
            List<string> towns = new List<string>();
            while (reader.Read())
            {
                towns.Add((string)reader[0]);
            }

            reader.Close();
            return $"{rowsAffected} town names were affected." + Environment.NewLine + $"[{string.Join(", ", towns)}]";
        }
    }
}
