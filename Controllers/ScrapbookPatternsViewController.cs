using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MobilePatterns.Models;

namespace MobilePatterns.Controllers
{
    public class ScrapbookPatternsViewController : DialogViewController
    {
        public ScrapbookPatternsViewController()
            : base (UITableViewStyle.Plain, null)
        {
            Title = "Scrapbook";
            TabBarItem.Image = Images.Tag;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => {
                PresentModalViewController(new UINavigationController(new NewScrapbookViewController((r) => {
                    DismissModalViewControllerAnimated(true);
                })), true);
            });
        }

        public override void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);
            LoadTable();
        }

        private void LoadTable()
        {
            var section = new Section();
            var projects = Data.Database.Main.Table<Project>();
            foreach (var p in projects)
            {
                var project = p;
                var element = new ProjectElement(project);
                element.Tapped += () => {
                    NavigationController.PushViewController(new LocalViewPatternsViewController() 
                                                            { Project = project, Title = project.Name }, true);
                };
                section.Add(element);
            }

            var root = new RootElement(Title) { section };
            Root = root;
        }

        public override Source CreateSizingSource(bool unevenRows)
        {
            return new EditingSource(this);
        }

        private class ProjectElement : StyledStringElement
        {
            public Project Project;
            public ProjectElement(Project p)
                : base(p.Name, "", UITableViewCellStyle.Value1)
            {
                Project = p;
                var count = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == p.Id).Count();
                this.Value = count.ToString();
                this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }
        }


        private class EditingSource : DialogViewController.Source 
        {
            public EditingSource(DialogViewController dvc)
                : base(dvc)
            {
            }

            public override bool CanEditRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                return true;
            }

            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                return UITableViewCellEditingStyle.Delete;
            }

            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                var section = Container.Root [indexPath.Section];
                var element = section [indexPath.Row];

                var item = element as ProjectElement;
                if (item == null)
                    return;

                //How to delete it
                Action deleteDelegate = () => {
                    item.Project.Remove();
                    section.Remove(element);
                };

                var count = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == item.Project.Id).Count();
                if (count > 0)
                {
                    var alert = new UIAlertView();
                    alert.Title = "Confirm?";
                    alert.Message = "Are you sure you want to delete this project and all " + count + " images?";
                    alert.CancelButtonIndex = alert.AddButton("No");
                    var ok = alert.AddButton("Yes");

                    alert.Clicked += (sender, e) => {
                        if (e.ButtonIndex == ok)
                            deleteDelegate();
                    };

                    alert.Show();
                }
                else
                {
                    deleteDelegate();
                }
            }

            public override bool CanMoveRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                return true;
            }
        }
    }
}

