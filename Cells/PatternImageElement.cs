using System;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;
using MobilePatterns.Data;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using MobilePatterns.Models;
using MonoTouch.CoreGraphics;

namespace MobilePatterns.Cells
{
    public class LocalPatternImageElement : OwnerDrawnElement
    {
        private ProjectImage _projectImage;
        public LocalPatternImageElement(ProjectImage projectImage)
            : base(UITableViewCellStyle.Default, "myimage")
        {
            _projectImage = projectImage;
        }

        public override float Height(System.Drawing.RectangleF bounds)
        {
            return 320f;
        }

        public override void Draw(RectangleF bounds, MonoTouch.CoreGraphics.CGContext context, UIView view)
        {
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = base.GetCell(tv);
            cell.ImageView.Image = UIImage.FromFile(_projectImage.Path);
            cell.ImageView.Transform = CGAffineTransform.MakeRotation((float)Math.PI * 90f / 180f);
            return cell;
        }
    }

    public class WebPatternImageElement : OwnerDrawnElement, IImageUpdated
    {
        private PatternImages _model;
        private UIActivityIndicatorView _activity;

        public WebPatternImageElement(PatternImages model)
            : base(UITableViewCellStyle.Default, "myimage")
        {
            _model = model;
        }


        public override void Draw(System.Drawing.RectangleF bounds, MonoTouch.CoreGraphics.CGContext context, UIView view)
        {
        }

        public override float Height(System.Drawing.RectangleF bounds)
        {
            return 320f;
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = base.GetCell(tv);
            cell.ImageView.Transform = CGAffineTransform.MakeRotation((float)Math.PI * 90f / 180f);

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

