using System;
using MonoTouch.UIKit;

namespace MobilePatterns.Utils
{
    public static class Util
    {
        public static bool IsRetina { get { return (UIScreen.MainScreen.Scale > 1.0); } }
    }
}

