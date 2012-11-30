using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace MobilePatterns.Controllers
{
    public class BaseDialogViewController : DialogViewController
    {
        public BaseDialogViewController()
            : base(UITableViewStyle.Grouped, null, true)
        {
            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => {
                NavigationController.PopViewControllerAnimated(true);
            });
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.TableView.BackgroundView = null;
            this.TableView.BackgroundColor = UIColor.Clear;
            ParentViewController.View.BackgroundColor = UIColor.FromPatternImage(Images.Background);
        }
    }
}

