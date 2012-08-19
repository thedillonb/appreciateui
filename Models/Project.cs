using System;
using SQLite;

namespace MobilePatterns.Models
{
    public class Project
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

