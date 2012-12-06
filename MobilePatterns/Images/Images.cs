using System;
using MonoTouch.UIKit;

namespace MobilePatterns
{
    public static class Images
    {
        public static UIImage Picture = UIImage.FromBundle("Images/Picture");
        public static UIImage Tag = UIImage.FromBundle("Images/Bookmark");
        public static UIImage Polaroid = UIImage.FromBundle("Images/Magnifer");
        public static UIImage Background = UIImage.FromBundle("Images/background");
        public static UIImage CellBackground = UIImage.FromBundle("Images/tablecell");

        public static class Controls
        {
            public static UIImage Tabbar = UIImage.FromBundle("Images/Controls/tabbar");
            public static UIImage Navbar = UIImage.FromBundle("Images/Controls/navbar");
            public static UIImage BackButton = UIImage.FromBundle("Images/Controls/back_button");
            public static UIImage Button = UIImage.FromBundle("Images/Controls/button");
        }
    }
}

