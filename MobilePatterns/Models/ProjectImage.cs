using System;
using SQLite;

namespace MobilePatterns.Models
{
    public class ProjectImage
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string Path { get; set; }

        public string Category { get; set; }

        public int Remove()
        {
            System.IO.File.Delete(this.Path);
            return Data.Database.Main.Delete(this);
        }
    }
}

