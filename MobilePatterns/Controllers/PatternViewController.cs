using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using System.Threading;
using MobilePatterns.Cells;
using CollectionViewBinding;
using System.Collections.Generic;

namespace MobilePatterns.Controllers
{

    public abstract class PatternViewController : UIViewController
    {
        protected CollectionViewBinding.PSCollectionView _collectionView;
        PhotoBrowser.MWPhotoBrowser _photoBrowser;
        PhotoBrowserDelegate _browserDelegate;
        
        public PatternViewController ()
            : base ()
        {
            _browserDelegate = new PhotoBrowserDelegate(this);
            _photoBrowser = new CustomBrowser(_browserDelegate);
            _photoBrowser.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
            _photoBrowser.WantsFullScreenLayout = true;
            _photoBrowser.DisplayActionButton = true;

            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => {
                NavigationController.PopViewControllerAnimated(true);
            });
        }

        protected abstract int OnGetItemsInCollection();

        protected abstract void OnAssignObject(PatternCell view, int index);

        protected abstract PhotoBrowser.MWPhoto OnGetPhoto(int index);

        protected virtual void OnClickItem(PSCollectionView view, PatternCell cell, int index)
        {
            _photoBrowser.SetInitialPageIndex(index);
            NavigationController.PushViewController(_photoBrowser, true);
            //PresentModalViewController(new UINavigationController(_photoBrowser), true);
        }

        public class CustomBrowser : PhotoBrowser.MWPhotoBrowser
        {
            public CustomBrowser(PhotoBrowserDelegate del)
                : base(del)
            {
                NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => {
                    NavigationController.PopViewControllerAnimated(true);
                });
            }

            public override void SetNavBarAppearance(bool animated)
            {
                NavigationController.NavigationBar.BarStyle = UIBarStyle.BlackTranslucent;
                NavigationController.NavigationBar.Alpha = 0.95f;
            }

			public override void ReloadData ()
			{
				base.ReloadData ();

				if (this.View.Subviews.Length == 0)
					return;

				UIToolbar toolbar = null;
				foreach (var view in this.View.Subviews)
					if (view is UIToolbar)
						toolbar = view as UIToolbar;
				
				if (toolbar == null)
					return;
				
				var items = new List<UIBarButtonItem>(toolbar.Items);
				var saveItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => SaveSelection());
				
				items.RemoveAt(items.Count - 1);
				items.Add(saveItem);
				toolbar.SetItems(items.ToArray(), true);
			}

			private void SaveSelection()
			{
				var photo = this.GetCurrentPhoto();
				if (photo == null)
					return;

				var img = photo.UnderlyingImage;
				if (img == null)
					return;

				var ctrl = new AddToAlbumViewController(img, photo.Caption);
				ctrl.Success = () => {
					NavigationController.PopToViewController(this, true);
				};
				NavigationController.PushViewController(ctrl, true);
			}
        }
        
        protected class DS : CollectionViewBinding.PSCollectionViewDataSource
        {
            PatternViewController _t;
            public DS(PatternViewController t)
            {
                _t = t;
            }
            public override CollectionViewBinding.PSCollectionViewCell ViewAtIndex (CollectionViewBinding.PSCollectionView collectionView, int viewAtIndex)
            {
                var v = collectionView.DequeueReusableView() as PatternCell;
                if (v == null)
                    v = new PatternCell();
                _t.OnAssignObject(v, viewAtIndex);
                return v;
            }

            public override int NumberOfViewsInCollectionView (CollectionViewBinding.PSCollectionView collectionView)
            {
                return _t.OnGetItemsInCollection();
            }
            
            public override float HeightForViewAtIndex (int index)
            {
                var width = _t._collectionView.ColWidth;
                var scale = 960f / (640f / width);
                return scale;
            }
        }
        
        public class PhotoBrowserDelegate : PhotoBrowser.MWPhotoBrowserDelegate
        {
            PatternViewController _t;
            public PhotoBrowserDelegate(PatternViewController pvc)
            {
                _t = pvc;
            }
            
            public override int NumberOfPhotosInPhotoBrowser (PhotoBrowser.MWPhotoBrowser photoBrowser)
            {
                return _t.OnGetItemsInCollection();
            }
            
            public override PhotoBrowser.MWPhoto PhotoBrowser (PhotoBrowser.MWPhotoBrowser photoBrowser, int photoAtIndex)
            {
				return _t.OnGetPhoto(photoAtIndex);
            }
        }
        
        private class ClickDelegate : CollectionViewBinding.PSCollectionViewDelegate
        {
            PatternViewController _t;
            public ClickDelegate(PatternViewController t)
            {
                _t = t;
            }
            public override void OnDidSelectView(PSCollectionView a, PSCollectionViewCell v, int index)
            {
                var cell = v as PatternCell;
                if (cell != null)
                    _t.OnClickItem(a, cell, index);
            }
        }


        
        private DS ds;
        private ClickDelegate cd;
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            ds = new DS(this);
            cd = new ClickDelegate(this);
            
            this.View.BackgroundColor = UIColor.FromPatternImage(Images.Background);
            
            _collectionView = new CollectionViewBinding.PSCollectionView(this.View.Bounds);
            _collectionView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _collectionView.NumColsLandscape = 2;
            _collectionView.NumColsPortrait = 2;
            _collectionView.BackgroundColor = UIColor.Clear;
            _collectionView.PSCollectionViewDataSourceDelegate = ds;
            _collectionView.PSCollectionViewDelegate = cd;
            this.View.AddSubview(_collectionView);
        }
    }
}

