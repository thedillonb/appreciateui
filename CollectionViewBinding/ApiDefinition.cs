using System;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CollectionViewBinding
{
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native library (or libraries)
    // that you want to bind.
    //
    // When you do that, you'll notice that MonoDevelop generates a code-behind file for each
    // native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
    // architectures that the native library supports and fills in that information for you,
    // however, it cannot auto-detect any Frameworks or other system libraries that the
    // native library may depend on, so you'll need to fill in that information yourself.
    //
    // Once you've done that, you're ready to move on to binding the API...
    //
    //
    // Here is where you'd define your API definition for the native Objective-C library.
    //
    // For example, to bind the following Objective-C class:
    //
    //     @interface Widget : NSObject {
    //     }
    //
    // The C# binding would look like this:
    //
    //     [BaseType (typeof (NSObject))]
    //     interface Widget {
    //     }
    //
    // To bind Objective-C properties, such as:
    //
    //     @property (nonatomic, readwrite, assign) CGPoint center;
    //
    // You would add a property definition in the C# interface like so:
    //
    //     [Export ("center")]
    //     PointF Center { get; set; }
    //
    // To bind an Objective-C method, such as:
    //
    //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
    //
    // You would add a method definition to the C# interface like so:
    //
    //     [Export ("doSomething:atIndex:")]
    //     void DoSomething (NSObject object, int index);
    //
    // Objective-C "constructors" such as:
    //
    //     -(id)initWithElmo:(ElmoMuppet *)elmo;
    //
    // Can be bound as:
    //
    //     [Export ("initWithElmo:")]
    //     IntPtr Constructor (ElmoMuppet elmo);
    //
    // For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_types
    //

    [BaseType(typeof(UIView))]
    public interface PSCollectionViewCell
    {
        [Export("prepareForReuse")]
        void PrepareForReuse();

        [Export("fillViewWithObject:object")]
        [Abstract]
        void FillViewWithObject(MonoTouch.Foundation.NSObject obj);

        [Export("heightForViewWithObject:inColumnWidth:")]
        [Abstract]
        float HeightForViewWithObject(MonoTouch.Foundation.NSObject obj, float columnWidth);
    }

    [BaseType(typeof(UIScrollView))]
    public interface PSCollectionView
    {
        [Export("initWithFrame:")]
        IntPtr Constructor(System.Drawing.RectangleF frame);

        [Export("colWidth")]
        float ColWidth { get; }

        [Export("numCols")]
        int NumCols { get; }

        [Export("numColsLandscape")]
        int NumColsLandscape { get; set; }

        [Export("numColsPortrait")]
        int NumColsPortrait { get; set; }

        [Export("headerView")]
        UIView HeaderView { get; set; }

        [Export("footerView")]
        UIView FooterView { get; set; }

        [Export("emptyView")]
        UIView EmptyView { get; set; }

        [Export("loadingView")]
        UIView LoadingView { get; set; }

        [Export("reloadData")]
        void ReloadData();

        [Export("dequeueReusableView")]
        UIView DequeueReusableView();

        [Wrap ("PSCollectionViewDataSourceWeakDelegate")]
        PSCollectionViewDataSource PSCollectionViewDataSourceDelegate { get; set; }
        
        [Export ("collectionViewDataSource", ArgumentSemantic.Assign)]
        [NullAllowed]
        NSObject PSCollectionViewDataSourceWeakDelegate { get; set; }

        [Wrap ("PSCollectionViewDelegateWeakDelegate")]
        PSCollectionViewDelegate PSCollectionViewDelegate { get; set; }
        
        [Export ("collectionViewDelegate", ArgumentSemantic.Assign)]
        [NullAllowed]
        NSObject PSCollectionViewDelegateWeakDelegate { get; set; }
    }

    [BaseType (typeof (NSObject))]
    [Model]
    public interface PSCollectionViewDataSource {
        [Export ("numberOfViewsInCollectionView:")]
        int NumberOfViewsInCollectionView (PSCollectionView collectionView);

        [Export ("collectionView:viewAtIndex:")]
        PSCollectionViewCell ViewAtIndex (PSCollectionView collectionView, int viewAtIndex);

        [Export ("heightForViewAtIndex:")]
        float HeightForViewAtIndex(int index);
    }


    [BaseType (typeof (NSObject))]
    [Model]
    public interface PSCollectionViewDelegate {
        [Export ("collectionView:didSelectView:atIndex:")]
        void OnDidSelectView(PSCollectionView a, PSCollectionViewCell v, int index);
    }
}

