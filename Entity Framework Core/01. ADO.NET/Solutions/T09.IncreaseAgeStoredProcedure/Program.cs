using System;
using Microsoft.Data.SqlClient;

namespace T09.IncreaseAgeStoredProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            int minionId = int.Parse(Console.ReadLine());

            using var connection = new SqlConnection
                ("Server=.;Database=MinionsDB;Integrated Security=true;Encrypt=false");
            connection.Open();

            var query = "EXEC usp_GetOlder @Id";
            var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", minionId);
            int count = cmd.ExecuteNonQuery();
            if (count != 0)
            {
                var selectQuery = "SELECT Name, Age FROM Minions WHERE Id = @Id";
                var selectCmd = new SqlCommand(selectQuery, connection);
                selectCmd.Parameters.AddWithValue("@Id", minionId);
                using var reader = selectCmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
                }
                reader.Close();
            }

            connection.Close();
        }
    }
}
