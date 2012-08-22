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

namespace MobilePatterns.Controllers
{
    public class LocalViewPatternsViewController : PatternViewController
    {
        public Project Project { get; set; }

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

        public WebViewPatternsViewController(Uri uri)
        {
            _uri = uri;
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            //Save the current item you're looking at!
            ToolbarItems = new UIBarButtonItem[] {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem("Add", UIBarButtonItemStyle.Bordered, SaveImage),
            };

            var hud = new RedPlum.MBProgressHUD(View.Frame)
            { Mode = RedPlum.MBProgressHUDMode.Indeterminate };
            hud.TitleText = "Requesting Patterns...";
            hud.TitleFont = UIFont.BoldSystemFontOfSize(14f);
            this.View.AddSubview(hud);
            hud.Show(false);

            //Do the loading
            ThreadPool.QueueUserWorkItem(delegate {
                var data = PattrnData.GetPatterns(_uri);

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
        }

        private void SaveImage(object sender, EventArgs args)
        {
            //var action = new UIActionSheet();
            //action.CancelButtonIndex = action.AddButton("Cancel");
            //action.AddButton("

            var cells = this.TableView.VisibleCells;
            if (cells.Length == 0 || cells[0].ImageView.Image == null)
                return;

            var img = cells[0].ImageView.Image;

            NavigationController.PushViewController(new AddToProjectViewController(img), true);
        }
    }
}

