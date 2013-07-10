using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AppreciateUI.Models
{
    public class Photo : PhotoBrowser.MWPhoto
    {
        public bool Icon { get; set; }

        public Photo(UIImage img)
            : base(img)
        {
        }

        public Photo(string url)
            : base(url)
        {
        }

        public Photo(NSUrl url)
            : base(url)
        {
        }
    }
}

