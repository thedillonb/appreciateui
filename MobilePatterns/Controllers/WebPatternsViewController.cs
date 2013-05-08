using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Threading;
using MonoTouch.Foundation;
using MobilePatterns.Data;
using MonoTouch.Dialog.Utilities;
using System.Drawing;
using MobilePatterns.Cells;
using System.Collections.Generic;
using MobilePatterns.Models;
using MonoTouch.CoreGraphics;
using MobilePatterns.Views;

namespace MobilePatterns.Controllers
{
    public class LocalViewPatternsViewController : PatternViewController
    {
        List<PhotoBrowser.MWPhoto> _photos = new List<PhotoBrowser.MWPhoto>();
        UIImage[] _thumbs;
        public ProjectImage[] Images { get; set; }

        public LocalViewPatternsViewController(ProjectImage[] images)
        {
            Images = images;
        }

//        private void DeleteImage()
//        {
//            var indexes = this.TableView.IndexPathsForVisibleRows;
//            if (indexes.Length == 0)
//                return;
//
//            var element = Root[indexes[0].Section][indexes[0].Row];
//            var item = element as LocalPatternImageElement;
//            if (item == null)
//                return;
//
//            //Remove the image!
//            item.ProjectImage.Remove();
//            Root[indexes[0].Section].Remove(item);
//
//            //If you've deleted them all return to the other screen.
//            if (Root[indexes[0].Section].Count == 0)
//            {
//                NavigationController.PopViewControllerAnimated(true);
//            }
//        }

        protected override int OnGetItemsInCollection ()
        {
            return Images.Length;
        }

        protected override void OnAssignObject (PatternCell view, int index)
        {
            if (index < Images.Length)
                view.FillWithLocal(_thumbs[index]);
        }

        protected override PhotoBrowser.MWPhoto OnGetPhoto (int index)
        {
            if (index < Images.Length)
                return _photos[index];
            return null;
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            _thumbs = new UIImage[Images.Length];
            for (int i = 0; i < Images.Length; i++)
            {
                var img = Images[i];
                _thumbs[i] = UIImage.FromFile(img.ThumbPath);
                var photo = new PhotoBrowser.MWPhoto(UIImage.FromFile(img.Path));
                if (img.Category != null)
                    photo.Caption = img.Category;
                _photos.Add(photo);
            }
            _collectionView.ReloadData();
        }
    }

    public class WebPatternsViewController : PatternViewController
    {
        Category _source;
        List<Screenshot> _screenshots;

		public WebPatternsViewController()
			: this (null)
		{
		}

        public WebPatternsViewController(Category source)
        {
            _source = source;
        }
       
        protected override int OnGetItemsInCollection ()
        {
            if (_screenshots == null)
                return 0;
            return _screenshots.Count;
        }

        protected override void OnAssignObject (PatternCell view, int index)
        {
            if (index < _screenshots.Count)
                view.FillViewWithObject(_screenshots[index].Id.ToString());
        }

        protected override PhotoBrowser.MWPhoto OnGetPhoto (int index)
        {
            if (index < _screenshots.Count)
                return fuck[index];
            return null;
        }

        List<PhotoBrowser.MWPhoto> fuck;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            var hud = new RedPlum.MBProgressHUD(View.Frame)
            { Mode = RedPlum.MBProgressHUDMode.Indeterminate };
            hud.TitleText = "Loading...";
            hud.TitleFont = UIFont.BoldSystemFontOfSize(14f);
            this.View.AddSubview(hud);
            hud.Show(false);

            //Do the loading
            ThreadPool.QueueUserWorkItem(delegate {
                try
                {
					if (_source != null)
					{
                    	_screenshots = Data.RequestFactory.GetScreenshots(_source.Id);
					}
					else
					{
						_screenshots = Data.RequestFactory.GetRecentScreenshots();
					}

                    fuck = new List<PhotoBrowser.MWPhoto>();
                    _screenshots.ForEach(x => {
                        fuck.Add(new PhotoBrowser.MWPhoto(new NSUrl(x.Url)) { Caption = x.App });

                    });
  
                    BeginInvokeOnMainThread(() => { 
                        hud.Hide(true);
                        hud.RemoveFromSuperview();
                        _collectionView.ReloadData();
                    });
                }
                catch (Exception e)
                {
					BeginInvokeOnMainThread(() =>  {
						UIAlertView alert = new UIAlertView();
						alert.Message = e.Message;
						alert.Title = "Error";
						alert.CancelButtonIndex = alert.AddButton("Ok");
						alert.Show();
					});
                }
            });
        }

//        private void SaveImage(object sender, EventArgs args)
//        {
//            var cells = this.TableView.VisibleCells;
//            if (cells.Length == 0 || cells[0].ImageView.Image == null)
//                return;
//
//            var img = cells[0].ImageView.Image;
//
//            if (IsCropping)
//            {
//                var s = UIScreen.MainScreen.Scale;
//                var f = new RectangleF(s * _crop.Frame.X, s * _crop.Frame.Y, s * _crop.Frame.Width, s * _crop.Frame.Height);
//
//                UIGraphics.BeginImageContext(f.Size);
//                var context = UIGraphics.GetCurrentContext();
//                context.ScaleCTM(1.0f, -1.0f);
//                context.TranslateCTM(0, -f.Size.Height);
//
//                var cgImage = img.CGImage.WithImageInRect(f);
//                context.DrawImage(new RectangleF(new PointF(0, 0), f.Size), cgImage);
//                img = UIGraphics.GetImageFromCurrentImageContext();
//                UIGraphics.EndImageContext();
//            }
//
//            var saveCtrl = new AddToScrapbookViewController(img, Title);
//            saveCtrl.Success = () => { IsCropping = false; };
//            NavigationController.PushViewController(saveCtrl, true);
//        }
    }
}

