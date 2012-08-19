using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MobilePatterns.Models;

namespace MobilePatterns.Controllers
{
    public class ProjectPatternsViewController : DialogViewController
    {
        public ProjectPatternsViewController()
            : base (UITableViewStyle.Plain, null)
        {
            Title = "Projects";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            var section = new Section();
            var projects = Data.Database.Main.Table<Project>();
            foreach (var p in projects)
            {
                var project = p;
                var element = new StyledStringElement(project.Name, () => {
                    NavigationController.PushViewController(new LocalViewPatternsViewController() 
                                                            { Project = project, Title = project.Name }, true);
                }) { Accessory = UITableViewCellAccessory.DisclosureIndicator };
                section.Add(element);
            }

            var root = new RootElement(Title) { section };
            Root = root;
        }
    }
}

