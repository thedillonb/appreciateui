using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MobilePatterns.Controllers;
using MonoTouch.Dialog;
using MobilePatterns.Utils;

namespace MobilePatterns
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
            if (MobilePatterns.Utils.Util.iOSVersion.Item1 < 6)
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackOpaque, false);
            else
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackTranslucent, false);

//
//            System.IO.Directory.Delete(System.IO.Path.Combine(System.IO.Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), ".."), "Library/Caches/Pictures.MonoTouch.Dialog/"), true);
//            
            //Set the theming
            UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(Images.Controls.BackButton.CreateResizableImage(new UIEdgeInsets(0, 16, 0, 10)), UIControlState.Normal, UIBarMetrics.Default);
            UIBarButtonItem.Appearance.SetBackgroundImage(Images.Controls.Button, UIControlState.Normal, UIBarMetrics.Default);
            UINavigationBar.Appearance.SetBackgroundImage(Images.Controls.Navbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIBarMetrics.Default);
            UIToolbar.Appearance.SetBackgroundImage(Images.Controls.Navbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIToolbarPosition.Any, UIBarMetrics.Default);
            UITabBar.Appearance.BackgroundImage = Images.Controls.Tabbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0));
            UIBarButtonItem.Appearance.TintColor = UIColor.FromRGBA(.16f, .16f, .16f, 0.9f);
            //UITabBar.Appearance.SetBackgroundImage(Images.Controls.Navbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIBarMetrics.Default);

//            UIBarButtonItem.Appearance.SetBackgroundImage(Images.BarButton.CreateResizableImage(new UIEdgeInsets(8, 7, 8, 7)), UIControlState.Normal, UIBarMetrics.Default);
//            UISegmentedControl.Appearance.SetBackgroundImage(Images.BarButton.CreateResizableImage(new UIEdgeInsets(8, 7, 8, 7)), UIControlState.Normal, UIBarMetrics.Default);
//            
//            UIBarButtonItem.Appearance.SetBackgroundImage(Images.BarButtonLandscape.CreateResizableImage(new UIEdgeInsets(8, 7, 8, 7)), UIControlState.Normal, UIBarMetrics.LandscapePhone);
//            UISegmentedControl.Appearance.SetBackgroundImage(Images.BarButtonLandscape.CreateResizableImage(new UIEdgeInsets(8, 7, 8, 7)), UIControlState.Normal, UIBarMetrics.LandscapePhone);
//            
//            //BackButton
//            UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(Images.BackButton.CreateResizableImage(new UIEdgeInsets(0, 14, 0, 5)), UIControlState.Normal, UIBarMetrics.Default);
//            
//            UISegmentedControl.Appearance.SetDividerImage(Images.Divider, UIControlState.Normal, UIControlState.Normal, UIBarMetrics.Default);
//            
//            UIToolbar.Appearance.SetBackgroundImage(Images.Bottombar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIToolbarPosition.Bottom, UIBarMetrics.Default);
//            //UIBarButtonItem.Appearance.TintColor = UIColor.White;
//            UISearchBar.Appearance.BackgroundImage = Images.Searchbar;
//            
//            var textAttrs = new UITextAttributes() { TextColor = UIColor.White, TextShadowColor = UIColor.DarkGray, TextShadowOffset = new UIOffset(0, -1) };
//            UINavigationBar.Appearance.SetTitleTextAttributes(textAttrs);
//            UISegmentedControl.Appearance.SetTitleTextAttributes(textAttrs, UIControlState.Normal);
//            
//            CodeFramework.UI.Views.SearchFilterBar.ButtonBackground = Images.BarButton.CreateResizableImage(new UIEdgeInsets(0, 6, 0, 6));
//            CodeFramework.UI.Views.SearchFilterBar.FilterImage = Images.Filter;
//            
//            DropbarView.Image = UIImage.FromBundle("/Images/Dropbar");
//            WatermarkView.Image = Images.Background;
//            HeaderView.Gradient = Images.CellGradient;
//            StyledElement.BgColor = UIColor.FromPatternImage(Images.TableCell);
//            ErrorView.AlertImage = UIImage.FromFile("Images/warning.png");
//            UserElement.Default = Images.Anonymous;
//            NewsFeedElement.DefaultImage = Images.Anonymous;
//            TableViewSectionView.BackgroundImage = Images.Searchbar;


            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            //SlideController = new SlideoutNavigationController();
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
                    killSplash = new UIImageView(UIImageHelper.FromFileAuto("Default-568h", "jpg"));
                else
                    killSplash = new UIImageView(UIImageHelper.FromFileAuto("Default", "jpg"));
                
                Window.AddSubview(killSplash);
                Window.BringSubviewToFront(killSplash);
                
                UIView.Animate(0.8, () => { 
                    killSplash.Alpha = 0.0f; 
                }, () => { 
                    killSplash.RemoveFromSuperview(); 
                });
            }


            return true;
        }

        private UIViewController _previousController;
		private void OpenAddPatternView()
		{
			TabBarController.SelectedViewController = _previousController;
			UIImagePickerController ctrl = null;
			ctrl = Camera.SelectPicture(TabBarController, (dic) => { 
				
				var original = dic[UIImagePickerController.OriginalImage] as UIImage;
				if (original == null)
					return;
				
				var atsvc = new AddToAlbumViewController(original, null);
				atsvc.Success = () => {
					ctrl.DismissModalViewControllerAnimated(true);
				};
				
				ctrl.PushViewController(atsvc, true);
			}, () => { 
				ctrl.DismissModalViewControllerAnimated(true);
			});
			
			TabBarController.PresentModalViewController(ctrl, true);
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
            if (retina)
                return UIImage.FromFile(filename + "@2x." + extension);
            else
                return UIImage.FromFile(filename + "." + extension);
        }
    }
}

