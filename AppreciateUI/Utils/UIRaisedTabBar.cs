using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

namespace AppreciateUI.Utils
{
	public class UIRaisedTabBar  : UITabBarController 
	{
		UIButton centerButton;
		UIImage normalState;
		UIImage highlightedState;
		
		public event NSAction Tapped;
		
		public UIRaisedTabBar (UIImage normal, UIImage highlighted, NSAction tapped) : base ()
		{
			normalState = normal;
			highlightedState = highlighted;
			if (tapped != null)
				Tapped += tapped;
		}

		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			if (centerButton == null){
				
				centerButton = UIButton.FromType(UIButtonType.Custom);
				centerButton.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin;
				centerButton.SetImage(normalState, UIControlState.Normal);
				centerButton.SetImage(highlightedState, UIControlState.Highlighted);
				centerButton.Frame = new RectangleF(0,0,normalState.Size.Width, normalState.Size.Height);
				PointF center = TabBar.Center;
				center.Y = (TabBar.Frame.Size.Height / 2.0f);

				centerButton.Center = center;
				
				if (Tapped != null){
					centerButton.TouchUpInside += delegate(object sender, EventArgs e) {
						Tapped();
					};
				}
				this.TabBar.AddSubview(centerButton);
				this.TabBar.BringSubviewToFront(centerButton);

				Console.WriteLine(centerButton.Center);
				//this.View.AddSubview(centerButton);
				//this.View.BringSubviewToFront(centerButton);
			}
		}
		
		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);
			
			if (centerButton != null){
				this.View.BringSubviewToFront(centerButton);
			}
		}
		
	}
}

