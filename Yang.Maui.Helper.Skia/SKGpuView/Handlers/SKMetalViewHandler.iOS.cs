#if __IOS__
using CoreAnimation;
using Foundation;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.SKGpuView.Handlers
{
    public partial class SKMetalViewHandler : IDisposable
    {
        private CADisplayLink displayLink;

        protected partial void SetupRenderLoop(bool oneShot, SKGpuView view)
        {
            // only start if we haven't already
            if (displayLink != null)
                return;

            // bail out if we are requesting something that the view doesn't want to
            if (!oneShot && !view.HasRenderLoop)
                return;

            // if this is a one shot request, don't bother with the display link
            if (oneShot)
            {
                var nativeView = PlatformView;
                nativeView?.BeginInvokeOnMainThread(() =>
                {
                    if (nativeView.Handle != IntPtr.Zero)
                        nativeView.SetNeedsDisplay();
                });
                return;
            }

            // create the loop
            displayLink = CADisplayLink.Create(() =>
            {
                var nativeView = PlatformView;
                var formsView = view;

                // stop the render loop if this was a one-shot, or the views are disposed
                if (nativeView == null || formsView == null || nativeView.Handle == IntPtr.Zero || !formsView.HasRenderLoop)
                {
                    displayLink.Invalidate();
                    displayLink.Dispose();
                    displayLink = null;
                    return;
                }

                // redraw the view
                nativeView.SetNeedsDisplay();
            });
            displayLink.AddToRunLoop(NSRunLoop.Current, NSRunLoopMode.Default);
        }

        public void Dispose()
        {
            // stop the render loop
            if (displayLink != null)
            {
                displayLink.Invalidate();
                displayLink.Dispose();
                displayLink = null;
            }
        }
    }
}
#endif