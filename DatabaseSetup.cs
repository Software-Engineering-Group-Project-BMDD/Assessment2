using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Reflection;

namespace MauiApp1
{
    public class DatabaseSetup
    {
        private static readonly string Assessment2Db = "Assessment2Db.db";
        private static readonly string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assessment2Db);
        private static readonly string ConnectionString = $"Data Source={DbPath}";

        // Singleton instance with thread safety using Lazy<T>
        private static readonly Lazy<SqliteConnection> LazyConnection = new(() =>
        {
            try
            {
                EnsureDatabaseExists();
                var connection = new SqliteConnection(ConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize database connection", ex);
            }
        });

        /// <summary>
        /// Gets a connection to the database
        /// </summary>
        public static SqliteConnection GetConnection()
        {
            return LazyConnection.Value;
        }

        /// <summary>
        /// Ensures the database file exists, creates it if necessary
        /// </summary>
        public static void EnsureDatabaseExists()
        {
            if (!File.Exists(DbPath))
            {
                try
                {
                    var assembly = typeof(DatabaseSetup).Assembly;
                    using var resourceStream = assembly.GetManifestResourceStream($"MauiApp1.Resources.Raw.{Assessment2Db}");

                    if (resourceStream != null)
                    {
                        // Copy existing database from resources if available
                        Directory.CreateDirectory(Path.GetDirectoryName(DbPath));
                        using var fileStream = File.Create(DbPath);
                        resourceStream.CopyTo(fileStream);
                    }
                    else
                    {
                        // Create new empty database
                        Directory.CreateDirectory(Path.GetDirectoryName(DbPath));
                        File.Create(DbPath).Close();

                        using var connection = new SqliteConnection(ConnectionString);
                        connection.Open();
                        CreateTables(connection);

                        // Add default roles and users
                        AddDefaultRoles(connection);
                        AddDefaultUsers(connection);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to create database file at {DbPath}", ex);
                }
            }
        }

        /// <summary>
        /// Creates the necessary tables in the database based on the ER diagram
        /// </summary>
        private static void CreateTables(SqliteConnection connection)
        {
            try
            {
                // Create Role table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS Role (
                        Role_Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Role_name TEXT NOT NULL
                    )", null, connection);

                // Create Users table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS Users (
                        User_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        F_Name TEXT NOT NULL,
                        L_Name TEXT NOT NULL,
                        Address TEXT,
                        Role INTEGER,
                        Password TEXT NOT NULL,
                        Incidence_id INTEGER,
                        FOREIGN KEY (Role) REFERENCES Role(Role_Id),
                        FOREIGN KEY (Incidence_id) REFERENCES Incidence(incidece_id)
                    )", null, connection);

                // Create Incidence table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS Incidence (
                        incidece_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        incidence_Type TEXT NOT NULL,
                        Date_of_event TEXT NOT NULL,
                        Alert BOOLEAN
                    )", null, connection);

                // Create Configuration table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS Configuration_ (
                        config_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        read_interval INTEGER,
                        reading_format TEXT,
                        min_threshold REAL,
                        max_threshold REAL
                    )", null, connection);

                // Create Sensor table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS Sensor (
                        Sensor_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Longitude REAL,
                        Latitude REAL,
                        sensor_type TEXT
                    )", null, connection);

                // Create Sensor_reading table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS Sensor_reading (
                        reading_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        sensor_id INTEGER,
                        sensor_value REAL,
                        timestamp TEXT,
                        sensor_setpoint REAL,
                        FOREIGN KEY (sensor_id) REFERENCES Sensor(Sensor_id)
                    )", null, connection);

                // Create Maintenance table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS maintenace (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        maintenance_type TEXT,
                        Date TEXT,
                        call_log TEXT,
                        sensor_id INTEGER,
                        FOREIGN KEY (sensor_id) REFERENCES Sensor(Sensor_id)
                    )", null, connection);

                // Create Update table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS Update (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        update_Type TEXT,
                        Date_of_last_update TEXT,
                        sensor_id INTEGER,
                        FOREIGN KEY (sensor_id) REFERENCES Sensor(Sensor_id)
                    )", null, connection);

                // Create Measurand table
                ExecuteNonQueryWithConnection(
                    @"CREATE TABLE IF NOT EXISTS Measurand (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Quantity_type TEXT,
                        Column2 TEXT,
                        Column3 TEXT,
                        Column4 TEXT,
                        sensor_id INTEGER,
                        FOREIGN KEY (sensor_id) REFERENCES Sensor(Sensor_id)
                    )", null, connection);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create database tables", ex);
            }
        }

        /// <summary>
        /// Helper method to execute non-query with an existing connection
        /// </summary>
        private static int ExecuteNonQueryWithConnection(string commandText, Dictionary<string, object> parameters, SqliteConnection connection)
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
        /// Executes a non-query SQL command
        /// </summary>
        public static int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters = null)
        {
            try
            {
                using var connection = GetConnection();
                return ExecuteNonQueryWithConnection(commandText, parameters, connection);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing SQL command: {commandText}", ex);
            }
        }

        /// <summary>
        /// Executes a query and returns a DataReader
        /// </summary>
        public static SqliteDataReader ExecuteReader(string commandText, Dictionary<string, object> parameters = null)
        {
            try
            {
                var connection = GetConnection();
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
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing reader: {commandText}", ex);
            }
        }

        /// <summary>
        /// Executes a scalar query and returns the first column of the first row
        /// </summary>
        public static object ExecuteScalar(string commandText, Dictionary<string, object> parameters = null)
        {
            try
            {
                using var connection = GetConnection();
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
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing scalar: {commandText}", ex);
            }
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        public static int AddUser(string firstName, string lastName, string address, int roleId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@f_name", firstName },
                { "@l_name", lastName },
                { "@address", address },
                { "@role", roleId }
            };

            ExecuteNonQuery(
                "INSERT INTO Users (F_Name, L_Name, Address, Role) VALUES (@f_name, @l_name, @address, @role)",
                parameters);

            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        /// <summary>
        /// Adds a new role to the database
        /// </summary>
        public static int AddRole(string roleName)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@role_name", roleName }
            };

            ExecuteNonQuery(
                "INSERT INTO Role (Role_name) VALUES (@role_name)",
                parameters);

            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        /// <summary>
        /// Adds a new incidence to the database
        /// </summary>
        public static int AddIncidence(string incidenceType, DateTime dateOfEvent, bool alert)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@incidence_type", incidenceType },
                { "@date_of_event", dateOfEvent.ToString("yyyy-MM-dd HH:mm:ss") },
                { "@alert", alert }
            };

            ExecuteNonQuery(
                "INSERT INTO Incidence (incidence_Type, Date_of_event, Alert) VALUES (@incidence_type, @date_of_event, @alert)",
                parameters);

            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        /// <summary>
        /// Adds a new sensor to the database
        /// </summary>
        public static int AddSensor(double longitude, double latitude, string sensorType)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@longitude", longitude },
                { "@latitude", latitude },
                { "@sensor_type", sensorType }
            };

            ExecuteNonQuery(
                "INSERT INTO Sensor (Longitude, Latitude, sensor_type) VALUES (@longitude, @latitude, @sensor_type)",
                parameters);

            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        /// <summary>
        /// Adds a new sensor reading to the database
        /// </summary>
        public static int AddSensorReading(int sensorId, double sensorValue, DateTime timestamp, double setpoint)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@sensor_id", sensorId },
                { "@sensor_value", sensorValue },
                { "@timestamp", timestamp.ToString("yyyy-MM-dd HH:mm:ss") },
                { "@sensor_setpoint", setpoint }
            };

            ExecuteNonQuery(
                "INSERT INTO Sensor_reading (sensor_id, sensor_value, timestamp, sensor_setpoint) " +
                "VALUES (@sensor_id, @sensor_value, @timestamp, @sensor_setpoint)",
                parameters);

            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        /// <summary>
        /// Adds a new maintenance record to the database
        /// </summary>
        public static int AddMaintenance(string maintenanceType, DateTime date, string callLog, int sensorId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@maintenance_type", maintenanceType },
                { "@date", date.ToString("yyyy-MM-dd HH:mm:ss") },
                { "@call_log", callLog },
                { "@sensor_id", sensorId }
            };

            ExecuteNonQuery(
                "INSERT INTO maintenace (maintenance_type, Date, call_log, sensor_id) " +
                "VALUES (@maintenance_type, @date, @call_log, @sensor_id)",
                parameters);

            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        /// <summary>
        /// Adds a new configuration to the database
        /// </summary>
        public static int AddConfiguration(int readInterval, string readingFormat, double minThreshold, double maxThreshold)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@read_interval", readInterval },
                { "@reading_format", readingFormat },
                { "@min_threshold", minThreshold },
                { "@max_threshold", maxThreshold }
            };

            ExecuteNonQuery(
                "INSERT INTO Configuration_ (read_interval, reading_format, min_threshold, max_threshold) " +
                "VALUES (@read_interval, @reading_format, @min_threshold, @max_threshold)",
                parameters);

            return Convert.ToInt32(ExecuteScalar("SELECT last_insert_rowid()"));
        }

        /// <summary>
        /// Associates a sensor with a configuration
        /// </summary>
        public static void AssociateSensorWithConfig(int sensorId, int configId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@sensor_id", sensorId },
                { "@config_id", configId }
            };

            ExecuteNonQuery(
                "UPDATE Sensor SET config_id = @config_id WHERE Sensor_id = @sensor_id",
                parameters);
        }

        /// <summary>
        /// Gets all users with their roles
        /// </summary>
        public static SqliteDataReader GetUsersWithRoles()
        {
            return ExecuteReader(
                @"SELECT u.User_id, u.F_Name, u.L_Name, u.Address, r.Role_name 
                FROM Users u 
                LEFT JOIN Role r ON u.Role = r.Role_Id");
        }

        /// <summary>
        /// Gets all incidents for a specific user
        /// </summary>
        public static SqliteDataReader GetUserIncidents(int userId)
        {
            return ExecuteReader(
                @"SELECT i.incidece_id, i.incidence_Type, i.Date_of_event, i.Alert 
                FROM Incidence i 
                JOIN Users u ON u.Incidence_id = i.incidece_id 
                WHERE u.User_id = @user_id",
                new Dictionary<string, object> { { "@user_id", userId } });
        }

        /// <summary>
        /// Gets all readings for a specific sensor
        /// </summary>
        public static SqliteDataReader GetSensorReadings(int sensorId)
        {
            return ExecuteReader(
                @"SELECT reading_id, sensor_value, timestamp, sensor_setpoint 
                FROM Sensor_reading 
                WHERE sensor_id = @sensor_id 
                ORDER BY timestamp DESC",
                new Dictionary<string, object> { { "@sensor_id", sensorId } });
        }

        /// <summary>
        /// Hashes a password using a secure cryptographic algorithm
        /// </summary>
        private static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Adds default roles to the database if they do not already exist
        /// </summary>
        private static void AddDefaultRoles(SqliteConnection connection)
        {
            var roles = new[] { "Admin", "Environmental Scientist", "Operational Manager" };

            foreach (var role in roles)
            {
                var parameters = new Dictionary<string, object> { { "@role_name", role } };
                ExecuteNonQueryWithConnection(
                    "INSERT OR IGNORE INTO Role (Role_name) VALUES (@role_name)",
                    parameters,
                    connection);
            }
        }

        /// <summary>
        /// Adds default user accounts to the database
        /// </summary>
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
                var roleId = ExecuteScalar(
                    "SELECT Role_Id FROM Role WHERE Role_name = @role_name",
                    new Dictionary<string, object> { { "@role_name", user.RoleName } });

                if (roleId != null)
                {
                    var parameters = new Dictionary<string, object>
                    {
                        { "@f_name", user.FirstName },
                        { "@l_name", user.LastName },
                        { "@address", user.Address },
                        { "@role", roleId },
                        { "@password", HashPassword(user.Password) }
                    };

                    ExecuteNonQueryWithConnection(
                        "INSERT OR IGNORE INTO Users (F_Name, L_Name, Address, Role, Password) VALUES (@f_name, @l_name, @address, @role, @password)",
                        parameters,
                        connection);
                }
            }
        }
          

        
}
