using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    /// <summary>
    /// Provides methods for interacting with the application's database, including user management and queries.
    /// </summary>
    public class DatabaseRepository : IDatabaseRepository
    {
        /// <summary>
        /// Adds a new user to the Users table.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="address">The address of the user.</param>
        /// <param name="roleId">The role ID associated with the user.</param>
        /// <returns>The ID of the newly inserted user.</returns>
        public int AddUser(string firstName, string lastName, string address, int roleId)
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

        /// <summary>
        /// Retrieves all users along with their associated roles.
        /// </summary>
        /// <returns>A <see cref="SqliteDataReader"/> containing user and role data.</returns>
        public SqliteDataReader GetUsersWithRoles()
        {
            return ExecuteReader(@"SELECT u.User_id, u.F_Name, u.L_Name, u.Address, r.Role_name 
                                   FROM Users u 
                                   LEFT JOIN Role r ON u.Role = r.Role_Id");
        }

        /// <summary>
        /// Executes a non-query SQL command (such as INSERT, UPDATE, or DELETE).
        /// </summary>
        /// <param name="commandText">The SQL command text.</param>
        /// <param name="parameters">Optional parameters for the command.</param>
        /// <returns>The number of rows affected.</returns>
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

        /// <summary>
        /// Executes a SQL query and returns a <see cref="SqliteDataReader"/> for reading the results.
        /// </summary>
        /// <param name="commandText">The SQL command text.</param>
        /// <param name="parameters">Optional parameters for the command.</param>
        /// <returns>A <see cref="SqliteDataReader"/> containing the query results.</returns>
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

        /// <summary>
        /// Executes a SQL command that returns a single value.
        /// </summary>
        /// <param name="commandText">The SQL command text.</param>
        /// <param name="parameters">Optional parameters for the command.</param>
        /// <returns>The value returned by the command, or null if none.</returns>
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
