using System;
using MonoTouch.UIKit;
using AppreciateUI.Views;

namespace AppreciateUI.Controllers
{
	public class SlideoutNavigationController : MonoTouch.SlideoutNavigation.SlideoutNavigationController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CodeBucket.Controllers.SlideoutNavigationController"/> class.
		/// </summary>
		public SlideoutNavigationController()
		{
			//Setting the height to a large amount means that it will activate the slide pretty much whereever your finger is on the screen.
            BackgroundColor = UIColor.FromRGB(227, 227, 227);
		}

		/// <summary>
		/// Creates the menu button.
		/// </summary>
		/// <returns>The menu button.</returns>
		protected override UIBarButtonItem CreateMenuButton()
		{
            //return new UIBarButtonItem(NavigationButton.Create(Images.Buttons.ThreeLines, () => Show()));
            return new UIBarButtonItem(Images.Buttons.ThreeLines, UIBarButtonItemStyle.Bordered, (s, e) => Show());
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			MenuView = new MenuController();
		}
	}
}

