using System;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PhotoBrowser
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

    [BaseType (typeof (NSObject))]
    [Model]
    public interface MWPhotoBrowserDelegate {
        [Export ("numberOfPhotosInPhotoBrowser:")]
        [Abstract]
        int NumberOfPhotosInPhotoBrowser (MWPhotoBrowser photoBrowser);
        
        [Export ("photoBrowser:photoAtIndex:")]
        [Abstract]
        MWPhoto PhotoBrowser (MWPhotoBrowser photoBrowser, int photoAtIndex);
        
        [Export ("photoBrowser:captionViewForPhotoAtIndex:")]
        MWCaptionView OnCaption(MWPhotoBrowser photoBrowser,int index);
    }

    [BaseType (typeof (NSObject))]
    public interface MWPhoto
    {
        [Export("caption")]
        string Caption { get; set; }

        [Export("initWithImage:")]
        IntPtr Constructor(UIImage a);

        [Export("initWithFilePath:")]
        IntPtr Constructor(string a);

        [Export("initWithURL:")]
        IntPtr Constructor(NSUrl a);
    }

    [BaseType (typeof (UIView))]
    public interface MWCaptionView
    {
        [Export("initWithPhoto:")]
        IntPtr Constructor(MWPhoto a);
        
        [Export("setupCaption")]
        void SetupCaption();
        
        [Export("sizeThatFits:")]
        System.Drawing.SizeF SizeThatFits(System.Drawing.SizeF a);
    }

    [BaseType (typeof (UIViewController))]
    public interface MWPhotoBrowser
    {
        [Export("displayActionButton")]
        bool DisplayActionButton { get; set; }

        [Export("initWithDelegate:")]
        IntPtr Constructor(MWPhotoBrowserDelegate del);

        [Export("reloadData")]
        void ReloadData();

        [Export("setInitialPageIndex:")]
        void SetInitialPageIndex(int index);

        [ExportAttribute("setNavBarAppearance:")]
        void SetNavBarAppearance(bool animated);

        [Export("storePreviousNavBarAppearance")]
        void StorePreviousNavBarAppearance();

        [ExportAttribute("restorePreviousNavBarAppearance:")]
        void RestorePreviousNavBarAppearance(bool animated);
    }

}

namespace SDWebImage
{
    [BaseType (typeof (UIView))]
    [Model]
    interface SDWebImageManagerDelegate
    {
        [Export ("webImageManager:didProgressWithPartialImage:forURL:userInfo:")]
        void DidProgressWithPartialImage (SDWebImageManager imageManager, UIImage image, NSUrl url, NSDictionary info);
        
        [Export ("webImageManager:didProgressWithPartialImage:forURL:")]
        void DidProgressWithPartialImage (SDWebImageManager imageManager, UIImage image, NSUrl url);
        
        [Export ("webImageManager:didFinishWithImage:forURL:userInfo:")]
        void DidFinishWithImage (SDWebImageManager imageManager, UIImage image, NSUrl url, NSDictionary info);
        
        [Export ("webImageManager:didFinishWithImage:forURL:")]
        void DidFinishWithImage (SDWebImageManager imageManager, UIImage image, NSUrl url);
        
        [Export ("webImageManager:didFinishWithImage:")]
        void DidFinishWithImage (SDWebImageManager imageManager, UIImage image);
        
        [Export ("webImageManager:didFailWithError:forURL:userInfo:")]
        void DidFailWithError (SDWebImageManager imageManager, NSError error, NSUrl url, NSDictionary info);
        
        [Export ("webImageManager:didFailWithError:forURL:")]
        void DidFailWithError (SDWebImageManager imageManager, NSError error, NSUrl url);
        
        [Export ("webImageManager:didFailWithError:")]
        void DidFailWithError (SDWebImageManager imageManager, NSError error);
    }
    
    public delegate void SDWebImageSuccessBlock (UIImage image, bool cached);
    public delegate void SDWebImageFailureBlock (NSError error);
    
    [BaseType (typeof (NSObject))]
    interface SDWebImageManager
    {
        [Static, Export ("sharedManager")]
        SDWebImageManager SharedManager { get; }
        
        [Export ("cancelForDelegate:")]
        void CancelForDelegate (NSObject del);
        
        [Bind ("setImageWithURL:")]
        void SetImage ([Target] UIImageView view, NSUrl url);
        
        [Bind ("setImageWithURL:")]
        void SetImage ([Target] UIButton view, NSUrl url);
        
        [Bind ("setBackgroundImageWithURL:")]
        void SetBackgroundImage ([Target] UIButton view, NSUrl url);
        
        [Export ("downloadWithURL:delegate:options:")]
        void Download (NSUrl url, NSObject del, SDWebImageOptions options);
        
        [Export ("downloadWithURL:delegate:options:success:failure:")]
        void Download (NSUrl url, [NullAllowed]NSObject del, SDWebImageOptions options, SDWebImageSuccessBlock success, SDWebImageFailureBlock failure);
    }
    
    [BaseType (typeof (NSObject))]
    interface SDImageCache
    {
        [Static, Export ("setMaxCacheAge:")]
        void SetMaxCacheAge (int age);
    }
}

