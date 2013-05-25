using AppreciateUI.Cells;
using MonoTouch.UIKit;
using CollectionViewBinding;

namespace AppreciateUI.Controllers
{
    public abstract class PatternViewController : UIViewController
    {
        protected CollectionViewBinding.PSCollectionView CollectionView;

        protected PatternViewController()
        {
            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => NavigationController.PopViewControllerAnimated(true));
        }

		protected abstract BrowserViewController CreateBrowserViewController();

        protected abstract int OnGetItemsInCollection();

        protected abstract void OnAssignObject(PatternCell view, int index);

        protected virtual void OnClickItem(PSCollectionView view, PatternCell cell, int index)
        {
			var photoBrowser = CreateBrowserViewController();
			photoBrowser.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
			photoBrowser.WantsFullScreenLayout = true;
			photoBrowser.DisplayActionButton = true;
			photoBrowser.SetInitialPageIndex(index);
			NavigationController.PushViewController(photoBrowser, true);
        }
        
        protected class CollectionDataSource : CollectionViewBinding.PSCollectionViewDataSource
        {
            readonly PatternViewController _t;
            public CollectionDataSource(PatternViewController t)
            {
                _t = t;
            }
            public override CollectionViewBinding.PSCollectionViewCell ViewAtIndex (CollectionViewBinding.PSCollectionView collectionView, int viewAtIndex)
            {
                var v = collectionView.DequeueReusableView() as PatternCell ?? new PatternCell();
                _t.OnAssignObject(v, viewAtIndex);
                return v;
            }

            public override int NumberOfViewsInCollectionView (CollectionViewBinding.PSCollectionView collectionView)
            {
                return _t.OnGetItemsInCollection();
            }
            
            public override float HeightForViewAtIndex (int index)
            {
                var width = _t.CollectionView.ColWidth;
                var scale = 960f / (640f / width);
                return scale;
            }
        }
   
        private class ClickDelegate : CollectionViewBinding.PSCollectionViewDelegate
        {
            readonly PatternViewController _t;
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


        
        private CollectionDataSource _collectionDataSource;
        private ClickDelegate _cd;
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            _collectionDataSource = new CollectionDataSource(this);
            _cd = new ClickDelegate(this);
            
            this.View.BackgroundColor = UIColor.FromPatternImage(Images.Background);
            
            CollectionView = new CollectionViewBinding.PSCollectionView(this.View.Bounds);
            CollectionView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            CollectionView.NumColsLandscape = 4;
            CollectionView.NumColsPortrait = 4;
            CollectionView.BackgroundColor = UIColor.Clear;
            CollectionView.PSCollectionViewDataSourceDelegate = _collectionDataSource;
            CollectionView.PSCollectionViewDelegate = _cd;
            this.View.AddSubview(CollectionView);
        }
    }
}

