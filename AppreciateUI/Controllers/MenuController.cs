using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using System.Linq;
using AppreciateUI.Views;
using AppreciateUI.Cells;
using AppreciateUI.Utils;
using AppreciateUI.Models;
using System;

namespace AppreciateUI.Controllers
{
    public class MenuController : DialogViewController
    {
        UIImageView _title;
        MenuElement _allProjects;

        public MenuController()
            : base(UITableViewStyle.Plain, new RootElement("AppreciateUI"))
        {
            Autorotate = true;
            _title = new UIImageView(new RectangleF(0, 0, 260, 40));
            _title.Image = Images.Components.Title;
            _title.BackgroundColor = UIColor.Clear;
            NavigationItem.TitleView = _title;
            Title = Root.Caption;
        }

		/// <summary>
		/// Invoked when it comes time to set the root
		/// </summary>
		private void CreateMenu()
		{
            var root = new RootElement(Title);
            root.Add(new Section() {
                new MenuElement("Add Pattern", () => OpenAddPatternView(), Images.Menu.Plus)
            });

            var browseSection = new Section() { HeaderView = new MenuSectionView("Browse") };
            root.Add(browseSection);
            browseSection.Add(new MenuElement("Recently Added", () => { 
                var c = new RecentPatternsViewController();
                NavigationController.PushViewController(c, true);
            }, Images.Menu.Recent));
            browseSection.Add(new MenuElement("UI Patterns", () => { 
                var c = new PatternCategoriesViewController();
                NavigationController.PushViewController(c, true);
            }, Images.Menu.UIPatterns));
            browseSection.Add(new MenuElement("Icons", () => { 
                var c = new IconBrowserController();
                NavigationController.PushViewController(c, true);
            }, Images.Menu.Icons));

      

            var albumSection = new Section() { HeaderView = new MenuSectionView("Albums") };
            root.Add(albumSection);

            var imageCount = Data.Database.Main.Table<ProjectImage>().Count();
            _allProjects = new MenuElement("All Albums", imageCount.ToString(), UITableViewCellStyle.Value1) { Image = Images.Menu.AllAlbums };
            _allProjects.Tapped += () => {
                if (Data.Database.Main.Table<ProjectImage>().Count() > 0)
                    NavigationController.PushViewController(new LocalViewPatternsViewController() { Title = "All" }, true);
                else
                {
                }
            };
            albumSection.Add(_allProjects);

            var projects = Data.Database.Main.Table<Project>();
            foreach (var p in projects)
            {
                var project = p;
                var element = new ProjectElement(project, this) { Image = Images.Menu.Album };
                albumSection.Add(element);
            }

//            albumSection.Add(new MenuElement("Add Album", () => {
//                PresentViewController(new UINavigationController(new NewAlbumViewController((r) => {
//                    DismissViewController(true, null);
//                })), true, null);
//            }, null));
//

            var moreSection = new Section() { HeaderView = new MenuSectionView("Info") };
            root.Add(moreSection);
            moreSection.Add(new MenuElement("About", () => NavigationController.PushViewController(new AboutController(), true), Images.Menu.Info));
            moreSection.Add(new MenuElement("Feedback & Support", () => { 
                var config = UserVoice.UVConfig.Create("appreciateui.uservoice.com", "y2jtRDr35UyLi2fjSv16bA", "x9U5XhzGUPdsghNbzNq3UHxtGDeuETsuwT4ufmV2Q");
                UserVoice.UserVoice.PresentUserVoiceInterface(this, config);
            }, Images.Menu.Feedback));

            Root = root;
		}

        private void Delete(ProjectElement element)
        {
            element.Project.Remove();

            //Shoudl always be true
            if (element.Parent is Section)
                ((Section)element.Parent).Remove(element);


            //_parent.CreateMenu();
            if (_allProjects != null)
            {
                var imageCount = Data.Database.Main.Table<ProjectImage>().Count();
                _allProjects.Value = imageCount.ToString();
                _allProjects.GetImmediateRootElement().Reload(_allProjects, UITableViewRowAnimation.None);
            }
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			//Add some nice looking colors and effects
            TableView.SeparatorColor = UIColor.FromRGB(14, 14, 14);
            TableView.TableFooterView = new UIView(new RectangleF(0, 0, View.Bounds.Width, 0));
            TableView.BackgroundColor = UIColor.FromRGB(34, 34, 34);

            //Prevent the scroll to top on this view
            this.TableView.ScrollsToTop = false;
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            CreateMenu();
        }

        private UIPopoverController _pop;
        private void OpenAddPatternView()
        {
            UIImagePickerController ctrl = null;
            ctrl = Camera.SelectPicture(this, (dic) => { 

                var original = dic[UIImagePickerController.OriginalImage] as UIImage;
                if (original == null)
                    return;

                var atsvc = new AddToAlbumViewController(original, null, false);
                atsvc.Success = () => DismissPopupOfModal(ctrl);
                ctrl.PushViewController(atsvc, true);
            }, () => { 
                DismissPopupOfModal(ctrl);
            });

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
                _pop = new UIPopoverController (ctrl);
                _pop.PopoverContentSize = new System.Drawing.SizeF(320, 480);
                _pop.PresentFromRect(this.View.Frame, this.View, UIPopoverArrowDirection.Left, false);
            } 
            else {
                PresentViewController(ctrl, true, null);
            }

        }

        

        private void DismissPopupOfModal(UIImagePickerController ctrl)
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                if (_pop != null)
                {
                    _pop.Dismiss(true);
                    _pop.Dispose();
                    _pop = null;
                }

                //Create the Menu again cause we made a change...
                CreateMenu();
            }
            else
            {
                ctrl.DismissViewController(true, null);
            }
        }

        private class ProjectElement : MenuElement
        {
            public readonly Project Project;
            public ProjectElement(Project p, UIViewController ctrl)
                : base(p.Name, "", UITableViewCellStyle.Value1)
            {
                Project = p;
                var count = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == p.Id).Count();
                this.Value = count.ToString();

                if (Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == p.Id).Count() > 0)
                {
                    this.Tapped += () => {
                        ctrl.NavigationController.PushViewController(new LocalViewPatternsViewController(p.Id) { Title = p.Name }, true);
                    };
                }
            }
        }

		private class MenuElement : StyledElement
		{
            public MenuElement(string title, string detail, UITableViewCellStyle style)
                : base(title, detail, style)
            {
                BackgroundColor = UIColor.Clear;
                TextColor = UIColor.FromRGB(213, 213, 213);
                DetailColor = UIColor.White;        DetailColor = UIColor.White;
            }

			public MenuElement(string title, NSAction tapped, UIImage image)
				: base(title, tapped)
			{
				BackgroundColor = UIColor.Clear;
				TextColor = UIColor.FromRGB(213, 213, 213);
				DetailColor = UIColor.White;
                Image = image;
			}
			
			public override UITableViewCell GetCell(UITableView tv)
			{
				var cell = base.GetCell(tv);
                cell.TextLabel.ShadowColor = UIColor.Black;
                cell.TextLabel.ShadowOffset = new SizeF(0, -1); 
                cell.SelectedBackgroundView = new UIView { BackgroundColor = UIColor.FromRGB(25, 25, 25) };
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				
				var f = cell.Subviews.Count(x => x.Tag == 1111);
				if (f == 0)
				{
					var v = new UIView(new RectangleF(0, 0, cell.Frame.Width, 1))
					{ BackgroundColor = UIColor.FromRGB(44, 44, 44), Tag = 1111};
					cell.AddSubview(v);
				}
				
				return cell;
			}
		}

        public override Source CreateSizingSource(bool unevenRows)
        {
            return new EditingSource(this);
        }
       
        private class EditingSource : Source 
        {
            readonly MenuController _parent;
            public EditingSource(MenuController dvc)
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
                if (indexPath.Section != 2 || indexPath.Row == 0)
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

                var count = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == item.Project.Id).Count();
                if (count > 0)
                {
                    var alert = new UIAlertView {Title = "Confirm?", Message = "Are you sure you want to delete this project and all " + count + " images?"};
                    alert.CancelButtonIndex = alert.AddButton("No");
                    var ok = alert.AddButton("Yes");

                    alert.Clicked += (sender, e) => {
                        if (e.ButtonIndex == ok)
                            _parent.Delete(item);
                    };

                    alert.Show();
                }
                else
                {
                    _parent.Delete(item);
                }
            }

            public override bool CanMoveRow(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                return true;
            }
        }
    }
}

