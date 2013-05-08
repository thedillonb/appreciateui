using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MobilePatterns.Models;
using MonoTouch.Foundation;
using System.IO;

namespace MobilePatterns.Controllers
{
    public class AddToAlbumViewController : DialogViewController
    {
        private UIImage _img;
        private string _category;
        private static string SavePath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

        public Action Success;

        public AddToAlbumViewController(UIImage img, string category)
            : base (UITableViewStyle.Plain, null, true)
        {
            _img = img;
            _category = category;
            Title = "Add";
        }

        public override void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);
            if (MobilePatterns.Utils.Util.iOSVersion.Item1 < 6)
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackOpaque, false);
            else
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackTranslucent, false);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Add a new project
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => {
                PresentModalViewController(new UINavigationController(new NewAlbumViewController((r) => {
                    DismissModalViewControllerAnimated(true);
                    if (r == true)
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
                var alert = new UIAlertView() { Title = "Error", Message = "Unable to save image. Error code: " + error.Code };
                alert.CancelButtonIndex = alert.AddButton("Ok");
                alert.Show();
                return;
            }


            var size = new System.Drawing.SizeF(148, 222);
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
                System.IO.File.Delete(path);
                var alert = new UIAlertView() { Title = "Error", Message = "Unable to save image. Error code: " + error.Code };
                alert.CancelButtonIndex = alert.AddButton("Ok");
                alert.Show();
                return;
            }

            UIGraphics.EndImageContext();


            var pi = new ProjectImage() { ProjectId = project.Id, Path = path, ThumbPath = thumbPath, Category = _category };
            Data.Database.Main.Insert(pi);

            if (Success != null)
                Success();

//            //Return to the previous controller
//            NavigationController.PopViewControllerAnimated(true);
        }
    }
}

