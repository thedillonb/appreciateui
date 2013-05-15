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
		bool _editActive = false;
		readonly bool _local;
		List<int> _selectedList = new List<int>();
        
        public PatternViewController(bool local)
            : base ()
        {
			_local = local;

            _browserDelegate = new PhotoBrowserDelegate(this);
            _photoBrowser = new CustomBrowser(_browserDelegate, !local);
            _photoBrowser.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
            _photoBrowser.WantsFullScreenLayout = true;
            _photoBrowser.DisplayActionButton = true;

			this.ToolbarItems = new UIBarButtonItem[] {
				new UIBarButtonItem(UIBarButtonSystemItem.Trash, (s, e) => {

				})
			};

            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => {
                NavigationController.PopViewControllerAnimated(true);
            });

			if (local)
			{
//				NavigationItem.RightBarButtonItem = new UIBarButtonItem("Edit", UIBarButtonItemStyle.Plain, (s, e) =>  {
//					EditActive(!_editActive);
//				});
			}
        }

		private void EditActive(bool active)
		{
			_editActive = active;
			_selectedList.Clear();

			if (_editActive)
			{
				NavigationItem.SetHidesBackButton(true, true);
				NavigationItem.RightBarButtonItem.Title = "Done";
				NavigationItem.RightBarButtonItem.Style = UIBarButtonItemStyle.Done;
				NavigationController.SetToolbarHidden(false, false);
			}
			else 
			{
				NavigationItem.SetHidesBackButton(false, true);
				NavigationItem.RightBarButtonItem.Title = "Edit";
				NavigationItem.RightBarButtonItem.Style = UIBarButtonItemStyle.Plain;
				NavigationController.SetToolbarHidden(true, false);

				for (var i = 0; i < ds.NumberOfViewsInCollectionView(_collectionView); i++)
				{
					var view = ds.ViewAtIndex(_collectionView, i);
					if (view != null && view is PatternCell)
					{
						((PatternCell)view).SetSelected(false);
					}
				}
			}

		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
//			if (_local)
//				EditActive(false);
		}

        protected abstract int OnGetItemsInCollection();

        protected abstract void OnAssignObject(PatternCell view, int index);

        protected abstract PhotoBrowser.MWPhoto OnGetPhoto(int index);

        protected virtual void OnClickItem(PSCollectionView view, PatternCell cell, int index)
        {
			if (_editActive)
			{
				if (_selectedList.Contains(index))
				{
					_selectedList.Remove(index);
					cell.SetSelected(false);
				}
				else
				{
					_selectedList.Add(index);
					cell.SetSelected(true);
				}
			}
			else
			{
	            _photoBrowser.SetInitialPageIndex(index);
	            NavigationController.PushViewController(_photoBrowser, true);
	            //PresentModalViewController(new UINavigationController(_photoBrowser), true);
			}
        }

        public class CustomBrowser : PhotoBrowser.MWPhotoBrowser
        {
            public CustomBrowser(PhotoBrowserDelegate del, bool saveMenu)
                : base(del)
            {
                NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => {
                    NavigationController.PopViewControllerAnimated(true);
                });

				if (saveMenu)
				{
					NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Save, (s, e) => {
						SaveSelection();
					});
				}
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
				var endItem = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace);
				endItem.Width = items[items.Count - 1].Width;
				items.RemoveAt(items.Count - 1);
				items.Add(endItem);
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
            _collectionView.NumColsLandscape = 4;
            _collectionView.NumColsPortrait = 3;
            _collectionView.BackgroundColor = UIColor.Clear;
            _collectionView.PSCollectionViewDataSourceDelegate = ds;
            _collectionView.PSCollectionViewDelegate = cd;
            this.View.AddSubview(_collectionView);
        }
    }
}

