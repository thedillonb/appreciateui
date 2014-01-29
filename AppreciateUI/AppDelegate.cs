using AppreciateUI.Controllers;
using AppreciateUI.Utils;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace AppreciateUI
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        public override UIWindow Window { get; set; }
        public SlideoutNavigationController SlideController { get; set; }

        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void WillTerminate(UIApplication application)
        {
            //Don't allow the application to use any more than 
            var size = SDWebImage.SDImageCache.SharedImageCache.GetSize();
            if (size >= Utils.Util.MaxCacheSize)
                SDWebImage.SDImageCache.SharedImageCache.ClearDisk();
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Set the status bar
            if (Util.iOSVersion.Item1 < 6)
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackOpaque, false);
            else
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackTranslucent, false);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				var textAttrs = new UITextAttributes() { TextColor = UIColor.White, TextShadowColor = UIColor.DarkGray, TextShadowOffset = new UIOffset(0, -1) };
				UINavigationBar.Appearance.SetTitleTextAttributes(textAttrs);
				UISegmentedControl.Appearance.SetTitleTextAttributes(textAttrs, UIControlState.Normal);
			}

            //Set the theming
            UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(Images.Controls.BackButton.CreateResizableImage(new UIEdgeInsets(0, 16, 0, 10)), UIControlState.Normal, UIBarMetrics.Default);
            UIBarButtonItem.AppearanceWhenContainedIn(typeof(UIPopoverController)).SetBackButtonBackgroundImage(null, UIControlState.Normal, UIBarMetrics.Default);

            UIBarButtonItem.Appearance.SetBackgroundImage(Images.Controls.Button, UIControlState.Normal, UIBarMetrics.Default);
            UIBarButtonItem.AppearanceWhenContainedIn(typeof(UIPopoverController)).SetBackgroundImage(null, UIControlState.Normal, UIBarMetrics.Default);

            UINavigationBar.Appearance.SetBackgroundImage(Images.Controls.Navbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIBarMetrics.Default);
			UINavigationBar.AppearanceWhenContainedIn(typeof(UIPopoverController)).SetBackgroundImage (null, UIBarMetrics.Default);

            UIToolbar.Appearance.SetBackgroundImage(Images.Controls.Navbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIToolbarPosition.Any, UIBarMetrics.Default);

            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            SlideController = new SlideoutNavigationController();
            Window.RootViewController = SlideController;
            SlideController.SelectView(new RecentPatternsViewController());
            Window.MakeKeyAndVisible();


            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                UIImageView killSplash = null;
                if (IsTall)
                    killSplash = new UIImageView(UIImageHelper.FromFileAuto("Default-568h"));
				else
					killSplash = new UIImageView(UIImageHelper.FromFileAuto("Default"));
                
                Window.AddSubview(killSplash);
                Window.BringSubviewToFront(killSplash);
				UIView.Animate(0.8, () => killSplash.Alpha = 0.0f, killSplash.RemoveFromSuperview);
            }

            return true;
        }

        public static bool IsTall
        {
            get 
            { 
                return UIDevice.CurrentDevice.UserInterfaceIdiom 
                    == UIUserInterfaceIdiom.Phone 
                        && UIScreen.MainScreen.Bounds.Height 
                        * UIScreen.MainScreen.Scale >= 1136;
            }     
        }
    }

    public static class UIImageHelper
    {
        public static UIImage FromFileAuto(string filename, string extension = "png")
        {
            var retina = (UIScreen.MainScreen.Scale > 1.0);
            return retina ? UIImage.FromFile(filename + "@2x." + extension) : UIImage.FromFile(filename + "." + extension);
        }
    }
}

