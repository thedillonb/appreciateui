using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace MobilePatterns.Controllers
{
    public class ProjectPatternsViewController : DialogViewController
    {
        public ProjectPatternsViewController()
            : base (UITableViewStyle.Plain, null)
        {
            Title = "Projects";
        }
    }
}

