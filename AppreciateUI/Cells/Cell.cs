using System;
using CollectionViewBinding;
using MonoTouch.UIKit;
using System.Drawing;
using SDWebImage;
using MonoTouch.Foundation;

namespace AppreciateUI.Cells
{
    /// <summary>
    /// A cell.
    /// Because the PSCollectionView dequeues cells without a key it means we can really only have
    /// one type of cell and must apply properties to it during the dequeue :(
    /// </summary>
    public class Cell : PSCollectionViewCell
    {
        protected readonly UIImageView ImageView;
        UIActivityIndicatorView _activity;
        private CellType _type;

        public enum CellType
        {
            Screenshot,
            Icon
        }

        public Cell()
        {
            ImageView = new UIImageView(RectangleF.Empty);
            ImageView.ContentMode = UIViewContentMode.ScaleToFill;
            this.AddSubview(ImageView);
            BackgroundColor = UIColor.White;
        }

        protected void AddSpinner()
        {
            RemoveSpinner();

            _activity = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
            _activity.Frame = new RectangleF(0, 0, 48, 48);
            _activity.Center = this.Center;
            _activity.AutoresizingMask = UIViewAutoresizing.All;
            this.AddSubview(_activity);
            _activity.StartAnimating();
        }

        public void PrepareForUse(CellType type)
        {
            _type = type;
            if (type == CellType.Screenshot)
            {
//                _shadowView.Layer.ShadowColor = UIColor.FromRGB(41, 41, 41).CGColor;
//                _shadowView.Layer.ShadowOffset = new SizeF(0, 0);
//                _shadowView.Layer.ShadowOpacity = 0.6f;
//                _shadowView.Layer.ShadowRadius = 4f;
            }
            else if (type == CellType.Icon)
            {
//                _shadowView.Layer.ShadowColor = UIColor.FromRGB(41, 41, 41).CGColor;
//                _shadowView.Layer.ShadowOffset = new SizeF(0, 0);
//                _shadowView.Layer.ShadowOpacity = 0.3f;
                ImageView.Layer.MasksToBounds = true;
            }
        }

        public override void PrepareForReuse ()
        {
            base.PrepareForReuse ();
            SDWebImageManager.SharedManager.CancelForDelegate(this);
            if (ImageView.Image != null)
                ImageView.Image = null;

//            _shadowView.Layer.ShadowColor = UIColor.Clear.CGColor;
//            _shadowView.Layer.ShadowOffset = new SizeF(0, 0);
//            _shadowView.Layer.ShadowOpacity = 0.0f;
//            _shadowView.Layer.ShadowRadius = 0;
            ImageView.Layer.MasksToBounds = false;
            this.Layer.CornerRadius = ImageView.Layer.CornerRadius = 0;
        }

        
        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            ImageView.Frame = new RectangleF(0, 0, Bounds.Width, Bounds.Height);
            
            //if (_type == CellType.Screenshot)
                //this.Layer.ShadowPath = UIBezierPath.FromRect(this.Bounds).CGPath;

            if (_type == CellType.Icon)
                this.Layer.CornerRadius = ImageView.Layer.CornerRadius = this.Bounds.Width * (10f / 57f);
        }
        
        protected void RemoveSpinner()
        {
            if (_activity != null)
            {
                _activity.StopAnimating();
                _activity.RemoveFromSuperview();
                _activity = null;
            }
        }

        public void FillWithLocal(UIImage img)
        {
            RemoveSpinner();
            ImageView.Image = img;
            ImageView.Alpha = 0;
            UIView.BeginAnimations("imageFade");
            UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
            UIView.SetAnimationDuration(0.3);
            ImageView.Alpha = 1;
            UIView.CommitAnimations();
        }

        public void FillViewWithObject(string url)
        {
            AddSpinner();

            SDWebImageManager.SharedManager.CancelForDelegate(this);
            SDWebImageManager.SharedManager.Download(new NSUrl(url), this, 0, (i, c) => {
                FillWithLocal(i);
            }, (e) => { 
                Console.WriteLine("Unable to fill with local image: " + e.ToString());
            });
        }

        public override void FillViewWithObject (MonoTouch.Foundation.NSObject obj)
        {
        }

        public override float HeightForViewWithObject (NSObject obj, float columnWidth)
        {
            //Not used.
            throw new NotSupportedException();
        }
    }
}

