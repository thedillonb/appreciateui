using System;
using System.IO;

namespace MobilePatterns.Data
{
    public class Database : SQLite.SQLiteConnection
    {
        private Database()
            : base(Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), "database.db"))
        {

        }

        public static readonly Database Main = new Database();
    }
}

