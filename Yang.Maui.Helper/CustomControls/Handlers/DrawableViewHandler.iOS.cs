#if __IOS__ || __MACCATALYST__
using Foundation;
using Yang.Maui.Helper.CustomControls.Platform;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Microsoft.Maui.Graphics;

namespace Yang.Maui.Helper.CustomControls.Handlers
{
    public class TouchEventArgs : EventArgs
    {
        public TouchEventArgs(Point point)
        {
            Point = point;
        }

        public Point Point { get; set; }
    }

    public class TouchNativeDrawableView : PlatformDrawableView
    {
        public event EventHandler<TouchEventArgs>? TouchDown;
        public event EventHandler<TouchEventArgs>? TouchMove;
        public event EventHandler<TouchEventArgs>? TouchUp;

        public override void TouchesBegan(NSSet touches, UIEvent? evt)
        {
            base.TouchesBegan(touches, evt);

            var viewPoints = this.GetPointsInView(evt);
            PointF viewPoint = viewPoints.Length > 0 ? viewPoints[0] : PointF.Zero;
            var point = new Point(viewPoint.X, viewPoint.Y);

            TouchDown?.Invoke(this, new TouchEventArgs(point));
        }

        public override void TouchesMoved(NSSet touches, UIEvent? evt)
        {
            base.TouchesMoved(touches, evt);

            var viewPoints = this.GetPointsInView(evt);
            PointF viewPoint = viewPoints.Length > 0 ? viewPoints[0] : PointF.Zero;
            var point = new Point(viewPoint.X, viewPoint.Y);

            TouchMove?.Invoke(this, new TouchEventArgs(point));
        }

        public override void TouchesEnded(NSSet touches, UIEvent? evt)
        {
            base.TouchesEnded(touches, evt);

            var viewPoints = this.GetPointsInView(evt);
            PointF viewPoint = viewPoints.Length > 0 ? viewPoints[0] : PointF.Zero;
            var point = new Point(viewPoint.X, viewPoint.Y);

            TouchUp?.Invoke(this, new TouchEventArgs(point));
        }

        public override void TouchesCancelled(NSSet touches, UIEvent? evt)
        {
            base.TouchesCancelled(touches, evt);

            var viewPoints = this.GetPointsInView(evt);
            PointF viewPoint = viewPoints.Length > 0 ? viewPoints[0] : PointF.Zero;
            var point = new Point(viewPoint.X, viewPoint.Y);

            TouchUp?.Invoke(this, new TouchEventArgs(point));
        }
    }

    public partial class DrawableViewHandler : ViewHandler<IDrawableView, TouchNativeDrawableView>
    {
        const NSKeyValueObservingOptions observingOptions = NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew | NSKeyValueObservingOptions.Prior;

        IDisposable? _isLoadedObserverDisposable;

        protected override TouchNativeDrawableView CreatePlatformView()
        {
            var nativeDrawableView = new TouchNativeDrawableView
            {
                UserInteractionEnabled = true,
                BackgroundColor = UIColor.Clear,
            };

            return nativeDrawableView;
        }

        protected override void ConnectHandler(TouchNativeDrawableView nativeView)
        {
            base.ConnectHandler(nativeView);

            var key = nativeView.Superview == null ? "subviews" : "superview";
            _isLoadedObserverDisposable = nativeView.AddObserver(key, observingOptions, OnViewLoadedObserver);

            nativeView.TouchDown += OnTouchDown;
            nativeView.TouchMove += OnTouchMove;
            nativeView.TouchUp += OnTouchUp;
            nativeView.PlatformDraw += OnDraw;
        }

        protected override void DisconnectHandler(TouchNativeDrawableView nativeView)
        {
            base.DisconnectHandler(nativeView);

            _isLoadedObserverDisposable?.Dispose();
            _isLoadedObserverDisposable = null;

            nativeView.TouchDown -= OnTouchDown;
            nativeView.TouchMove -= OnTouchMove;
            nativeView.TouchUp -= OnTouchUp;
            nativeView.PlatformDraw -= OnDraw;
        }

        public static void MapInvalidate(DrawableViewHandler handler, IDrawableView drawableView, object? arg)
        {
            handler.PlatformView?.SetNeedsDisplay();
        }

        void OnViewLoadedObserver(NSObservedChange nSObservedChange)
        {
            if (!nSObservedChange?.NewValue?.Equals(NSNull.Null) ?? false)
            {
                VirtualView?.Load();
            }
            else if (!nSObservedChange?.OldValue?.Equals(NSNull.Null) ?? false)
            {
                VirtualView?.Unload();

                _isLoadedObserverDisposable?.Dispose();
                _isLoadedObserverDisposable = null;
            }
        }

        void OnTouchDown(object? sender, TouchEventArgs e)
        {
            VirtualView?.OnTouchDown(e.Point);
        }

        void OnTouchMove(object? sender, TouchEventArgs e)
        {
            VirtualView?.OnTouchMove(e.Point);
        }

        void OnTouchUp(object? sender, TouchEventArgs e)
        {
            VirtualView?.OnTouchUp(e.Point);
        }

        private void OnDraw(object sender, PlatformDrawEventArgs e)
        {
            VirtualView?.OnDraw(sender, e);
        }
    }

}
#endif