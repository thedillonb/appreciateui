using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Threading;
using MonoTouch.Foundation;
using MobilePatterns.Data;
using MonoTouch.Dialog.Utilities;

namespace MobilePatterns.Controllers
{
    public class NewPatternsViewController : DialogViewController
    {
        public NewPatternsViewController()
            : base (UITableViewStyle.Plain, null, false)
        {
            Title = "New Patterns";
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //Do the loading
            NSTimer.CreateScheduledTimer(0, () => {
                var data = PattrnData.GetData();
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


        private class MyImageElement : OwnerDrawnElement, IImageUpdated
        {
            private PattrnData.Model _model;
            private UIImage _img;

            public MyImageElement(PattrnData.Model model)
                : base(UITableViewCellStyle.Default, "myimage")
            {
                _model = model;
            }


            public override void Draw(System.Drawing.RectangleF bounds, MonoTouch.CoreGraphics.CGContext context, UIView view)
            {
                if (_img == null)
                    return;


                _img.Draw(bounds);
            }

            public override float Height(System.Drawing.RectangleF bounds)
            {
                return 200f;
            }

            public override UITableViewCell GetCell(UITableView tv)
            {
                var key = CellKey;
                var cell = tv.DequeueReusableCell(key);
                if (cell == null)
                {
                    cell = new UITableViewCell(Style, key);
                }

                //Request the image
                if (_img == null)
                {
                    _img = ImageLoader.DefaultRequestImage(new Uri(_model.Image), this);
                }

                return cell;
            }

            void IImageUpdated.UpdatedImage(Uri uri)
            {
                if (uri == null)
                    return;

                _img = ImageLoader.DefaultRequestImage(uri, this);
                var root = GetImmediateRootElement ();
                if (root == null || root.TableView == null)
                    return;
                root.TableView.ReloadRows (new NSIndexPath [] { IndexPath }, UITableViewRowAnimation.None);
            }
        }
    }
}

