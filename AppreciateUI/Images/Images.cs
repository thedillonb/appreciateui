using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreImage;
using System.Drawing;

namespace AppreciateUI
{
    public static class Images
    {
        public static UIImage Picture = UIImage.FromBundle("Images/Picture");
        public static UIImage Tag = UIImage.FromBundle("Images/Bookmark");
		public static UIImage Gear = UIImage.FromBundle("Images/gear");
		public static UIImage Tray = UIImage.FromBundle("Images/tray_full");
        public static UIImage Polaroid = UIImage.FromBundle("Images/Magnifer");
        public static UIImage Background = UIImage.FromBundle("Images/background");
        public static UIImage CellBackground = UIImage.FromBundle("Images/tablecell");

        public static class Controls
        {
            public static UIImage Navbar = UIImage.FromBundle("Images/Controls/navbar");
            public static UIImage BackButton = UIImage.FromBundle("Images/Controls/back_button");
            public static UIImage Button = UIImage.FromBundle("Images/Controls/button");
        }

        public static class Buttons
        {
            public static UIImage ThreeLines = UIImage.FromBundle("Images/Buttons/three_lines");
        }

        public static class Components
        {
            public static UIImage MenuSectionBackground = UIImage.FromBundle("Images/Components/menu_section_bg");
            public static UIImage Title = UIImage.FromBundle("Images/Components/title");
        }

        public static class Menu
        {
            public static UIImage Feedback = UIImage.FromBundle("Images/Menu/feedback");
            public static UIImage Info = UIImage.FromBundle("Images/Menu/info");
            public static UIImage Album = UIImage.FromBundle("Images/Menu/album");
            public static UIImage Icons = UIImage.FromBundle("Images/Menu/icons");
            public static UIImage Recent = UIImage.FromBundle("Images/Menu/recent");
            public static UIImage UIPatterns = UIImage.FromBundle("Images/Menu/uipatterns");
            public static UIImage Plus = UIImage.FromBundle("Images/Menu/plus");
            public static UIImage AllAlbums = UIImage.FromBundle("Images/Menu/allalbums");
        }
    }
}

