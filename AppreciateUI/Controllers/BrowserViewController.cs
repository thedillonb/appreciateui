using System;
using MonoTouch.UIKit;
using System.Collections.Generic;

namespace AppreciateUI.Controllers
{
	public abstract class BrowserViewController : PhotoBrowser.MWPhotoBrowser
	{
		protected List<PhotoBrowser.MWPhoto> Photos;

	    protected BrowserViewController(List<PhotoBrowser.MWPhoto> photos)
	    {
			Photos = photos;
			this.PhotoDelegate = new PhotoBrowserDelegate(photos);

			NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => {
				NavigationController.PopViewControllerAnimated(true);
			});
		}
		
		public override void SetNavBarAppearance(bool animated)
		{
			NavigationController.NavigationBar.BarStyle = UIBarStyle.BlackTranslucent;
			NavigationController.NavigationBar.Alpha = 0.95f;
		}

		protected abstract UIBarButtonItem CreateActionButton();
		
		public override void ReloadData ()
		{
			base.ReloadData ();
			
			if (this.View.Subviews.Length == 0)
				return;
			
			UIToolbar toolbar = null;
			foreach (var view in this.View.Subviews)
				if (view is UIToolbar)
					toolbar = view as UIToolbar;
			
			if (toolbar == null)
				return;
			
			var items = new List<UIBarButtonItem>();
			items.Add(new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 16 });
			items.Add(toolbar.Items[toolbar.Items.Length - 1]);
			items.Add(new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace));
			items.Add (CreateActionButton());
			items.Add(new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 16 });
			toolbar.SetItems(items.ToArray(), true);
		}

		public class PhotoBrowserDelegate : PhotoBrowser.MWPhotoBrowserDelegate
		{
		    readonly List<PhotoBrowser.MWPhoto> _photos;
			public PhotoBrowserDelegate(List<PhotoBrowser.MWPhoto> photos)
			{
				_photos = photos;
			}
			
			public override int NumberOfPhotosInPhotoBrowser (PhotoBrowser.MWPhotoBrowser photoBrowser)
			{
				return _photos.Count;
			}
			
			public override PhotoBrowser.MWPhoto PhotoBrowser (PhotoBrowser.MWPhotoBrowser photoBrowser, int photoAtIndex)
			{
				return _photos[photoAtIndex];
			}
		}
	}
}

