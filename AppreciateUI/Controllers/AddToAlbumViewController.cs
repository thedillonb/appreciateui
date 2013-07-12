using System;
using AppreciateUI.Models;
using AppreciateUI.Utils;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;
using System.Drawing;

namespace AppreciateUI.Controllers
{
    public class AddToAlbumViewController : DialogViewController
    {
        private readonly UIImage _img;
        private readonly string _category;
        private readonly bool _icon;
        private static readonly string SavePath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

        public Action Success;

        public AddToAlbumViewController(UIImage img, string category, bool icon)
            : base (UITableViewStyle.Plain, null, true)
        {
            _img = img;
            _category = category;
            _icon = icon;
            Title = "Add To Album";
        }

        public override void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);
            UIApplication.SharedApplication.SetStatusBarStyle(Util.iOSVersion.Item1 < 6 ? UIStatusBarStyle.BlackOpaque : UIStatusBarStyle.BlackTranslucent, false);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) 
				ContentSizeForViewInPopover = new System.Drawing.SizeF(320,480);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Add a new project
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => {
                PresentViewController(new UINavigationController(new NewAlbumViewController((r) => {
                    DismissViewController(true);
                    if (r)
                        LoadTable();
                })), true);
            });

            LoadTable();
        }

        private void LoadTable()
        {
            var section = new Section();
            var projects = Data.Database.Main.Table<Project>();
            foreach (var p in projects)
            {
                var project = p;
                var element = new StyledStringElement(project.Name, () => Save(project));
                section.Add(element);
            }

            var root = new RootElement(Title) { section };
            Root = root;
        }

        private void Save(Project project)
        {
            //Save the image to the project
            var path = Path.Combine(SavePath, Guid.NewGuid().ToString() + ".png");
            var thumbPath = Path.Combine(SavePath, Guid.NewGuid().ToString() + ".png");
            NSError error;
            _img.AsPNG().Save(path, true, out error);
            if (error != null && error.Code != 0)
            {
                //Unable to save image
                var alert = new UIAlertView { Title = "Error", Message = "Unable to save image. Error code: " + error.Code };
                alert.CancelButtonIndex = alert.AddButton("Ok");
                alert.Show();
                return;
            }


            var size = AppreciateUI.Utils.Util.ThumbnailSize;
            UIGraphics.BeginImageContextWithOptions(size, false, 0f);
            var context = UIGraphics.GetCurrentContext();
            context.TranslateCTM(0, size.Height);
            context.ScaleCTM(1f, -1f);
            context.DrawImage(new System.Drawing.RectangleF(0, 0, size.Width, size.Height), _img.CGImage);
            var cgImage = UIGraphics.GetImageFromCurrentImageContext();

            cgImage.AsPNG().Save(thumbPath, true, out error);
            if (error != null && error.Code != 0)
            {
                //Delete the first save..
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    AppreciateUI.Utils.Util.LogException("Unable to delete image file at: " + path, e);
                }

                var alert = new UIAlertView { Title = "Error", Message = "Unable to save image. Error code: " + error.Code };
                alert.CancelButtonIndex = alert.AddButton("Ok");
                alert.Show();
                return;
            }

            UIGraphics.EndImageContext();


            var pi = new ProjectImage { ProjectId = project.Id, Path = path, ThumbPath = thumbPath, Category = _category, Icon = _icon };
            Data.Database.Main.Insert(pi);

            if (Success != null)
                Success();
        }
    }
}

