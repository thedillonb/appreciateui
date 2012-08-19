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
    }
}

