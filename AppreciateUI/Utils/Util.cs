using System;
using MonoTouch.UIKit;
using System.IO;
using System.Drawing;

namespace AppreciateUI.Utils
{
    public static class Util
    {
        public readonly static string BaseDir = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "..");

        // The size of the cache in bytes: 512MB
        public static readonly int MaxCacheSize = 1024 * 1024 * 512;

        public static bool IsRetina 
        { 
            get 
            { 
                return (UIScreen.MainScreen.Scale > 1.0); 
            } 
        }

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

        public static SizeF ThumbnailSize
        {
            get
            {
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad && Utils.Util.IsRetina)
                    return new SizeF(296, 444);
                else
                    return new SizeF(148, 222);
            }
        }

        public static SizeF IconThumbnailSize
        {
            get
            {
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad && Utils.Util.IsRetina)
                    return new SizeF(256, 256);
                else
                    return new SizeF(128, 128);
            }
        }

        public static string DownloadScreenshotUrl(string id, string ext)
        {
            SizeF size = AppreciateUI.Utils.Util.ThumbnailSize;
            return "http://www.dillonbuchanan.com/appreciateui/downloader.php?id=" + id + "&w=" + size.Width + "&h=" + size.Height + "&ext=" + ext;
        }

        public static string DownloadIconUrl(string id, string ext)
        {
            SizeF size = AppreciateUI.Utils.Util.IconThumbnailSize;
            return "http://www.dillonbuchanan.com/appreciateui/icon_downloader.php?id=" + id + "&w=" + size.Width + "&h=" + size.Height + "&ext=" + ext;
        }

        public static void LogException (string text, Exception e)
        {
            using (var s = File.AppendText (BaseDir + "/Documents/crash.log")){
                var msg = String.Format ("On {0}, message: {1}\nException:\n{2}", DateTime.Now, text, e);
                s.WriteLine (msg);
                Console.WriteLine (msg);
            }
        }
    }
}

