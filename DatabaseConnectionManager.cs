using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    /// <summary>
    /// Provides management for the application's SQLite database connection.
    /// Handles initialization, connection retrieval, and database path access.
    /// </summary>
    public static class DatabaseConnectionManager
    {
        /// <summary>
        /// The name of the SQLite database file.
        /// </summary>
        private static readonly string Assessment2Db = "Assessment2Db.db";

        /// <summary>
        /// The full file path to the SQLite database.
        /// </summary>
        private static readonly string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assessment2Db);

        /// <summary>
        /// The connection string used to connect to the SQLite database.
        /// </summary>
        private static readonly string ConnectionString = $"Data Source={DbPath}";

        /// <summary>
        /// Lazy-initialized singleton instance of the SQLite connection.
        /// </summary>
        private static readonly Lazy<SqliteConnection> LazyConnection = new(() =>
        {
            try
            {
                DatabaseInitializer.EnsureDatabaseExists();
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
        /// Gets the singleton instance of the open SQLite connection.
        /// </summary>
        /// <returns>An open <see cref="SqliteConnection"/> to the application's database.</returns>
        public static SqliteConnection GetConnection()
        {
            return LazyConnection.Value;
        }

        /// <summary>
        /// Gets the full file path to the application's SQLite database.
        /// </summary>
        /// <returns>The database file path as a <see cref="string"/>.</returns>
        public static string GetDatabasePath() => DbPath;
    }
}
