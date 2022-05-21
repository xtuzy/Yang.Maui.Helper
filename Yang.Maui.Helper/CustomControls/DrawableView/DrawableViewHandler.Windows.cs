#if WINDOWS
using Yang.Maui.Helper.CustomControls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace Yang.Maui.Helper.CustomControls.DrawableView
{
    public partial class DrawableViewHandler : ViewHandler<IDrawableView, PlatformDrawableView>
    {
        protected override PlatformDrawableView CreatePlatformView() => new PlatformDrawableView
        {
        };

        protected override void ConnectHandler(PlatformDrawableView nativeView)
        {
            base.ConnectHandler(nativeView);

            nativeView.PointerPressed += OnPointerPressed;
            nativeView.PointerMoved += OnPointerMoved;
            nativeView.PointerReleased += OnPointerReleased;
            nativeView.PointerCanceled += OnPointerCanceled;
            nativeView.PlatformDraw += OnDraw;
        }

        protected override void DisconnectHandler(PlatformDrawableView nativeView)
        {
            base.DisconnectHandler(nativeView);

            nativeView.PointerPressed -= OnPointerPressed;
            nativeView.PointerMoved -= OnPointerMoved;
            nativeView.PointerReleased -= OnPointerReleased;
            nativeView.PointerCanceled -= OnPointerCanceled;
            nativeView.PlatformDraw -= OnDraw;
        }

        public static void MapInvalidate(DrawableViewHandler handler, IDrawableView drawableView, object? arg)
        {
            handler.PlatformView?.Invalidate();
        }

        void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                var currentPoint = e.GetCurrentPoint(PlatformView);
                var currentPosition = currentPoint.Position;
                var point = new Point(currentPosition.X, currentPosition.Y);

                VirtualView?.OnTouchDown(point);
            }
            catch (Exception exc)
            {
                Debug.WriteLine("An unexpected error occured handling a touch event within the control.", exc);
            }
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                var currentPoint = e.GetCurrentPoint(PlatformView);
                var currentPosition = currentPoint.Position;
                var point = new Point(currentPosition.X, currentPosition.Y);

                VirtualView?.OnTouchMove(point);
            }
            catch (Exception exc)
            {
                Debug.WriteLine("An unexpected error occured handling a touch moved event within the control.", exc);
            }
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                var currentPoint = e.GetCurrentPoint(PlatformView);
                var currentPosition = currentPoint.Position;
                var point = new Point(currentPosition.X, currentPosition.Y);

                VirtualView?.OnTouchUp(point);
            }
            catch (Exception exc)
            {
                Debug.WriteLine("An unexpected error occured handling a touch ended event within the control.", exc);
            }
        }

        void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                var currentPoint = e.GetCurrentPoint(PlatformView);
                var currentPosition = currentPoint.Position;
                var point = new Point(currentPosition.X, currentPosition.Y);

                VirtualView?.OnTouchUp(point);
            }
            catch (Exception exc)
            {
                Debug.WriteLine("An unexpected error occured cancelling the touches within the control.", exc);
            }
        }

        private void OnDraw(object sender, PlatformDrawEventArgs e)
        {
            VirtualView?.OnDraw(sender, e);
        }
    }
}
#endif