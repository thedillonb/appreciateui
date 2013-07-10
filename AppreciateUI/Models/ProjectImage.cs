using System;
using SQLite;

namespace AppreciateUI.Models
{
    public class ProjectImage
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string Path { get; set; }

        public string ThumbPath { get; set; }

        public string Category { get; set; }

        public bool Icon { get; set; }

        public int Remove()
        {
            try
            {
                System.IO.File.Delete(this.Path);
            }
            catch (Exception e)
            {
                AppreciateUI.Utils.Util.LogException("Unable to delete: " + this.Path, e);
            }

            try
            {
                System.IO.File.Delete(this.ThumbPath);
            }
            catch (Exception e)
            {
                AppreciateUI.Utils.Util.LogException("Unable to delete: " + this.ThumbPath, e);
            }

            return Data.Database.Main.Delete(this);
        }
    }
}

