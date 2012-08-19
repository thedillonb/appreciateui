using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace MobilePatterns.Controllers
{
    public abstract class PatternViewController : DialogViewController
    {
        private UITapGestureRecognizer _tapGesture;

        public PatternViewController()
            : base (UITableViewStyle.Plain, null, true)
        {
            Title = "Patterns";
            HidesBottomBarWhenPushed = true;

            _tapGesture = new UITapGestureRecognizer();
            _tapGesture.NumberOfTapsRequired = 1;
            _tapGesture.AddTarget(() => {
                NavigationController.SetNavigationBarHidden(false, true);
                if (ToolbarItems != null && ToolbarItems.Length > 0)
                {
                    NavigationController.SetToolbarHidden(false, true);
                }
            });
        }

        protected override void DraggingStarted()
        {
            base.DraggingStarted();
            if (!NavigationController.NavigationBarHidden)
            {
                NavigationController.SetNavigationBarHidden(true, true);
                NavigationController.SetToolbarHidden(true, true);
            }
        }


        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.WantsFullScreenLayout = true;
            this.TableView.PagingEnabled = true;
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            this.View.AddGestureRecognizer(_tapGesture);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.Translucent = true;
            NavigationController.Toolbar.Translucent = true;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            NavigationController.SetNavigationBarHidden(true, true);
            NavigationController.SetToolbarHidden(true, true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NavigationController.NavigationBar.Translucent = false;
            NavigationController.Toolbar.Translucent = false;
            NavigationController.SetNavigationBarHidden(false, true);
            NavigationController.SetToolbarHidden(true, true);
        }
    }
}

