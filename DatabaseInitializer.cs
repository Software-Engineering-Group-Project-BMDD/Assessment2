using System;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    /// <summary>
    /// Provides methods to initialize the application's SQLite database, create tables, and seed default data.
    /// </summary>
    public class DatabaseInitializer : IDatabaseInitializer
    {
        /// <summary>
        /// Ensures that the database file exists, creates it if necessary, and initializes tables and default data.
        /// </summary>
        public void EnsureDatabaseExists()
        {
            var dbPath = DatabaseConnectionManager.GetDatabasePath();

            if (!File.Exists(dbPath))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
                    File.Create(dbPath).Close();

                    using var connection = DatabaseConnectionManager.GetConnection();
                    CreateTables(connection);
                    SeedDefaultData(connection);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create database file at {dbPath}", ex);
                }
            }
        }

        /// <summary>
        /// Creates the required tables in the database if they do not already exist.
        /// </summary>
        /// <param name="connection">The SQLite connection to use for table creation.</param>
        private static void CreateTables(SqliteConnection connection)
        {
            try
            {
                ExecuteNonQuery(connection, @"CREATE TABLE IF NOT EXISTS Role (
                    Role_Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Role_name TEXT NOT NULL
                )");

                ExecuteNonQuery(connection, @"CREATE TABLE IF NOT EXISTS Users (
                    User_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    F_Name TEXT NOT NULL,
                    L_Name TEXT NOT NULL,
                    Address TEXT,
                    Role INTEGER,
                    Password TEXT NOT NULL,
                    Incidence_id INTEGER,
                    FOREIGN KEY (Role) REFERENCES Role(Role_Id),
                    FOREIGN KEY (Incidence_id) REFERENCES Incidence(incidece_id)
                )");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create database tables", ex);
            }
        }

        /// <summary>
        /// Seeds the database with default roles and users.
        /// </summary>
        /// <param name="connection">The SQLite connection to use for seeding data.</param>
        private static void SeedDefaultData(SqliteConnection connection)
        {
            AddDefaultRoles(connection);
            AddDefaultUsers(connection);
        }

        /// <summary>
        /// Adds default roles to the Role table if they do not already exist.
        /// </summary>
        /// <param name="connection">The SQLite connection to use for inserting roles.</param>
        private static void AddDefaultRoles(SqliteConnection connection)
        {
            var roles = new[] { "Admin", "Environmental Scientist", "Operational Manager" };

            foreach (var role in roles)
            {
                ExecuteNonQuery(connection, "INSERT OR IGNORE INTO Role (Role_name) VALUES (@role_name)",
                    new Dictionary<string, object> { { "@role_name", role } });
            }
        }

        /// <summary>
        /// Adds default users to the Users table if they do not already exist.
        /// </summary>
        /// <param name="connection">The SQLite connection to use for inserting users.</param>
        private static void AddDefaultUsers(SqliteConnection connection)
        {
            var users = new[]
            {
                new { FirstName = "Admin", LastName = "User", Address = "Admin Address", RoleName = "Admin", Password = "Admin123" },
                new { FirstName = "Env", LastName = "Scientist", Address = "Env Address", RoleName = "Environmental Scientist", Password = "Env123" },
                new { FirstName = "Op", LastName = "Manager", Address = "Op Address", RoleName = "Operational Manager", Password = "Op123" }
            };

            foreach (var user in users)
            {
                var roleId = ExecuteScalar(connection, "SELECT Role_Id FROM Role WHERE Role_name = @role_name",
                    new Dictionary<string, object> { { "@role_name", user.RoleName } });

                if (roleId != null)
                {
                    ExecuteNonQuery(connection, "INSERT OR IGNORE INTO Users (F_Name, L_Name, Address, Role, Password) VALUES (@f_name, @l_name, @address, @role, @password)",
                        new Dictionary<string, object>
                        {
                            { "@f_name", user.FirstName },
                            { "@l_name", user.LastName },
                            { "@address", user.Address },
                            { "@role", roleId },
                            { "@password", HashPassword(user.Password) }
                        });
                }
            }
        }

        /// <summary>
        /// Executes a non-query SQL command (such as INSERT, UPDATE, or CREATE TABLE).
        /// </summary>
        /// <param name="connection">The SQLite connection to use.</param>
        /// <param name="commandText">The SQL command text.</param>
        /// <param name="parameters">Optional parameters for the command.</param>
        /// <returns>The number of rows affected.</returns>
        private static int ExecuteNonQuery(SqliteConnection connection, string commandText, Dictionary<string, object> parameters = null)
        {
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
        /// Executes a SQL command that returns a single value.
        /// </summary>
        /// <param name="connection">The SQLite connection to use.</param>
        /// <param name="commandText">The SQL command text.</param>
        /// <param name="parameters">Optional parameters for the command.</param>
        /// <returns>The value returned by the command, or null if none.</returns>
        private static object ExecuteScalar(SqliteConnection connection, string commandText, Dictionary<string, object> parameters = null)
        {
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

        /// <summary>
        /// Hashes a password using SHA256 and returns the Base64-encoded hash.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>The Base64-encoded SHA256 hash of the password.</returns>
        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
