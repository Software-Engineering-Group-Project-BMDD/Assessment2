using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class DatabaseConnectionChecker : IDatabaseConnectionChecker
    {
        private readonly string _dbPath;

        public DatabaseConnectionChecker(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                if (!File.Exists(_dbPath))
                {
                    Console.WriteLine("Database file does not exist.");
                    return false;
                }

                using var conn = new SqliteConnection($"Data Source={_dbPath};Version=3;");
                await conn.OpenAsync();
                Console.WriteLine("Database connection successful.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }
    }
}
