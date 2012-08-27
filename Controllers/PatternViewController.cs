using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;

namespace MobilePatterns.Controllers
{
    public abstract class PatternViewController : DialogViewController
    {
        private UITapGestureRecognizer _tapGesture;

        public PatternViewController()
            : base(UITableViewStyle.Plain, null)
        {
            Title = "Patterns";
            HidesBottomBarWhenPushed = true;

            _tapGesture = new UITapGestureRecognizer();
            _tapGesture.NumberOfTapsRequired = 1;
            _tapGesture.AddTarget(ShowToolbars);

            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => {
                NavigationController.PopViewControllerAnimated(true);
            });
        }

        private void ShowToolbars()
        {
            NavigationController.SetNavigationBarHidden(!NavigationController.NavigationBarHidden, true);
            if (ToolbarItems != null && ToolbarItems.Length > 0)
            {
                NavigationController.SetToolbarHidden(!NavigationController.ToolbarHidden, true);
            }
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.TableView.Frame = new System.Drawing.RectangleF(-79, 81, UIScreen.MainScreen.Bounds.Height, UIScreen.MainScreen.Bounds.Width);
            this.View.AddSubview(this.TableView);
            this.TableView.Transform = CGAffineTransform.MakeRotation((float)Math.PI * -90f / 180f);
            this.TableView.ShowsHorizontalScrollIndicator = false;
            this.TableView.ShowsVerticalScrollIndicator = false;

            this.WantsFullScreenLayout = true;
            this.TableView.PagingEnabled = true;
            this.TableView.ContentSize = new System.Drawing.SizeF(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
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

        protected override void DraggingStarted ()
        {
            base.DraggingStarted ();
            if (!NavigationController.NavigationBarHidden)
            {
                NavigationController.SetNavigationBarHidden(true, true);
                NavigationController.SetToolbarHidden(true, true);
            }
        }
    }
}

