using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MobilePatterns.Data;
using System.Threading;

namespace MobilePatterns.Controllers
{
    public class BrowsePatternsViewController : DialogViewController
    {
        public BrowsePatternsViewController(bool pushing = false)
            : base(UITableViewStyle.Plain, null, pushing)
        {
            Title = "Patterns";
            TabBarItem.Image = Images.Polaroid;

            
            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => {
                NavigationController.PopViewControllerAnimated(true);
            });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var hud = new RedPlum.MBProgressHUD(View.Frame)
            { Mode = RedPlum.MBProgressHUDMode.Indeterminate };
            hud.TitleText = "Requesting Categories...";
            hud.TitleFont = UIFont.BoldSystemFontOfSize(14f);
            this.ParentViewController.View.AddSubview(hud);
            hud.Show(false);

            //Do the loading
            ThreadPool.QueueUserWorkItem(delegate {
                var patternSource = new MobilePatterns.Data.InspiredUI();
                var data = patternSource.GetMenus();
                var section = new Section();

                BeginInvokeOnMainThread(() => {
                    hud.Hide(true);
                    hud.RemoveFromSuperview();
                });

                foreach (var d in data)
                {
                    var menu = d;
                    var element = new StyledStringElement(d.Name, () => {
                        NavigationController.PushViewController(new WebViewPatternsViewController(patternSource, menu.Uri)
                                                                { Title = menu.Name }
                                                                , true);
                    }) { Accessory = UITableViewCellAccessory.DisclosureIndicator };
                    section.Add(element);
                }

                var root = new RootElement(Title);
                root.Add(section);

                BeginInvokeOnMainThread(() => { Root = root; });
            });
        }
    }
}

