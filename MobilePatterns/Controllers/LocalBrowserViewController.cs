using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MobilePatterns.Models;

namespace MobilePatterns.Controllers
{
	public class LocalBrowserViewController : BrowserViewController
	{
		List<ProjectImage> _projectImages;

		public LocalBrowserViewController(List<PhotoBrowser.MWPhoto> photos, List<ProjectImage> projectImages)
			: base(photos)
		{
			_projectImages = projectImages;
		}

		protected override UIBarButtonItem CreateActionButton ()
		{
			return new UIBarButtonItem(UIBarButtonSystemItem.Trash, (s, e) => {

				var currentIndex = this.CurrentIndex;

				_photos.RemoveAt(currentIndex);
				_projectImages[currentIndex].Remove();
				_projectImages.RemoveAt(currentIndex);

				this.ShowProgressHUDWithMessage("Deleting...");
				this.ShowProgressHUDCompleteMessage("Deleted!");

			
				if (_photos.Count == 0)
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

