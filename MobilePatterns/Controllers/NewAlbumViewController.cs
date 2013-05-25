using System;
using AppreciateUI.Cells;
using AppreciateUI.Models;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace AppreciateUI.Controllers
{
    public class NewAlbumViewController : BaseDialogViewController
    {
        private readonly Func<bool, Void> _action;

        public NewAlbumViewController(Func<bool, Void> action)
        {
            _action = action;
            Title = "New Album";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (s, e) => _action(false));

            var root = new RootElement(Title) {
                new Section("Add a new album to group similar mobile patterns!") {
                    new EntryElement("Name", "Album Name", "") {
                        EntryFont = UIFont.SystemFontOfSize(14f),
                        TitleFont = StyledElement.DefaultTitleFont,
                        TitleColor = StyledElement.DefaultTitleColor,
                    }
                },
                new Section
                {
                    new StyledElement("Create Album", CreateProject)
                }
            };

            Root = root;
        }

        private void CreateProject()
        {
            var name = ((EntryElement)Root[0][0]).Value;
            if (string.IsNullOrEmpty(name))
            {
                var alert = new UIAlertView {Title = "Missing Name", Message = "You must enter a name!"};
                alert.CancelButtonIndex = alert.AddButton("Ok");
                alert.Show();
                return;
            }

            Data.Database.Main.Insert(new Project { Name = name });
            _action(true);
        }
    }
}

