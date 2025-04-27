using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    interface IDatabaseConnectionManager
    {
        /// <summary>
        /// Gets the database connection.
        /// </summary>
        /// <returns>The database connection.</returns>
        SqliteConnection GetConnection();
        /// <summary>
        /// Gets the path to the database.
        /// </summary>
        /// <returns>The path to the database.</returns>
        string GetDatabasePath();
    }
}
