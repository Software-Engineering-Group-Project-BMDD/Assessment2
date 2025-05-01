using System;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseExists()
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
                    CreateSensorTables(connection);
                    SeedDefaultData(connection);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create database file at {dbPath}", ex);
                }
            }

        }

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

                // Add other table creation queries here...
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create database tables", ex);
            }
        }
        
        private static void CreateSensorTables(SqliteConnection connection)
        {
            // will attempt to create the tables for sensor, and sensor reading
            try
            {
                // Create Sensor table
                ExecuteNonQuery(connection,
                    @"CREATE TABLE IF NOT EXISTS Sensor (
                        Sensor_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Sensor_Quantity TEXT,
                        Symbol TEXT,
                        Unit TEXT,
                        Unit_Desc TEXT,
                        Frequency TEXT,
                        SafeLevel REAL,
                        Longitude REAL,
                        Latitude REAL,
                        sensor_type TEXT
                    )");

                // Create Sensor_reading table
                ExecuteNonQuery(connection,
                    @"CREATE TABLE IF NOT EXISTS Sensor_reading (
                        reading_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        sensor_id INTEGER,
                        sensor_value REAL,
                        timestamp TEXT,
                        sensor_setpoint REAL,
                        FOREIGN KEY (sensor_id) REFERENCES Sensor(Sensor_id)
                    )");

                // must be done in this order!
                readSampleData.initializeFullAirQuality();
                readSampleData.initializeFullWaterQuality();
                readSampleData.initializeFullWeatherData();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create database tables", ex);
            }
        }

        private static void SeedDefaultData(SqliteConnection connection)
        {
            AddDefaultRoles(connection);
            AddDefaultUsers(connection);
        }

        private static void AddDefaultRoles(SqliteConnection connection)
        {
            var roles = new[] { "Admin", "Environmental Scientist", "Operational Manager" };

            foreach (var role in roles)
            {
                ExecuteNonQuery(connection, "INSERT OR IGNORE INTO Role (Role_name) VALUES (@role_name)",
                    new Dictionary<string, object> { { "@role_name", role } });
            }
        }

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

        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
