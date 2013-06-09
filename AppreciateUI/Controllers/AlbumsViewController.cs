using System;
using AppreciateUI.Cells;
using AppreciateUI.Models;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Linq;

namespace AppreciateUI.Controllers
{
    public class AlbumsViewController : BaseDialogViewController
    {
        public AlbumsViewController()
        {
            Title = "Albums";
            TabBarItem.Image = Images.Tag;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => {
                PresentModalViewController(new UINavigationController(new NewAlbumViewController((r) => {
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
            var imageCount = Data.Database.Main.Table<ProjectImage>().Count();
            var allPatternsButton = new StyledElement("All UI Images", imageCount.ToString(), UITableViewCellStyle.Value1);
            if (imageCount > 0)
            {
                allPatternsButton.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                allPatternsButton.Tapped += () => NavigationController.PushViewController(new LocalViewPatternsViewController() { Title = "All" }, true);
            }

            var section = new Section();
            var projects = Data.Database.Main.Table<Project>();
            foreach (var p in projects)
            {
                var project = p;
                var element = new ProjectElement(project);
                if (Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == project.Id).Count() > 0)
                {
                    element.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    element.Tapped += () => {
                        NavigationController.PushViewController(new LocalViewPatternsViewController(project.Id) { Title = project.Name }, true);
                    };
                }
                section.Add(element);
            }

            var root = new RootElement(Title) { new Section() { allPatternsButton }, section };
            Root = root;
        }

        public override Source CreateSizingSource(bool unevenRows)
        {
            return new EditingSource(this);
        }

        private class ProjectElement : StyledElement
        {
            public readonly Project Project;
            public ProjectElement(Project p)
                : base(p.Name, "", UITableViewCellStyle.Value1)
            {
                Project = p;
                var count = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == p.Id).Count();
                this.Value = count.ToString();
            }
        }


        private class EditingSource : Source 
        {
            readonly AlbumsViewController _parent;
            public EditingSource(AlbumsViewController dvc)
                : base(dvc)
            {
                _parent = dvc;
            }

            public override bool CanEditRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                return true;
            }

            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                //var section = Container.Root [indexPath.Section];
                //var element = section [indexPath.Row];

                if (indexPath.Section == 0)
                    return UITableViewCellEditingStyle.None;
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
                    _parent.LoadTable();
                };

                var count = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == item.Project.Id).Count();
                if (count > 0)
                {
                    var alert = new UIAlertView {Title = "Confirm?", Message = "Are you sure you want to delete this project and all " + count + " images?"};
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

