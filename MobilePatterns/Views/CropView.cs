using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;

namespace MobilePatterns.Views
{
    public class CropView : UIView
    {
        private UIView _topLeft, _bottomRight;

        public CropView ()
        {
            this.BackgroundColor = UIColor.FromRGBA(0.9f, 0.9f, 0.9f, 0.2f);
            this.AutosizesSubviews = true;

            _topLeft = new MovableView();
            _topLeft.BackgroundColor = UIColor.Green;
            _topLeft.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleTopMargin;
            this.AddSubviews(_topLeft);

            _bottomRight = new MovableView();
            _bottomRight.BackgroundColor = UIColor.Green;
            _topLeft.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
            this.AddSubviews(_bottomRight);
        }

        public override void Draw (RectangleF rect)
        {
            base.Draw (rect);

            var lineWidth = 2.0f;

            var context = UIGraphics.GetCurrentContext();
            context.SaveState();
            context.SetLineWidth(lineWidth);
            context.SetLineDash(3f, new float[] { 6, 2 });
            context.SetStrokeColor(UIColor.Gray.CGColor);
            context.StrokeRect(rect);
            //context.SetFillColor(UIColor.FromRGBA(0.9f, 0.9f, 0.9f, 0.1f).CGColor);
            //context.FillRect(new RectangleF(rect.X + lineWidth, rect.Y + lineWidth, rect.Width - 2*lineWidth, rect.Height - 2*lineWidth));
            context.RestoreState();
        }

        public override void DrawRect (RectangleF area, UIViewPrintFormatter formatter)
        {
            base.DrawRect (area, formatter);
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            var f = _topLeft.Frame;
            f.Location = new PointF(0, 0);
            _topLeft.Frame = f;

            f = _bottomRight.Frame;
            f.Location = new PointF(this.Frame.Width - f.Size.Width, this.Frame.Height - f.Size.Height);
            _bottomRight.Frame = f;
        }


        private PointF _location;
        private UIView _dragView;
        public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan (touches, evt);

            //Check for touches in the movers
            if (evt.TouchesForView(_topLeft) != null)
            {
                _dragView = _topLeft;
            }
            else if (evt.TouchesForView(_bottomRight) != null)
            {
                _dragView = _bottomRight;
            }
            else if (evt.TouchesForView(this) != null)
            {
                _dragView = this;
            }

            if (_dragView != null)
                _location = ((UITouch)evt.TouchesForView(_dragView).AnyObject).LocationInView(_dragView);
        }

        public override void TouchesMoved (NSSet touches, UIEvent evt)
        {
            base.TouchesMoved (touches, evt);

            if (_dragView == null)
                return;

            var tfv = evt.TouchesForView(_dragView);
            if (tfv == null)
                return;

            var touch = (UITouch)tfv.AnyObject;
            var loc = this.Frame.Location;
            var size = this.Frame.Size;

            if (_dragView == _topLeft)
            {
                var xMovement = touch.LocationInView(_dragView).X - _location.X;
                if (loc.X + xMovement < 0)
                    xMovement = -loc.X;
                if (size.Width - xMovement < _dragView.Frame.Width * 2)
                    xMovement = size.Width - _dragView.Frame.Width * 2;

                loc.X += xMovement;
                size.Width += -(xMovement);

                var yMovement = touch.LocationInView(_dragView).Y - _location.Y;
                if (loc.Y + yMovement < 0)
                    yMovement = -loc.Y;
                if (size.Height - yMovement < _dragView.Frame.Height * 2)
                    yMovement = size.Height - _dragView.Frame.Height * 2;


                loc.Y += yMovement;
                size.Height += -(yMovement);
            }
            else if (_dragView == _bottomRight)
            {
                var xMovement = touch.LocationInView(_dragView).X - _location.X;
                if (size.Width + xMovement < _dragView.Frame.Width * 2)
                    xMovement = -size.Width + _dragView.Frame.Width * 2;
                if (loc.X + xMovement + size.Width > Superview.Bounds.Width)
                    xMovement = Superview.Bounds.Width - loc.X - size.Width;


                size.Width += xMovement;

                var yMovement = touch.LocationInView(_dragView).Y - _location.Y;
                if (size.Height + yMovement < _dragView.Frame.Height * 2)
                    yMovement = -size.Height + _dragView.Frame.Height * 2;
                if (loc.Y + yMovement + size.Height > Superview.Bounds.Height)
                    yMovement = Superview.Bounds.Height - loc.Y - size.Height;

                size.Height += yMovement;
            }
            else
            {
                var xMovement = touch.LocationInView(_dragView).X - _location.X;
                var yMovement = touch.LocationInView(_dragView).Y - _location.Y;

                if (loc.X + xMovement < 0)
                    xMovement = -loc.X;
                if (loc.Y + yMovement < 0)
                    yMovement = -loc.Y;
                if (loc.X + xMovement + size.Width > Superview.Bounds.Width)
                    xMovement = Superview.Bounds.Width - loc.X - size.Width;
                if (loc.Y + yMovement + size.Height > Superview.Bounds.Height)
                    yMovement = Superview.Bounds.Height - loc.Y - size.Height;

                loc.X += xMovement;
                loc.Y += yMovement;
            }

            this.Frame = new RectangleF(loc, size);
            this.SetNeedsDisplay();
        }

        public override void TouchesCancelled (NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled (touches, evt);
            _dragView = null;
        }


        private class MovableView : UIView
        {
            public MovableView()
                : base(new RectangleF(0, 0, 22, 22))
            {
            }
        }

    }
}

