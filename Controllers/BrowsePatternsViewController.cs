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
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Do the loading
            ThreadPool.QueueUserWorkItem(delegate {
                var data = PattrnData.GetMenus();
                var section = new Section();

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

