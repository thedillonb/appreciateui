using System;
using MonoTouch.Dialog;
using MobilePatterns.Cells;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.MessageUI;

namespace MobilePatterns.Controllers
{
	public class SettingsViewController : BaseDialogViewController
	{
		public SettingsViewController ()
		{
			Title = "Settings";
			TabBarItem.Image = Images.Gear;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			var root = new RootElement(Title);

			var supportSection = new Section("Support");
			root.Add (supportSection);
			//supportSection.Add(new StyledElement("Technical Support", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://twitter.com/Codebucketapp"))));

			if (MFMailComposeViewController.CanSendMail)
				supportSection.Add(new StyledElement("Contact Me", OpenMailer));
			
			
			var aboutSection = new Section("About", "Thank you for downloading. Enjoy!");
			root.Add(aboutSection);
			aboutSection.Add(new StyledElement("Follow On Twitter", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://twitter.com/appreciateuiapp"))));
			//aboutSection.Add(new StyledElement("Rate This App", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/codebucket/id551531422?mt=8"))));
			aboutSection.Add(new StyledElement("App Version", NSBundle.MainBundle.InfoDictionary.ValueForKey(new NSString("CFBundleVersion")).ToString()));

			
			//Assign the root
			Root = root;

		}

		private void OpenMailer()
		{
			var mailer = new MFMailComposeViewController();
			mailer.SetSubject("AppreciateUI Feedback");
			mailer.SetToRecipients(new string[] { "appreciateuiapp@gmail.com" });
			mailer.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
			mailer.Finished += (sender, e) => this.DismissModalViewControllerAnimated(true);
			this.PresentModalViewController(mailer, true);
		}
	}
}

