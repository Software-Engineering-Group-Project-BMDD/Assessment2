using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace MauiApp1
{
    public class DatabaseConnectionChecker
    {
        private readonly string _dbPath;

        public DatabaseConnectionChecker(string dbPath)
        {
            _dbPath = dbPath;
        }

        /// <summary>
        /// Checks if the database file exists and attempts to open a connection.
        /// </summary>
        /// <returns>True if the connection is successful, otherwise false.</returns>
        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                // Check if the database file exists
                if (!File.Exists(_dbPath))
                {
                    Console.WriteLine("Database file does not exist.");
                    return false;
                }

                // Attempt to open a connection
                using var conn = new SqliteConnection($"Data Source={_dbPath};Version=3;");
                await conn.OpenAsync();
                Console.WriteLine("Database connection successful.");
                return true;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }
    }
}
