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
        public Project Project { get; set; }

        public LocalViewPatternsViewController()
        {
            var flex = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            ToolbarItems = new UIBarButtonItem[] {
                flex,
                new UIBarButtonItem("Delete", UIBarButtonItemStyle.Done, (s, e) => DeleteImage()),
                flex,
            };
        }

        private void DeleteImage()
        {
            var indexes = this.TableView.IndexPathsForVisibleRows;
            if (indexes.Length == 0)
                return;

            var element = Root[indexes[0].Section][indexes[0].Row];
            var item = element as LocalPatternImageElement;
            if (item == null)
                return;

            //Remove the image!
            item.ProjectImage.Remove();
            Root[indexes[0].Section].Remove(item);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var images = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == Project.Id);
            var section = new Section();
            foreach (var d in images)
            {
                var element = new LocalPatternImageElement(d);
                section.Add(element);
            }

            var root = new RootElement(Title);
            root.Add(section);
            Root = root;
        }
    }

    public class WebViewPatternsViewController : PatternViewController
    {
        private Uri _uri;
        private PatternSource _source;
        private UIBarButtonItem _cropButton, _addButton;
        private CropView _crop;
        private bool _isCropping;

        private bool IsCropping
        {
            get { return _isCropping; }
            set
            {
                if (_isCropping == value)
                    return;

                _isCropping = value;
                if (_isCropping)
                {
                    _crop = new CropView();
                    _crop.Frame = new RectangleF(100, 100, 100, 100);
                    this.View.AddSubview(_crop);
                }
                else
                {
                    _crop.RemoveFromSuperview();
                    _crop.Dispose();
                    _crop = null;
                }


                _cropButton.Title = _isCropping ? "Exit Crop" : "Crop";
                _addButton.Title = _isCropping ? "Add Crop" : "Add";

                TableView.ScrollEnabled = !_isCropping;
                NavigationController.SetNavigationBarHidden(true, true);
                NavigationController.SetToolbarHidden(true, true);
            }
        }

        public WebViewPatternsViewController(PatternSource source, Uri uri)
        {
            _uri = uri;
            _source = source;
            _cropButton = new UIBarButtonItem("Crop", UIBarButtonItemStyle.Bordered, CropImage);
            _addButton = new UIBarButtonItem("Add", UIBarButtonItemStyle.Bordered, SaveImage);
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            //Save the current item you're looking at!
            ToolbarItems = new UIBarButtonItem[] {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                _cropButton,
                _addButton,
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
            };

            var hud = new RedPlum.MBProgressHUD(View.Frame)
            { Mode = RedPlum.MBProgressHUDMode.Indeterminate };
            hud.TitleText = "Requesting Patterns...";
            hud.TitleFont = UIFont.BoldSystemFontOfSize(14f);
            this.View.AddSubview(hud);
            hud.Show(false);

            //Do the loading
            ThreadPool.QueueUserWorkItem(delegate {
                var data = _source.GetPatterns(_uri);

                BeginInvokeOnMainThread(() => {
                    hud.Hide(true);
                    hud.RemoveFromSuperview();
                });

                var section = new Section();
                foreach (var d in data)
                {
                    var element = new WebPatternImageElement(d);
                    section.Add(element);
                }

                var root = new RootElement(Title);
                root.Add(section);

                BeginInvokeOnMainThread(() => { Root = root; });
            });

            this.View.BackgroundColor = UIColor.Red;
        }

        private void CropImage(object sender, EventArgs args)
        {
            var cells = this.TableView.VisibleCells;
            if (cells.Length == 0 || cells[0].ImageView.Image == null)
                return;


            IsCropping = !IsCropping;
        }

        private void SaveImage(object sender, EventArgs args)
        {
            var cells = this.TableView.VisibleCells;
            if (cells.Length == 0 || cells[0].ImageView.Image == null)
                return;

            var img = cells[0].ImageView.Image;

            if (IsCropping)
            {
                var s = UIScreen.MainScreen.Scale;
                var f = new RectangleF(s * _crop.Frame.X, s * _crop.Frame.Y, s * _crop.Frame.Width, s * _crop.Frame.Height);

                UIGraphics.BeginImageContext(f.Size);
                var context = UIGraphics.GetCurrentContext();
                context.ScaleCTM(1.0f, -1.0f);
                context.TranslateCTM(0, -f.Size.Height);

                var cgImage = img.CGImage.WithImageInRect(f);
                context.DrawImage(new RectangleF(new PointF(0, 0), f.Size), cgImage);
                img = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
            }

            var saveCtrl = new AddToScrapbookViewController(img);
            saveCtrl.Success = () => { IsCropping = false; };
            NavigationController.PushViewController(saveCtrl, true);
        }
    }
}

