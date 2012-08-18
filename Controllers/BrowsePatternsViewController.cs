using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MobilePatterns.Data;

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
            NSTimer.CreateScheduledTimer(0, () => {
                var data = PattrnData.GetMenus();
                var section = new Section();

                foreach (var d in data)
                {
                    var menu = d;
                    var element = new StyledStringElement(d.Name, () => {
                        NavigationController.PushViewController(new ViewPatternsViewController(menu.Uri)
                                                                { Title = menu.Name }
                                                                , true);
                    });
                    section.Add(element);
                }

                var root = new RootElement(Title);
                root.Add(section);
                Root = root;
            });
        }
    }
}

