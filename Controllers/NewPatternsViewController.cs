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
    public class NewPatternsViewController : DialogViewController
    {
        public NewPatternsViewController()
            : base (UITableViewStyle.Plain, null, false)
        {
            Title = "New Patterns";
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.TableView.PagingEnabled = true;
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
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

            public MyImageElement(PattrnData.Model model)
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

