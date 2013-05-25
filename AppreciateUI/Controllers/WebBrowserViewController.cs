using MonoTouch.UIKit;
using System.Collections.Generic;

namespace AppreciateUI.Controllers
{
	public class WebBrowserViewController : BrowserViewController
	{
		public WebBrowserViewController(List<PhotoBrowser.MWPhoto> photos)
			: base(photos)
		{
		}

		protected override UIBarButtonItem CreateActionButton ()
		{
			return new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => SaveSelection());
		}
		
		private void SaveSelection()
		{
			var photo = this.GetCurrentPhoto();
			if (photo == null)
				return;
			
			var img = photo.UnderlyingImage;
			if (img == null)
				return;

			var ctrl = new AddToAlbumViewController(img, photo.Caption);
			ctrl.Success = () => {
				NavigationController.PopToViewController(this, true);
				this.ShowProgressHUDWithMessage("Saving...");
				this.ShowProgressHUDCompleteMessage("Saved!");
			};
			NavigationController.PushViewController(ctrl, true);
		}
	}
}

