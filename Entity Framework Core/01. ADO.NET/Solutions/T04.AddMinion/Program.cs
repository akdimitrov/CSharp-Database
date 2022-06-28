using System;
using Microsoft.Data.SqlClient;

namespace T04.AddMinion
{
    class Program
    {
        static void Main(string[] args)
        {
            using var connection = new SqlConnection
                ("Server=.;Database=MinionsDB;Integrated Security=true;Encrypt=false");
            connection.Open();

            string[] minionInfo = Console.ReadLine().Split();
            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string minionTown = minionInfo[3];
            string villainName = Console.ReadLine().Split()[1];

            SqlTransaction sqlTransaction = connection.BeginTransaction();
            try
            {
                int townId = GetTownId(sqlTransaction, connection, minionTown);
                int villainId = GetVillainId(sqlTransaction, connection, villainName);
                int minionId = AddMinionAndGetId(sqlTransaction, connection, minionName, minionAge, townId);

                var addMinionsVillainsStatement = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
                var addMinionsVillainsCmd = new SqlCommand(addMinionsVillainsStatement, connection, sqlTransaction);
                addMinionsVillainsCmd.Parameters.AddWithValue("@villainId", villainId);
                addMinionsVillainsCmd.Parameters.AddWithValue("@minionId", minionId);
                addMinionsVillainsCmd.ExecuteNonQuery();

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                Console.WriteLine(e.Message);
            }

            connection.Close();
        }

        private static int AddMinionAndGetId(SqlTransaction sqlTransaction, SqlConnection connection, string minionName, int minionAge, int townId)
        {
            var addMinionStatement = "INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";
            var addMinionCmd = new SqlCommand(addMinionStatement, connection, sqlTransaction);
            addMinionCmd.Parameters.AddWithValue(@"nam", minionName);
            addMinionCmd.Parameters.AddWithValue(@"age", minionAge);
            addMinionCmd.Parameters.AddWithValue(@"townId", townId);
            addMinionCmd.ExecuteNonQuery();

            var getMinionQuery = "SELECT Id FROM Minions WHERE Name = @Name";
            var getMinionCmd = new SqlCommand(getMinionQuery, connection, sqlTransaction);
            getMinionCmd.Parameters.AddWithValue("@Name", minionName);

            return (int)getMinionCmd.ExecuteScalar();
        }

        private static int GetVillainId(SqlTransaction sqlTransaction, SqlConnection connection, string villainName)
        {
            var getVillainQuery = "SELECT Id FROM Villains WHERE Name = @Name";
            var getVillainCmd = new SqlCommand(getVillainQuery, connection, sqlTransaction);
            getVillainCmd.Parameters.AddWithValue("@Name", villainName);

            var result = getVillainCmd.ExecuteScalar();
            if (result == null)
            {
                var addVillainStatement = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
                var addVillainCmd = new SqlCommand(addVillainStatement, connection, sqlTransaction);
                addVillainCmd.Parameters.AddWithValue("@villainName", villainName);
                addVillainCmd.ExecuteNonQuery();
                Console.WriteLine($"Villain {villainName} was added to the database.");
            }

            return (int)getVillainCmd.ExecuteScalar();
        }

        private static int GetTownId(SqlTransaction sqlTransaction, SqlConnection connection, string minionTown)
        {
            var getTownQuery = "SELECT Id FROM Towns WHERE Name = @townName";
            var getTownCmd = new SqlCommand(getTownQuery, connection, sqlTransaction);
            getTownCmd.Parameters.AddWithValue("@townName", minionTown);
            var result = getTownCmd.ExecuteScalar();
            if (result == null)
            {
                var addTownStatement = "INSERT INTO Towns (Name) VALUES (@townName)";
                var addTownCmd = new SqlCommand(addTownStatement, connection, sqlTransaction);
                addTownCmd.Parameters.AddWithValue("@townName", minionTown);
                addTownCmd.ExecuteNonQuery();
                Console.WriteLine($"Town {minionTown} was added to the database.");
            }

            return (int)getTownCmd.ExecuteScalar();
        }
    }
}
