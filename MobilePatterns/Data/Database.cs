using System;
using System.IO;
using MobilePatterns.Models;

namespace MobilePatterns.Data
{
    public class Database : SQLite.SQLiteConnection
    {
        private Database()
            : base(Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), "database.db"))
        {
            CreateTable<Project>();
            CreateTable<ProjectImage>();

            if (Table<Project>().Count() == 0)
            {
                Insert(new Project() { Name = "Scrapbook" });
            }
        }

        public static readonly Database Main = new Database();
    }
}

