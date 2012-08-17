using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MobilePatterns.Controllers;

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



            /*
            var tabbar = new UITabBarController();
            tabbar.ViewControllerSelected += (sender, e) => {
                if (e.ViewController.ChildViewControllers[0] is AddPatternViewController)
                {
                    Utils.Camera.SelectPicture(e.ViewController, (x) => {
                        Console.WriteLine("Picture selected!");
                    }, () => {

                    });
                }
            };


            tabbar.AddChildViewController(new UINavigationController(new NewPatternsViewController()));
            tabbar.AddChildViewController(new UINavigationController(new BrowsePatternsViewController()));
            tabbar.AddChildViewController(new UINavigationController(new AddPatternViewController()));
            tabbar.AddChildViewController(new UINavigationController(new ProjectPatternsViewController()));
            */

            Window.RootViewController = new NewPatternsViewController();
            Window.MakeKeyAndVisible();
            return true;
        }
    }
}

