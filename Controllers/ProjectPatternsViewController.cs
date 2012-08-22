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
            TabBarItem.Image = Images.Tag;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            var section = new Section();
            var projects = Data.Database.Main.Table<Project>();
            foreach (var p in projects)
            {
                var project = p;
                var count = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == project.Id).Count();
                var element = new StyledStringElement(project.Name, count.ToString(), UITableViewCellStyle.Value1);
                element.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                element.Tapped += () => {
                    NavigationController.PushViewController(new LocalViewPatternsViewController() 
                                                            { Project = project, Title = project.Name }, true);
                };
                section.Add(element);
            }

            var root = new RootElement(Title) { section };
            Root = root;
        }
    }
}

