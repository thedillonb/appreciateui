using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MobilePatterns.Models;

namespace MobilePatterns.Controllers
{
    public class NewProjectViewController : DialogViewController
    {
        private Func<bool, Void> _action;

        public NewProjectViewController(Func<bool, Void> action)
            : base(UITableViewStyle.Grouped, null, true)
        {
            _action = action;
            Title = "New Project";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (s, e) => {
                _action(false);
            });

            var root = new RootElement(Title) {
                new Section("Add A New Project") {
                    new EntryElement("Name", "Project Name", "")
                },
                new Section() {
                    new StyledStringElement("Create Project", CreateProject)
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

