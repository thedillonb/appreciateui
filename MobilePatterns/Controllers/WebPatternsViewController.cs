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
                view.FillWithLocal(Images[index].Path);
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
            foreach (var img in Images)
            {
                var photo = new PhotoBrowser.MWPhoto(UIImage.FromFile(img.Path));
                if (img.Category != null)
                    photo.Caption = img.Category;
                _photos.Add(photo);
            }
            _collectionView.ReloadData();
        }
    }

    public class WebPatternsViewController : PatternViewController, IImageUpdated
    {
        Category _source;
        List<Screenshot> _screenshots;


//        private UIBarButtonItem _cropButton, _addButton;
//        private CropView _crop;
//        private bool _isCropping;
//
//        private bool IsCropping
//        {
//            get { return _isCropping; }
//            set
//            {
//                if (_isCropping == value)
//                    return;
//
//                _isCropping = value;
//                if (_isCropping)
//                {
//                    _crop = new CropView();
//                    _crop.Frame = new RectangleF(100, 100, 100, 100);
//                    this.View.AddSubview(_crop);
//                }
//                else
//                {
//                    _crop.RemoveFromSuperview();
//                    _crop.Dispose();
//                    _crop = null;
//                }
//
//
//                _cropButton.Title = _isCropping ? "Exit Crop" : "Crop";
//                _addButton.Title = _isCropping ? "Add Crop" : "Add";
//
//                TableView.ScrollEnabled = !_isCropping;
//                NavigationController.SetNavigationBarHidden(true, true);
//                NavigationController.SetToolbarHidden(true, true);
//            }
//        }

        public WebPatternsViewController(Category source)
        {
            _source = source;
//            _cropButton = new UIBarButtonItem("Crop", UIBarButtonItemStyle.Bordered, CropImage);
//            _addButton = new UIBarButtonItem("Add", UIBarButtonItemStyle.Bordered, SaveImage);
        }

        public void UpdatedImage (Uri uri)
        {
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
                view.FillViewWithObject(_screenshots[index].Thumb);
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

            //Save the current item you're looking at!
//            ToolbarItems = new UIBarButtonItem[] {
//                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
//                _cropButton,
//                _addButton,
//                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
//            };

            var hud = new RedPlum.MBProgressHUD(View.Frame)
            { Mode = RedPlum.MBProgressHUDMode.Indeterminate };
            hud.TitleText = "Requesting Patterns...";
            hud.TitleFont = UIFont.BoldSystemFontOfSize(14f);
            this.View.AddSubview(hud);
            hud.Show(false);

            //Do the loading
            ThreadPool.QueueUserWorkItem(delegate {
                try
                {
                    _screenshots = Data.RequestFactory.GetScreenshots(_source.Id);

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
                    UIAlertView alert = new UIAlertView();
                    alert.Message = e.Message;
                    alert.Title = "Error";
                    alert.CancelButtonIndex = alert.AddButton("Ok");
                    alert.Show();
                }
            });
        }

//        private void CropImage(object sender, EventArgs args)
//        {
//            var cells = this.TableView.VisibleCells;
//            if (cells.Length == 0 || cells[0].ImageView.Image == null)
//                return;
//
//
//            IsCropping = !IsCropping;
//        }

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

