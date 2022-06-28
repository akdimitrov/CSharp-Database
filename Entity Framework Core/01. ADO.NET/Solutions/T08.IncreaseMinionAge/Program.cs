using System;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace T08.IncreaseMinionAge
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] minionsIds = Console.ReadLine().Split().Select(int.Parse).ToArray();

            using var connection = new SqlConnection
                ("Server=.;Database=MinionsDB;Integrated Security=true;Encrypt=false");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                foreach (var id in minionsIds)
                {
                    UpdateMinionAgeAndName(connection, transaction, id);
                }

                var selectCmd = new SqlCommand("SELECT * FROM Minions", connection, transaction);
                using var reader = selectCmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
                }

                reader.Close();
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e.Message);
            }

            connection.Close();
        }

        private static void UpdateMinionAgeAndName(SqlConnection connection, SqlTransaction transaction, int id)
        {
            var updateQuery = @"UPDATE Minions
                                   SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                 WHERE Id = @Id";
            var updateCmd = new SqlCommand(updateQuery, connection, transaction);
            updateCmd.Parameters.AddWithValue("@Id", id);

            updateCmd.ExecuteNonQuery();
        }
    }
}
