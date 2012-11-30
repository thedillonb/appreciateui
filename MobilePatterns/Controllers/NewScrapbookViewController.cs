using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MobilePatterns.Models;
using MobilePatterns.Cells;

namespace MobilePatterns.Controllers
{
    public class NewScrapbookViewController : BaseDialogViewController
    {
        private Func<bool, Void> _action;

        public NewScrapbookViewController(Func<bool, Void> action)
        {
            _action = action;
            Title = "New Scrapbook";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (s, e) => {
                _action(false);
            });

            var root = new RootElement(Title) {
                new Section("Add A New Scrapbook to keep collections similar mobile patterns") {
                    new EntryElement("Name", "Scrapbook Name", "")
                },
                new Section() {
                    new StyledElement("Create Scrapbook", CreateProject)
                }
            };

            Root = root;
        }

        private void CreateProject()
        {
            var name = ((EntryElement)Root[0][0]).Value;
            Data.Database.Main.Insert(new Project() { Name = name });
            _action(true);
        }
    }
}

