using System;

namespace MobilePatterns.Data
{
    public class PatternImages
    {
        public string Image { get; set; }

        public override string ToString()
        {
            return this.Image;
        }
    }

    public class WebMenu
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
    }
}

