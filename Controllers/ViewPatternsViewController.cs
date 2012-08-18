using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Threading;
using MonoTouch.Foundation;
using MobilePatterns.Data;
using MonoTouch.Dialog.Utilities;
using System.Drawing;

namespace MobilePatterns.Controllers
{
    public class ViewPatternsViewController : DialogViewController
    {
        private Uri _uri;
        private UITapGestureRecognizer _tapGesture;


        public ViewPatternsViewController(Uri uri)
            : base (UITableViewStyle.Plain, null, false)
        {
            _uri = uri;
            Title = "Patterns";
            HidesBottomBarWhenPushed = true;

            _tapGesture = new UITapGestureRecognizer();
            _tapGesture.NumberOfTapsRequired = 1;
            _tapGesture.AddTarget(() => {
                NavigationController.SetNavigationBarHidden(false, true);
            });
        }

        protected override void DraggingStarted()
        {
            base.DraggingStarted();
            if (!NavigationController.NavigationBarHidden)
                    NavigationController.SetNavigationBarHidden(true, true);
        }


        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.WantsFullScreenLayout = true;
            this.TableView.PagingEnabled = true;
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            this.View.AddGestureRecognizer(_tapGesture);

            //Do the loading
            NSTimer.CreateScheduledTimer(0, () => {
                var data = PattrnData.GetPatterns(_uri);
                var section = new Section();

                foreach (var d in data)
                {
                    var element = new MyImageElement(d);
                    section.Add(element);
                }

                var root = new RootElement(Title);
                root.Add(section);
                Root = root;
            });
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.Translucent = true;

        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            NavigationController.SetNavigationBarHidden(true, true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NavigationController.NavigationBar.Translucent = false;
            NavigationController.SetNavigationBarHidden(false, true);
        }

        private class MyImageElement : OwnerDrawnElement, IImageUpdated
        {
            private PatternImages _model;
            private UIActivityIndicatorView _activity;

            public MyImageElement(PatternImages model)
                : base(UITableViewCellStyle.Default, "myimage")
            {
                _model = model;
            }


            public override void Draw(System.Drawing.RectangleF bounds, MonoTouch.CoreGraphics.CGContext context, UIView view)
            {
            }

            public override float Height(System.Drawing.RectangleF bounds)
            {
                return 480f;
            }

            public override UITableViewCell GetCell(UITableView tv)
            {
                var cell = base.GetCell(tv);

                //Request the image
                cell.ImageView.Image = ImageLoader.DefaultRequestImage(new Uri(_model.Image), this);
                if (cell.ImageView.Image == null)
                {
                    if (_activity == null) 
                    {
                        _activity = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
                        _activity.Frame = new RectangleF(0, 0, 48, 48);
                        _activity.Center = cell.ContentView.Center;
                        cell.ContentView.AutosizesSubviews = true;
                        _activity.AutoresizingMask = UIViewAutoresizing.All;
                        cell.ContentView.AddSubview(_activity);
                        _activity.StartAnimating();
                    }
                } 
                else
                {
                    if (_activity != null)
                    {
                        _activity.StopAnimating();
                        _activity.RemoveFromSuperview();
                        _activity = null;
                    }

                    //Fade the image in
                    cell.ImageView.Alpha = 0;
                    UIView.BeginAnimations("imageFade");
                    UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
                    UIView.SetAnimationDuration(0.3);
                    cell.ImageView.Alpha = 1;
                    UIView.CommitAnimations();
                }

                return cell;
            }

            void IImageUpdated.UpdatedImage(Uri uri)
            {
                if (uri == null)
                    return;
                var root = GetImmediateRootElement ();
                if (root == null || root.TableView == null)
                    return;
                root.TableView.ReloadRows (new NSIndexPath [] { IndexPath }, UITableViewRowAnimation.None);
            }
        }
    }
}

