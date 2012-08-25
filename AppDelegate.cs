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
                new UINavigationController(new ProjectPatternsViewController()),
            };

            UIViewController previousController = TabBarController.ViewControllers[0];
            TabBarController.ViewControllerSelected += (sender, e) => {
                if (e.ViewController is AddPatternViewController)
                {
                    TabBarController.SelectedViewController = previousController;
                    UIImagePickerController ctrl;
                    ctrl = Camera.SelectPicture(TabBarController, (dic) => { 
                        
                        ctrl.PushViewController(new AddToProjectViewController(null), true);


                    }, () => { 
                        ctrl.DismissModalViewControllerAnimated(true);
                    });

                    TabBarController.PresentModalViewController(ctrl, true);
                }
                else
                {
                    previousController = e.ViewController;
                }
            };

            Window.RootViewController = TabBarController;
            Window.MakeKeyAndVisible();
            return true;
        }
    }
}

