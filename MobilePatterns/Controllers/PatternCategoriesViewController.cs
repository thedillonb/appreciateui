using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MobilePatterns.Data;
using System.Threading;
using MobilePatterns.Cells;

namespace MobilePatterns.Controllers
{
    public class PatternCategoriesViewController : BaseDialogViewController
    {
        public PatternCategoriesViewController()
        {
            Title = "Categories";
            TabBarItem.Image = Images.Tray;
            RefreshRequested += (s, e) => Refresh();
        }

        private void Refresh()
        {
            var hud = new RedPlum.MBProgressHUD(View.Frame)
            { Mode = RedPlum.MBProgressHUDMode.Indeterminate };
            hud.TitleText = "Requesting Categories...";
            hud.TitleFont = UIFont.BoldSystemFontOfSize(14f);
            this.ParentViewController.View.AddSubview(hud);
            hud.Show(false);
            
            //Do the loading
            ThreadPool.QueueUserWorkItem(delegate {
                try
                {
                    var data = Data.RequestFactory.GetCategories();
                    var section = new Section();
                    
                    BeginInvokeOnMainThread(() => {
                        hud.Hide(true);
                        hud.RemoveFromSuperview();
                    });
                    
                    data.ForEach(d => {
                        section.Add(new StyledElement(d.Name, () => NavigationController.PushViewController(new WebPatternsViewController(d) { Title = d.Name } , true)));
                    });
                    
                    
                    BeginInvokeOnMainThread(() => { 
                        var root = new RootElement(Title);
                        root.Add(section);
                        Root = root;
                    });
                }
                catch (Exception e)
                {
                    UIAlertView alert = new UIAlertView();
                    alert.Message = e.Message;
                    alert.Title = "Error";
                    alert.CancelButtonIndex = alert.AddButton("Ok");
                    alert.Show();
                }

                BeginInvokeOnMainThread(ReloadComplete);
            });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Refresh();
        }
    }
}

