using System;

namespace MobilePatterns.Controllers
{
	public class RecentPatternsViewController : WebPatternsViewController
	{
		public RecentPatternsViewController ()
		{
			Title = "Recent";
			TabBarItem.Image = Images.Polaroid;
		}
	}
}

