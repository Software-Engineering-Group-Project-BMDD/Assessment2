using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MauiApp1
{
    /// <summary>
    /// Provides functionality to check the connection to a SQLite database file.
    /// </summary>
    public class DatabaseConnectionChecker : IDatabaseConnectionChecker
    {
        private readonly string _dbPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionChecker"/> class with the specified database path.
        /// </summary>
        /// <param name="dbPath">The file path to the SQLite database.</param>
        public DatabaseConnectionChecker(string dbPath)
        {
            _dbPath = dbPath;
        }

        /// <summary>
        /// Asynchronously checks if the database file exists and can be opened.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <c>true</c> if the connection is successful; otherwise, <c>false</c>.
        /// </returns>
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
