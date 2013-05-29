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
        public UITabBarController TabBarController { get; set; }

        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
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
            UITabBar.Appearance.BackgroundImage = Images.Controls.Tabbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0));


            Window = new UIWindow(UIScreen.MainScreen.Bounds);
			TabBarController = new UIRaisedTabBar(Images.Controls.CenterButton, Images.Controls.CenterButton, OpenAddPatternView);

            TabBarController.ViewControllers = new UIViewController[] {
				new UINavigationController(new RecentPatternsViewController()),
                new UINavigationController(new PatternCategoriesViewController()),
                new AddPatternViewController(),
                new UINavigationController(new AlbumsViewController()),
				new UINavigationController(new SettingsViewController())
            };

            _previousController = TabBarController.ViewControllers[0];
            TabBarController.ViewControllerSelected += ViewControllerSelected;

            Window.RootViewController = TabBarController;
            Window.MakeKeyAndVisible();


            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                UIImageView killSplash = null;
                if (IsTall)
                    killSplash = new UIImageView(UIImageHelper.FromFileAuto("Default-568h", "png"));
                else
                    killSplash = new UIImageView(UIImageHelper.FromFileAuto("Default", "jpg"));
                
                Window.AddSubview(killSplash);
                Window.BringSubviewToFront(killSplash);
                
                UIView.Animate(0.8, () => { 
                    killSplash.Alpha = 0.0f; 
                }, () => killSplash.RemoveFromSuperview());
            }


            return true;
        }

        private UIViewController _previousController;
		private UIPopoverController _pop;
		private void OpenAddPatternView()
		{
			TabBarController.SelectedViewController = _previousController;
			UIImagePickerController ctrl = null;
			ctrl = Camera.SelectPicture(TabBarController, (dic) => { 
				
				var original = dic[UIImagePickerController.OriginalImage] as UIImage;
				if (original == null)
					return;
				
				var atsvc = new AddToAlbumViewController(original, null);
				atsvc.Success = () => DismissPopupOfModal(ctrl);
				ctrl.PushViewController(atsvc, true);
			}, () => { 
				DismissPopupOfModal(ctrl);
			});

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				_pop = new UIPopoverController (ctrl);
				_pop.PopoverContentSize = new System.Drawing.SizeF(320, 480);
				_pop.PresentFromRect(TabBarController.TabBar.Frame, TabBarController.View, UIPopoverArrowDirection.Down, false);
			} 
			else {
				TabBarController.PresentModalViewController(ctrl, true);
			}

		}

		private void DismissPopupOfModal(UIImagePickerController ctrl)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				if (_pop != null)
				{
					_pop.Dismiss(true);
					_pop.Dispose();
					_pop = null;
				}
			}
			else
			{
				ctrl.DismissModalViewControllerAnimated(true);
			}
		}


        private void ViewControllerSelected(object sender, UITabBarSelectionEventArgs e)
        {
			//Remember what was selected last
            if (e.ViewController is AddPatternViewController)
				OpenAddPatternView();
			else
                _previousController = e.ViewController;
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

