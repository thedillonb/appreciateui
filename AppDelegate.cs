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
            UIApplication.SharedApplication.SetStatusBarHidden(true, true); 

            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            //SlideController = new SlideoutNavigationController();
            TabBarController = new UITabBarController();

            TabBarController.ViewControllers = new UIViewController[] {
                new UINavigationController(new BrowsePatternsViewController()),
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

