using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libCollectionViewUniversal.a", LinkTarget.Simulator | LinkTarget.ArmV7, ForceLoad = true, Frameworks="Foundation")]
