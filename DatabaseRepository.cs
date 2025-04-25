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


         /// <summary>
        /// Adds a new sensor to the database
        /// </summary>
        /// 
        /// 
       
        public static int AddSensor(string Sensor_Quantity, string Symbol, string unit, string Unit_Desc, string frequency, double SafeLevel, double longitude, double latitude, string sensorType)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@sensor_Quantity", Sensor_Quantity},
                { "@symbol", Symbol},
                { "@unit", unit},
                { "@unit_desc", Unit_Desc},
                { "@frequency", frequency},
                { "@safeLevel", SafeLevel},
                { "@longitude", longitude },
                { "@latitude", latitude },
                { "@sensor_type", sensorType }
            };

            ExecuteNonQuery(
                @"INSERT INTO Sensor (Sensor_Quantity, Symbol, Unit, Unit_Desc, Frequency, SafeLevel, Longitude, Latitude, sensor_type) 
                VALUES (@sensor_Quantity, @symbol, @unit, @unit_desc, @frequency, @safeLevel, @longitude, @latitude, @sensor_type)",
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
