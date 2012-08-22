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
            Title = "Browse";
            TabBarItem.Image = Images.Polaroid;
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
                var data = PattrnData.GetMenus();
                var section = new Section();

                BeginInvokeOnMainThread(() => {
                    hud.Hide(true);
                    hud.RemoveFromSuperview();
                });

                foreach (var d in data)
                {
                    var menu = d;
                    var element = new StyledStringElement(d.Name, () => {
                        NavigationController.PushViewController(new WebViewPatternsViewController(menu.Uri)
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

