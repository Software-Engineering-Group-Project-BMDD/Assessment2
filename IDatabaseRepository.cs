using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    /// <summary>
    /// Defines methods for interacting with the application's user-related database operations.
    /// </summary>
    public interface IDatabaseRepository
    {
        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="address">The address of the user.</param>
        /// <param name="roleId">The role ID associated with the user.</param>
        /// <returns>The ID of the newly inserted user.</returns>
        int AddUser(string firstName, string lastName, string address, int roleId);

        /// <summary>
        /// Retrieves all users along with their associated roles from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="SqliteDataReader"/> containing user and role data.
        /// </returns>
        SqliteDataReader GetUsersWithRoles();
    }
}
