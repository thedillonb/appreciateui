using System;
using CollectionViewBinding;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using System.Threading;
using SDWebImage;

namespace AppreciateUI.Cells
{
    public class IconCell : PSCollectionViewCell
    {
        UIImageView _imageView;
		UIView _selectedView;
        UIActivityIndicatorView _activity;

        public IconCell()
        {
            _imageView = new UIImageView(RectangleF.Empty);
            _imageView.ContentMode = UIViewContentMode.ScaleToFill;
            _imageView.Layer.CornerRadius = 16f;
            _imageView.Layer.MasksToBounds = true;
            this.AddSubview(_imageView);

            this.Layer.ShadowColor = UIColor.FromRGB(41, 41, 41).CGColor;
            this.Layer.ShadowOffset = new SizeF(0, 0);
            this.Layer.ShadowOpacity = 0.6f;
            //this.Layer.ShadowRadius = 4f;
            this.Layer.CornerRadius = 16f;
            this.Layer.ShouldRasterize = true;
            this.Layer.RasterizationScale = UIScreen.MainScreen.Scale;

            BackgroundColor = UIColor.White;
        }

		public void SetSelected(bool selected)
		{
			if (selected)
			{
				if (_selectedView != null)
					return;

				_selectedView = new UIView(_imageView.Frame);
				_selectedView.UserInteractionEnabled = false;
				_selectedView.BackgroundColor = UIColor.FromRGBA(1f, 1f, 1f, 0.8f);
				this.AddSubview(_selectedView);
			}
			else
			{
				if (_selectedView != null)
				{
					_selectedView.RemoveFromSuperview();
					_selectedView = null;
				}
			}
		}

        public override void PrepareForReuse ()
        {
            base.PrepareForReuse ();
            SDWebImageManager.SharedManager.CancelForDelegate(this);
            if (_imageView.Image != null)
            {
                //_imageView.Image.Dispose();
                _imageView.Image = null;
            }
        }
        
        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            this.Layer.ShadowPath = UIBezierPath.FromRoundedRect(this.Bounds, 16f).CGPath;
            
            _imageView.Frame = new RectangleF(0, 0, Bounds.Width, Bounds.Height);
        }
        
        public override float HeightForViewWithObject (NSObject obj, float columnWidth)
        {
            return Bounds.Width;
        }
        
        public void FillViewWithObject(string id, string ext)
        {
            AddSpinner();

            SizeF size = AppreciateUI.Utils.Util.IconThumbnailSize;
            var url = "http://www.dillonbuchanan.com/appreciateui/icon_downloader.php?id=" + id + "&w=" + size.Width + "&h=" + size.Height + "&ext=" + ext;
            SDWebImageManager.SharedManager.CancelForDelegate(this);
            SDWebImageManager.SharedManager.Download(new NSUrl(url), this, 0, (i, c) => {
                FillWithLocal(i);
            }, (e) => { 
                Console.WriteLine("Unable to fill with local image: " + e.ToString());
            });
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

        public void FillWithLocal(UIImage img)
        {
            RemoveSpinner();
            _imageView.Image = img;
            _imageView.Alpha = 0;
            UIView.BeginAnimations("imageFade");
            UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
            UIView.SetAnimationDuration(0.3);
            _imageView.Alpha = 1;
            UIView.CommitAnimations();
        }


        UIImage currentLoad;
        public void FillWithLocalNotResized(UIImage img)
        {
            AddSpinner();

            currentLoad = img;
            var size = new SizeF(148, 222);

            ThreadPool.QueueUserWorkItem(delegate {
                var ciimage = MonoTouch.CoreImage.CIImage.FromCGImage (img.CGImage);
                var transform = MonoTouch.CoreGraphics.CGAffineTransform.MakeScale (size.Width / img.Size.Width * 2, size.Height / img.Size.Height * 2);
                var affineTransform = new MonoTouch.CoreImage.CIAffineTransform () { 
                    Image = ciimage,
                    Transform = transform
                };
                var output = affineTransform.OutputImage;
                var context = MonoTouch.CoreImage.CIContext.FromOptions (null);
                var a = context.CreateCGImage (output, output.Extent);

                _imageView.BeginInvokeOnMainThread(() => {
                    if (img == currentLoad)
                    {
                        FillWithLocal(UIImage.FromImage(a));
                    }
                });

            });
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
    }

}

