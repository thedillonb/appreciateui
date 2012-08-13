using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace MobilePatterns.Controllers
{
    public class BrowsePatternsViewController : DialogViewController
    {
        public BrowsePatternsViewController(bool pushing = false)
            : base(UITableViewStyle.Plain, null, pushing)
        {
            Title = "Browse";
        }
    }
}

