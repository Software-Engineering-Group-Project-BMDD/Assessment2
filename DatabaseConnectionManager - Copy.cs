using System;
using System.IO;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    public static class SensorDataRepository
    {
        public static object DatabaseConnectionManager { get; private set; }

        public static List<SensorData> GetSensorData()
        {
            var connection = DatabaseConnectionManager.GetConnection();
            var sensorData = new List<SensorData>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Id, SensorType, Value, Timestamp FROM SensorData";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sensorData.Add(new SensorData
                        {
                            Id = reader.GetInt32(0),
                            SensorType = reader.GetString(1),
                            Value = reader.GetDouble(2),
                            Timestamp = reader.GetDateTime(3)
                        });
                    }
                }
            }

            return sensorData;
        }

        // Add other methods for CRUD operations
    }
}
