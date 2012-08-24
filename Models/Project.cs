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

        public int Remove()
        {
            var images = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == this.Id);
            foreach (var img in images)
            {
                img.Remove();
            }

            return Data.Database.Main.Delete(this);
        }
    }
}

