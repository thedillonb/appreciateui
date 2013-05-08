using System;
using MonoTouch.UIKit;

namespace MobilePatterns.Utils
{
    public static class Util
    {
        public static bool IsRetina { get { return (UIScreen.MainScreen.Scale > 1.0); } }

        public static Tuple<int, int> iOSVersion
        {
            get
            {
                try
                {
                    var version = UIDevice.CurrentDevice.SystemVersion.Split('.');
                    var major = Int32.Parse(version[0]);
                    var minor = Int32.Parse(version[1]);
                    return new Tuple<int, int>(major, minor);
                }
                catch (Exception e)
                {
                    Console.WriteLine("When attempting to get version: " + e.Message);
                    return new Tuple<int, int>(5, 0);
                }
            }
        }
    }
}

