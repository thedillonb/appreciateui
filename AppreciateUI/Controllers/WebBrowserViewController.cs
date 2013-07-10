using MonoTouch.UIKit;
using System.Collections.Generic;
using AppreciateUI.Models;

namespace AppreciateUI.Controllers
{
	public class WebBrowserViewController : BrowserViewController
	{
		public WebBrowserViewController(List<Photo> photos)
			: base(photos)
		{
		}

		protected override UIBarButtonItem CreateActionButton ()
		{
			return new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => SaveSelection());
		}
		
		private void SaveSelection()
		{
            if (this.GetCurrentPhoto() == null)
                return;

            var photo = this.GetCurrentPhoto() as Photo;
			if (photo == null)
				return;
			
			var img = photo.UnderlyingImage;
			if (img == null)
				return;

			var ctrl = new AddToAlbumViewController(img, photo.Caption, photo.Icon);
			ctrl.Success = () => {
				NavigationController.PopToViewController(this, true);
				this.ShowProgressHUDWithMessage("Saving...");
				this.ShowProgressHUDCompleteMessage("Saved!");
			};
			NavigationController.PushViewController(ctrl, true);
		}
	}
}

