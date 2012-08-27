using System;
using MonoTouch.UIKit;
using MobilePatterns.Views;
using System.Drawing;

namespace MobilePatterns.Controllers
{
    public class CropViewController : UIViewController
    {
        private UIImageView _imgView;

        public UIImage SourceImage { get; set; }

        public CropViewController(UIImage sourceImage)
        {
            SourceImage = sourceImage;
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            _imgView = new UIImageView(SourceImage);
            _imgView.Frame = new System.Drawing.RectangleF(0, 0, View.Bounds.Width, View.Bounds.Height);
            _imgView.Center = new System.Drawing.PointF(View.Bounds.Width / 2f, View.Bounds.Height / 2f);
            View.AddSubview(_imgView);


        }
    }
}

