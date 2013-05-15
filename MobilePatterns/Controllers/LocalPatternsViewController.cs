using System;
using System.Collections.Generic;
using MobilePatterns.Models;
using MonoTouch.UIKit;
using MobilePatterns.Cells;

namespace MobilePatterns.Controllers
{
	public class LocalViewPatternsViewController : PatternViewController
	{
		List<PhotoBrowser.MWPhoto> _photos = new List<PhotoBrowser.MWPhoto>();
		UIImage[] _thumbs;
		public ProjectImage[] Images { get; set; }
		
		public LocalViewPatternsViewController(ProjectImage[] images)
			: base(true)
		{
			Images = images;
		}
		
		//        private void DeleteImage()
		//        {
		//            var indexes = this.TableView.IndexPathsForVisibleRows;
		//            if (indexes.Length == 0)
		//                return;
		//
		//            var element = Root[indexes[0].Section][indexes[0].Row];
		//            var item = element as LocalPatternImageElement;
		//            if (item == null)
		//                return;
		//
		//            //Remove the image!
		//            item.ProjectImage.Remove();
		//            Root[indexes[0].Section].Remove(item);
		//
		//            //If you've deleted them all return to the other screen.
		//            if (Root[indexes[0].Section].Count == 0)
		//            {
		//                NavigationController.PopViewControllerAnimated(true);
		//            }
		//        }
		
		protected override int OnGetItemsInCollection ()
		{
			return Images.Length;
		}
		
		protected override void OnAssignObject (PatternCell view, int index)
		{
			if (index < Images.Length)
				view.FillWithLocal(_thumbs[index]);
		}
		
		protected override PhotoBrowser.MWPhoto OnGetPhoto (int index)
		{
			if (index < Images.Length)
				return _photos[index];
			return null;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			_thumbs = new UIImage[Images.Length];
			for (int i = 0; i < Images.Length; i++)
			{
				var img = Images[i];
				_thumbs[i] = UIImage.FromFile(img.ThumbPath);
				var photo = new PhotoBrowser.MWPhoto(UIImage.FromFile(img.Path));
				if (img.Category != null)
					photo.Caption = img.Category;
				_photos.Add(photo);
			}
			_collectionView.ReloadData();
		}
	}

}

