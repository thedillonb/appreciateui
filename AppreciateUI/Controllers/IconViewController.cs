using System;
using CollectionViewBinding;
using AppreciateUI.Cells;
using MonoTouch.UIKit;

namespace AppreciateUI.Controllers
{
    public abstract class IconViewController : UIViewController
    {
        protected CollectionViewBinding.PSCollectionView CollectionView;

        protected IconViewController()
        {
            NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Plain, (s, e) => NavigationController.PopViewControllerAnimated(true));
        }

        protected abstract BrowserViewController CreateBrowserViewController();

        protected abstract int OnGetItemsInCollection();

        protected abstract void OnAssignObject(IconCell view, int index);

        protected virtual void OnClickItem(PSCollectionView view, IconCell cell, int index)
        {
            var photoBrowser = CreateBrowserViewController();
            photoBrowser.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
            photoBrowser.DisplayActionButton = true;
            photoBrowser.SetInitialPageIndex(index);
            PresentViewController(new UINavigationController(photoBrowser), true, null);
        }

        protected class CollectionDataSource : CollectionViewBinding.PSCollectionViewDataSource
        {
            readonly IconViewController _t;
            public CollectionDataSource(IconViewController t)
            {
                _t = t;
            }
            public override CollectionViewBinding.PSCollectionViewCell ViewAtIndex (CollectionViewBinding.PSCollectionView collectionView, int viewAtIndex)
            {
                var v = collectionView.DequeueReusableView() as IconCell ?? new IconCell();
                _t.OnAssignObject(v, viewAtIndex);
                return v;
            }

            public override int NumberOfViewsInCollectionView (CollectionViewBinding.PSCollectionView collectionView)
            {
                return _t.OnGetItemsInCollection();
            }

            public override float HeightForViewAtIndex (int index)
            {
                return _t.CollectionView.ColWidth;
            }
        }

        private class ClickDelegate : CollectionViewBinding.PSCollectionViewDelegate
        {
            readonly IconViewController _t;
            public ClickDelegate(IconViewController t)
            {
                _t = t;
            }
            public override void OnDidSelectView(PSCollectionView a, PSCollectionViewCell v, int index)
            {
                var cell = v as IconCell;
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
            CollectionView.NumColsLandscape = CollectionView.NumColsPortrait = 4;
            CollectionView.BackgroundColor = UIColor.Clear;
            CollectionView.PSCollectionViewDataSourceDelegate = _collectionDataSource;
            CollectionView.PSCollectionViewDelegate = _cd;

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                CollectionView.NumColsPortrait = CollectionView.NumColsLandscape = 5;
            }

            this.View.AddSubview(CollectionView);
        }
    }
}

