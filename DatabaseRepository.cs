using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    public static class DatabaseRepository
    {
        public static int AddUser(string firstName, string lastName, string address, int roleId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@f_name", firstName },
                { "@l_name", lastName },
                { "@address", address },
                { "@role", roleId }
            };

            ExecuteNonQuery("INSERT INTO Users (F_Name, L_Name, Address, Role) VALUES (@f_name, @l_name, @address, @role)", parameters);
            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        public static SqliteDataReader GetUsersWithRoles()
        {
            return ExecuteReader(@"SELECT u.User_id, u.F_Name, u.L_Name, u.Address, r.Role_name 
                                   FROM Users u 
                                   LEFT JOIN Role r ON u.Role = r.Role_Id");
        }

        private static int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters = null)
        {
            using var connection = DatabaseConnectionManager.GetConnection();
            using var command = new SqliteCommand(commandText, connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            return command.ExecuteNonQuery();
        }

        private static SqliteDataReader ExecuteReader(string commandText, Dictionary<string, object> parameters = null)
        {
            var connection = DatabaseConnectionManager.GetConnection();
            var command = new SqliteCommand(commandText, connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            return command.ExecuteReader();
        }

        private static object ExecuteScalar(string commandText, Dictionary<string, object> parameters = null)
        {
            using var connection = DatabaseConnectionManager.GetConnection();
            using var command = new SqliteCommand(commandText, connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            return command.ExecuteScalar();
        }
    }
}
