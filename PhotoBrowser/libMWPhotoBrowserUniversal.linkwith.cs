using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libMWPhotoBrowserUniversal.a", LinkTarget.Simulator | LinkTarget.ArmV7, ForceLoad = true, Frameworks="ImageIO MessageUI Foundation")]
