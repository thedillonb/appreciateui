using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using System.Linq;
using AppreciateUI.Views;
using AppreciateUI.Cells;
using AppreciateUI.Utils;
using AppreciateUI.Models;

namespace AppreciateUI.Controllers
{
    public class MenuController : DialogViewController
    {
        UILabel _title;

        public MenuController()
            : base(UITableViewStyle.Plain, new RootElement("AppreciateUI"))
        {
            Autorotate = true;
            _title = new UILabel(new RectangleF(0, 40, 320, 40));
            _title.TextAlignment = UITextAlignment.Left;
            _title.BackgroundColor = UIColor.Clear;
            _title.Font = UIFont.BoldSystemFontOfSize(20f);
            _title.TextColor = UIColor.FromRGB(246, 246, 246);
            _title.ShadowColor = UIColor.FromRGB(21, 21, 21);
            _title.ShadowOffset = new SizeF(0, 1);
            NavigationItem.TitleView = _title;
            Title = Root.Caption;
        }

		/// <summary>
		/// Invoked when it comes time to set the root so the child classes can create their own menus
		/// </summary>
		private void OnCreateMenu(RootElement root)
		{
            root.Add(new Section() {
                new MenuElement("Add Pattern", () => {
                    var c = new AddPatternViewController();
                    PresentViewController(c, true, null);
                }, null)
            });

            var browseSection = new Section() { HeaderView = new MenuSectionView("Browse") };
            root.Add(browseSection);
            browseSection.Add(new MenuElement("Recent", () => { 
                var c = new RecentPatternsViewController();
                NavigationController.PushViewController(c, true);
            }, null));
            browseSection.Add(new MenuElement("Categories", () => { 
                var c = new PatternCategoriesViewController();
                NavigationController.PushViewController(c, true);
            }, null));
            browseSection.Add(new MenuElement("Icons", () => { 
                var c = new IconBrowserController();
                NavigationController.PushViewController(c, true);
            }, null));

      

            var albumSection = new Section() { HeaderView = new MenuSectionView("Albums") };
            root.Add(albumSection);



            var imageCount = Data.Database.Main.Table<ProjectImage>().Count();
            var allPatternsButton = new MenuElement("All UI Images", imageCount.ToString(), UITableViewCellStyle.Value1);
            allPatternsButton.Tapped += () => NavigationController.PushViewController(new LocalViewPatternsViewController() { Title = "All" }, true);
            albumSection.Add(allPatternsButton);

            var projects = Data.Database.Main.Table<Project>();
            foreach (var p in projects)
            {
                var project = p;
                var element = new ProjectElement(project, this);
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
            moreSection.Add(new MenuElement("About", () => NavigationController.PushViewController(new AboutController(), true), null));
            moreSection.Add(new MenuElement("Feedback & Support", () => { 
                var config = UserVoice.UVConfig.Create("http://gistacular.uservoice.com", "lYY6AwnzrNKjHIkiiYbbqA", "9iLse96r8yki4ZKknfHKBlWcbZAH9g8yQWb9fuG4");
                UserVoice.UserVoice.PresentUserVoiceInterface(this, config);
            }, null));

//
//            TabBarController.ViewControllers = new UIViewController[] {
//                new UINavigationController(new RecentPatternsViewController()),
//                new UINavigationController(new PatternCategoriesViewController()),
//                new AddPatternViewController(),
//                new UINavigationController(new AlbumsViewController()),
//            };

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

			var root = new RootElement(Title);
            Title = root.Caption;
			OnCreateMenu(root);
			Root = root;
        }

        private UIViewController _previousController;
        private UIPopoverController _pop;
        private void OpenAddPatternView()
        {
            TabBarController.SelectedViewController = _previousController;
            UIImagePickerController ctrl = null;
            ctrl = Camera.SelectPicture(TabBarController, (dic) => { 

                var original = dic[UIImagePickerController.OriginalImage] as UIImage;
                if (original == null)
                    return;

                var atsvc = new AddToAlbumViewController(original, null);
                atsvc.Success = () => DismissPopupOfModal(ctrl);
                ctrl.PushViewController(atsvc, true);
            }, () => { 
                DismissPopupOfModal(ctrl);
            });

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
                _pop = new UIPopoverController (ctrl);
                _pop.PopoverContentSize = new System.Drawing.SizeF(320, 480);
                _pop.PresentFromRect(TabBarController.TabBar.Frame, TabBarController.View, UIPopoverArrowDirection.Down, false);
            } 
            else {
                TabBarController.PresentModalViewController(ctrl, true);
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
    }
}

