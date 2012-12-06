using System;
using CollectionViewBinding;
using MonoTouch.UIKit;
using System.Drawing;
using MobilePatterns.Data;
using MonoTouch.Foundation;
using System.Threading;

namespace MobilePatterns.Cells
{
    public class PatternCell : PSCollectionViewCell, MonoTouch.Dialog.Utilities.IImageUpdated
    {
        UIImageView _imageView;
        UIActivityIndicatorView _activity;
        Uri _requestUri;

        UILabel _label;

        public PatternCell()
        {
            _imageView = new UIImageView(RectangleF.Empty);
            _imageView.ClipsToBounds = true;
            this.AddSubview(_imageView);

            _label = new UILabel();
            _label.TextColor = UIColor.FromRGB(41, 41, 41);
            _label.BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 0.9f);
            _label.TextAlignment = UITextAlignment.Left;
            _label.Font = UIFont.SystemFontOfSize(8f);
            _label.Text = "Evernote";
            this.AddSubview(_label);
            
            this.Layer.ShadowColor = UIColor.FromRGB(41, 41, 41).CGColor;
            this.Layer.ShadowOffset = new SizeF(0, 0);
            this.Layer.ShadowOpacity = 0.6f;
            this.Layer.ShadowRadius = 4f;

            BackgroundColor = UIColor.White;
        }
        
        public override void PrepareForReuse ()
        {
            base.PrepareForReuse ();
            if (_imageView.Image != null)
            {
                _imageView.Image.Dispose();
                _imageView.Image = null;
            }
        }
        
        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            this.Layer.ShadowPath = UIBezierPath.FromRect(this.Bounds).CGPath;
            
            _imageView.Frame = new RectangleF(1, 1, Bounds.Width - 2, Bounds.Height - 16);
            _label.Frame = new RectangleF(1, Bounds.Height - 14f, Bounds.Width - 2f, 13f);
        }
        
        public override float HeightForViewWithObject (NSObject obj, float columnWidth)
        {
            var width = Bounds.Width - 2f;
            var scale = 960f / (640f / width);
            return scale + 16;
        }
        
        public void FillViewWithObject(string sc)
        {
            AddSpinner();

            _requestUri = new Uri(sc);
            UpdatedImage(_requestUri);
        }

        private void AddSpinner()
        {
            RemoveSpinner();

            _activity = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
            _activity.Frame = new RectangleF(0, 0, 48, 48);
            _activity.Center = this.Center;
            _activity.AutoresizingMask = UIViewAutoresizing.All;
            this.AddSubview(_activity);
            _activity.StartAnimating();
        }

        public void FillWithLocal(string path)
        {
            _imageView.Image = UIImage.FromFile(path);
        }
        
        
        public override void FillViewWithObject (MonoTouch.Foundation.NSObject obj)
        {
        }

        private void RemoveSpinner()
        {
            if (_activity != null)
            {
                _activity.StopAnimating();
                _activity.RemoveFromSuperview();
                _activity = null;
            }
        }
        
        public void UpdatedImage (Uri uri)
        {
            if (uri == null)
                return;

            if (uri != _requestUri)
                return;

            var img = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
            if (img == null)
                return;

            RemoveSpinner();
            _imageView.Image = img;

            if (img.Size.Width == 0)
            {
                throw new Exception("Shit");
            }

            //Fade the image in
//            _imageView.Alpha = 0;
//            UIView.BeginAnimations("imageFade");
//            UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
//            UIView.SetAnimationDuration(0.3);
//            _imageView.Alpha = 1;
//            UIView.CommitAnimations();
        }
    }

}

