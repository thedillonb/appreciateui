using System.Collections.Generic;
using AppreciateUI.Cells;
using AppreciateUI.Models;
using MonoTouch.UIKit;
using System.Linq;

namespace AppreciateUI.Controllers
{
	public class LocalViewPatternsViewController : PatternViewController
	{
		UIImage[] _thumbs;
        List<ProjectImage> _images;
        readonly int? _projectId;
		
		public LocalViewPatternsViewController(List<ProjectImage> images)
		{
			_images = images;
		}

        public LocalViewPatternsViewController()
            : this(-1)
        {
        }

        public LocalViewPatternsViewController(int projectId)
        {
            _projectId = projectId;
            _images = new List<ProjectImage>();
        }

		protected override BrowserViewController CreateBrowserViewController()
		{
			var photos = new List<PhotoBrowser.MWPhoto>(_images.Count);
			foreach (var img in _images)
			{
				var photo = new PhotoBrowser.MWPhoto(UIImage.FromFile(img.Path));
				if (img.Category != null)
					photo.Caption = img.Category;
				photos.Add(photo);
			}

			return new LocalBrowserViewController(photos, _images);
		}
		
		protected override int OnGetItemsInCollection ()
		{
			return _images.Count;
		}
		
		protected override void OnAssignObject (PatternCell view, int index)
		{
			if (index < _images.Count)
			{
				if (_thumbs[index] == null)
				{
					_thumbs[index] = UIImage.FromFile(_images[index].ThumbPath);
				}

				view.FillWithLocal(_thumbs[index]);
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

            //If we used a project id lets update the images, just incase they changed!
            if (_projectId.HasValue)
            {
                //A project id of -1 indicates everything
                if (_projectId == -1)
                    _images = Data.Database.Main.Table<ProjectImage>().ToList();
                else
                    _images = Data.Database.Main.Table<ProjectImage>().Where(a => a.ProjectId == _projectId.Value).ToList();
            }

			if (_thumbs != null)
				foreach (var img in _thumbs)
					if (img != null) 
						img.Dispose();
			_thumbs = new UIImage[_images.Count];
            CollectionView.ReloadData();
		}
	}

}

