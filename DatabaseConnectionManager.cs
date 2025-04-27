using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace MauiApp1
{
    public static class DatabaseConnectionManager
    {
        private static readonly string Assessment2Db = "Assessment2Db.db";
        private static readonly string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assessment2Db);
        private static readonly string ConnectionString = $"Data Source={DbPath}";

        public static bool isDatabaseAvailable = false;

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

        public static SqliteConnection GetConnection()
        {
            return LazyConnection.Value;
        }

        public static string GetDatabasePath() => DbPath;
    }
}
