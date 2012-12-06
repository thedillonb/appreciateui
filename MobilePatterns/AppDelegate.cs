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
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackOpaque, false);

            System.IO.Directory.Delete(System.IO.Path.Combine(System.IO.Path.Combine(Environment.GetFolderPath (Environment.SpecialFolder.Personal), ".."), "Library/Caches/Pictures.MonoTouch.Dialog/"), true);
            
            //Set the theming
            UIBarButtonItem.Appearance.SetBackButtonBackgroundImage(Images.Controls.BackButton.CreateResizableImage(new UIEdgeInsets(0, 0, 0, -5)), UIControlState.Normal, UIBarMetrics.Default);
            UIBarButtonItem.Appearance.SetBackgroundImage(Images.Controls.Button, UIControlState.Normal, UIBarMetrics.Default);
            UINavigationBar.Appearance.SetBackgroundImage(Images.Controls.Navbar.CreateResizableImage(new UIEdgeInsets(0, 0, 0, 0)), UIBarMetrics.Default);
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
            TabBarController = new UITabBarController();

            TabBarController.ViewControllers = new UIViewController[] {
                new UINavigationController(new PatternCategoriesViewController()),
                new AddPatternViewController(),
                new UINavigationController(new ScrapbookPatternsViewController()),
            };

            _previousController = TabBarController.ViewControllers[0];
            TabBarController.ViewControllerSelected += ViewControllerSelected;

            Window.RootViewController = TabBarController;
            Window.MakeKeyAndVisible();
            return true;
        }

        private UIViewController _previousController;
        private void ViewControllerSelected(object sender, UITabBarSelectionEventArgs e)
        {
            if (e.ViewController is AddPatternViewController)
            {
                TabBarController.SelectedViewController = _previousController;
                UIImagePickerController ctrl = null;
                ctrl = Camera.SelectPicture(TabBarController, (dic) => { 

                    var original = dic[UIImagePickerController.OriginalImage] as UIImage;
                    if (original == null)
                        return;

                    var atsvc = new AddToScrapbookViewController(original, null);
                    atsvc.Success = () => {
                        ctrl.DismissModalViewControllerAnimated(true);
                    };

                    ctrl.PushViewController(atsvc, true);
                }, () => { 
                    ctrl.DismissModalViewControllerAnimated(true);
                });

                TabBarController.PresentModalViewController(ctrl, true);
            }
            else
            {
                _previousController = e.ViewController;
            }
        }
    }
}

