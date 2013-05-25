using AppreciateUI.Models;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace AppreciateUI.Controllers
{
	public class LocalBrowserViewController : BrowserViewController
	{
	    readonly List<ProjectImage> _projectImages;

		public LocalBrowserViewController(List<PhotoBrowser.MWPhoto> photos, List<ProjectImage> projectImages)
			: base(photos)
		{
			_projectImages = projectImages;
		}

		protected override UIBarButtonItem CreateActionButton ()
		{
			return new UIBarButtonItem(UIBarButtonSystemItem.Trash, (s, e) => {

				var currentIndex = this.CurrentIndex;

				Photos.RemoveAt(currentIndex);
				_projectImages[currentIndex].Remove();
				_projectImages.RemoveAt(currentIndex);

				this.ShowProgressHUDWithMessage("Deleting...");
				this.ShowProgressHUDCompleteMessage("Deleted!");

			
				if (Photos.Count == 0)
				{
					//Nothing more to see...
					NavigationController.PopViewControllerAnimated(true);
					return;
				}

				if (currentIndex == 0)
				{
					this.GotoNextPage();
				}
				else
				{
					this.GotoPreviousPage();
				}

				this.ReloadData();
			});
		}
	}
}

