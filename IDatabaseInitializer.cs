using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    /// <summary>
    /// Defines a method for ensuring the application's database exists and is properly initialized.
    /// </summary>
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Ensures that the database file exists and is initialized with required tables and data.
        /// </summary>
        void EnsureDatabaseExists();
    }
}
