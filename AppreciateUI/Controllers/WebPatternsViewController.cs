using System;
using AppreciateUI.Cells;
using AppreciateUI.Data;
using MonoTouch.UIKit;
using System.Threading;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace AppreciateUI.Controllers
{
    public class WebPatternsViewController : PatternViewController
    {
        readonly Category _source;
        State _state = State.Waiting;
        List<Screenshot> _screenshots;

        /// <summary>
        /// A simple state machine enumeration
        /// </summary>
        enum State
        {
            Waiting, Loading, Loaded
        }

		public WebPatternsViewController()
			: this (null)
		{
		}

        public WebPatternsViewController(Category source)
        {
            _source = source;
        }

		protected override BrowserViewController CreateBrowserViewController()
		{
			return new WebBrowserViewController(_loadedImages);
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
                view.FillViewWithObject(_screenshots[index].Url, _screenshots[index].Ext);
        }

		List<PhotoBrowser.MWPhoto> _loadedImages = new List<PhotoBrowser.MWPhoto>();

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            LoadImages();
        }

        private void LoadImages()
        {
            if (_state != State.Waiting)
                return;

            _state = State.Loading;
            var hud = new RedPlum.MBProgressHUD(View.Frame)
            {
                Mode = RedPlum.MBProgressHUDMode.Indeterminate, 
                TitleText = "Loading...", 
                TitleFont = UIFont.BoldSystemFontOfSize(14f)
            };

            this.View.AddSubview(hud);
            hud.Show(false);

            //Do the loading
            ThreadPool.QueueUserWorkItem(delegate {
                try
                {
                    _screenshots = _source != null ? RequestFactory.GetScreenshots(_source.Id) : RequestFactory.GetRecentScreenshots();
                    _loadedImages = new List<PhotoBrowser.MWPhoto>();
                    _screenshots.ForEach(x => {
                        _loadedImages.Add(new PhotoBrowser.MWPhoto(new NSUrl(x.FullUrl)) { Caption = x.App });

                    });

                    BeginInvokeOnMainThread(() => { 
                        _state = State.Loaded;
                        hud.Hide(true);
                        hud.RemoveFromSuperview();
                        CollectionView.ReloadData();
                    });
                }
                catch (Exception e)
                {
                    BeginInvokeOnMainThread(() =>  {
                        _state = State.Waiting;
                        hud.Hide(true);
                        hud.RemoveFromSuperview();
                        var alert = new UIAlertView {Message = e.Message, Title = "Error"};
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

