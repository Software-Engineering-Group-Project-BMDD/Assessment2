using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public interface IDatabaseRepository
    {
        int AddUser(string firstName, string lastName, string address, int roleId);
        SqliteDataReader GetUsersWithRoles();
    }

}
