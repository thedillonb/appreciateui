using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;

namespace MobilePatterns.Controllers
{
    public abstract class PatternViewController : UIViewController
    {
        private UITapGestureRecognizer _tapGesture;
        private MyDialog _dialog = new MyDialog();

        protected UITableView TableView { get { return _dialog.TableView; } }
        protected RootElement Root
        { 
            get { return _dialog.Root; }
            set { _dialog.Root = value; }
        }

        public PatternViewController()
        {
            Title = "Patterns";
            HidesBottomBarWhenPushed = true;

            _dialog.Dragging = () => {
                if (!NavigationController.NavigationBarHidden)
                {
                    NavigationController.SetNavigationBarHidden(true, true);
                    NavigationController.SetToolbarHidden(true, true);
                }
            };

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

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.TableView.Frame = new System.Drawing.RectangleF(-80, 80, 480, 320);
            this.View.AddSubview(this.TableView);
            this.TableView.Transform = CGAffineTransform.MakeRotation((float)Math.PI * -90f / 180f);
            this.TableView.ShowsHorizontalScrollIndicator = false;
            this.TableView.ShowsVerticalScrollIndicator = false;

            this.WantsFullScreenLayout = true;
            this.TableView.PagingEnabled = true;
            this.TableView.ContentSize = new System.Drawing.SizeF(320, 480);
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

        private class MyDialog : DialogViewController
        {
            public NSAction Dragging;
            public MyDialog()
                : base (UITableViewStyle.Plain, null)
            {
            }

            protected override void DraggingStarted()
            {
                base.DraggingStarted();
                Dragging();
            }
        }
    }
}

