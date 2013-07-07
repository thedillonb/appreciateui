using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using AppreciateUI.Elements;
using AppreciateUI.Cells;
using MonoTouch.MessageUI;

namespace AppreciateUI.Controllers
{
    public class AboutController : BaseDialogViewController
    {
        static readonly string About = "AppreciateUI is the ideal tool for gathering inspiration when designing a mobile user interface. " +
                                       "Screenshots from hundreds of apps all within the palm of your hand. Browse the latest designs added weekly, or view by category to find just the design to inspire your own creation. " +
                                       "\n\nCreated By Dillon Buchanan";

        public AboutController()
        {
            Title = "About";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var root = new RootElement(Title);
            root.Add(new Section()
            {
                new MultilinedElement("AppreciateUI") { Value = About }
            });

            root.Add(new Section()
            {
                new StyledElement("Source Code", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://github.com/thedillonb/appreciateui")))
            });


            if (MFMailComposeViewController.CanSendMail)
            {
                root.Add(new Section()
                {
                    new StyledElement("Contact Me", OpenMailer)
                });
            }

            root.Add(new Section(String.Empty, "Thank you for downloading. Enjoy!")
            {
                new StyledElement("Follow Me On Twitter", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://twitter.com/thedillonb"))),
                new StyledElement("Rate This App", () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/appreciateui/id651317060?ls=1&mt=8"))),
                new StyledElement("App Version", NSBundle.MainBundle.InfoDictionary.ValueForKey(new NSString("CFBundleVersion")).ToString())
            });

            root.UnevenRows = true;
            Root = root;
        }

        private void OpenMailer()
        {
            var mailer = new MFMailComposeViewController();
            mailer.SetSubject("AppreciateUI Feedback");
            mailer.SetToRecipients(new string[] { "appreciateuiapp@gmail.com" });
            mailer.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
            mailer.Finished += (sender, e) => this.DismissViewController(true, null);
            this.PresentViewController(mailer, true, null);
        }
    }
}

