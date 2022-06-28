using System;
using Microsoft.Data.SqlClient;

namespace T06.RemoveVillain
{
    class Program
    {
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using var connection = new SqlConnection
                ("Server=.;Database=MinionsDB;Integrated Security=true;Encrypt=false");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                string villainName = GetVillainName(connection, transaction, villainId);
                if (villainName == null)
                {
                    Console.WriteLine($"No such villain was found.");
                }
                else
                {
                    int count = DeleteFromMinionsVillains(connection, transaction, villainId);
                    DeleteVillain(connection, villainId, transaction);
                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{count} minions were released.");
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e.Message);
            }

            connection.Close();
        }

        private static void DeleteVillain(SqlConnection connection, int villainId, SqlTransaction transaction)
        {
            var deleteQuery = "DELETE FROM Villains WHERE Id = @villainId";
            var deleteCmd = new SqlCommand(deleteQuery, connection, transaction);
            deleteCmd.Parameters.AddWithValue("@villainId", villainId);
            deleteCmd.ExecuteNonQuery();
        }

        private static int DeleteFromMinionsVillains(SqlConnection connection, SqlTransaction transaction, int villainId)
        {
            var deleteQuery = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";
            var deleteCmd = new SqlCommand(deleteQuery, connection, transaction);
            deleteCmd.Parameters.AddWithValue("@villainId", villainId);

            return deleteCmd.ExecuteNonQuery();
        }

        private static string GetVillainName(SqlConnection connection, SqlTransaction transaction, int villainId)
        {
            var getVillainIdQuery = "SELECT Name FROM Villains WHERE Id = @villainId";
            var getVillainIdCmd = new SqlCommand(getVillainIdQuery, connection, transaction);
            getVillainIdCmd.Parameters.AddWithValue("@villainId", villainId);
            string name = (string)getVillainIdCmd.ExecuteScalar();

            return name;
        }
    }
}
