using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    /// <summary>
    /// Provides methods for interacting with the database, including CRUD operations.
    /// </summary>
    public static class DatabaseRepository
    {
        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="address">The address of the user.</param>
        /// <param name="roleId">The role ID of the user.</param>
        /// <returns>The ID of the newly added user.</returns>
        public static int AddUser2Db(string firstName, string lastName, string address, int roleId)
        {
            var user_parameters = new Dictionary<string, object>
            {
                { "@f_name", firstName },
                { "@l_name", lastName },
                { "@address", address },
                { "@role", roleId }
            };

            ExecuteNonQuery("INSERT INTO Users (F_Name, L_Name, Address, Role) VALUES (@f_name, @l_name, @address, @role)", user_parameters);
            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        /// <summary>
        /// Retrieves all users along with their roles from the database.
        /// </summary>
        /// <returns>A <see cref="SqliteDataReader"/> containing the user and role data.</returns>
        public static SqliteDataReader GetUsersWithRoles()
        {
            return ExecuteReader(@"SELECT u.User_id, u.F_Name, u.L_Name, u.Address, r.Role_name 
                                   FROM Users u 
                                   LEFT JOIN Role r ON u.Role = r.Role_Id");
        }

        /// <summary>
        /// Executes a non-query SQL command (e.g., INSERT, UPDATE, DELETE).
        /// </summary>
        /// <param name="commandText">The SQL command to execute.</param>
        /// <param name="parameters">The parameters for the SQL command.</param>
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
        /// <param name="commandText">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the SQL query.</param>
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
        /// Executes a scalar SQL query and returns the first column of the first row in the result set.
        /// </summary>
        /// <param name="commandText">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the SQL query.</param>
        /// <returns>The result of the scalar query.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the scalar query returns null.</exception>
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

            var result = command.ExecuteScalar();
            return result ?? throw new InvalidOperationException("ExecuteScalar returned null.");
        }
    }
}
