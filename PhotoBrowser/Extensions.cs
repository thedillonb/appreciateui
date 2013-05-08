using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace SDWebImage
{
    public static class Extensions
    {
        public static void SetImage (this UIImageView view, NSUrl url)
        {
            SDWebImageManager.SharedManager.SetImage (view, url);
        }
        
        public static void SetImage (this UIButton button, NSUrl url)
        {
            SDWebImageManager.SharedManager.SetImage (button, url);
        }
        
        public static void SetBackgroundImage (this UIButton button, NSUrl url)
        {
            SDWebImageManager.SharedManager.SetBackgroundImage (button, url);
        }
    }
    
    // A little help for Interface Builder
    public partial class SDWebImageManagerDelegate : UIView
    {
        
    }
}