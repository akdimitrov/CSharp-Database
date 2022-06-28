using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace T07.PrintAllMinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
                ("Server=.;Database=MinionsDB;Integrated Security=true;Encrypt=false");
            connection.Open();
            List<string> names = GetAllMinionsNames(connection);
            connection.Close();

            for (int i = 0; i < names.Count / 2; i++)
            {

                Console.WriteLine(names[i]);
                Console.WriteLine(names[names.Count - i - 1]);
                if (i == names.Count / 2 - 1 && names.Count % 2 != 0)
                {
                    Console.WriteLine(names[i + 1]);
                }
            }
        }

        private static List<string> GetAllMinionsNames(SqlConnection connection)
        {
            List<string> names = new List<string>();
            var query = "SELECT Name FROM Minions";
            var cmd = new SqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                names.Add((string)reader[0]);
            }

            reader.Close();
            return names;
        }
    }
}
