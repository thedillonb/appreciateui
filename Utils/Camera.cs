//
// Camera.cs: Support code for taking pictures
//
// Copyright 2010 Miguel de Icaza
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace MobilePatterns.Utils
{
    //
    // A static class that will reuse the UIImagePickerController
    // as iPhoneOS has a crash if multiple UIImagePickerController are created
    //   http://stackoverflow.com/questions/487173
    // (Follow the links)
    //
    public class Camera : UIImagePickerController
    {
        static Camera picker;
        static Action<NSDictionary> _callback;
        static Action _canceled;
        
        static void Init ()
        {
            if (picker != null)
                return;
            
            picker = new Camera ();
        }

        protected Camera()
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            Delegate = new CameraDelegate ();
        }
        
        class CameraDelegate : UIImagePickerControllerDelegate {
            public override void FinishedPickingMedia (UIImagePickerController picker, NSDictionary info)
            {
                var cb = _callback;
                _callback = null;
                _canceled = null;
                cb (info);
            }

            public override void Canceled(UIImagePickerController picker)
            {
                var cb = _canceled;
                _callback = null;
                _canceled = null;
                if (cb != null)
                    cb();
            }

            public override void WillShowViewController (UINavigationController navigationController, UIViewController viewController, bool animated)
            {
                navigationController.NavigationBar.BarStyle = UIBarStyle.Default;
            }
        }
        
        public static void TakePicture (UIViewController parent, Action<NSDictionary> callback)
        {
            Init ();
            picker.SourceType = UIImagePickerControllerSourceType.Camera;
            _callback = callback;
            parent.PresentModalViewController (picker, true);
        }

        public static UIImagePickerController SelectPicture (UIViewController parent, Action<NSDictionary> callback, Action canceled = null)
        {
            Init ();
            picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            _callback = callback;
            _canceled = canceled;
            return picker;
        }
    }
}

