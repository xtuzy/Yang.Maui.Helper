#if ANDROID
using Microsoft.Maui.Handlers;
using SkiaSharp.Views.Maui.Platform;
using SKMauiView = Yang.Maui.Helper.Skia.SKGpuView.SKGpuView;
using SkiaSharp;
using Yang.Maui.Helper.Skia.SKGpuView.Platform;
using Microsoft.Maui.Controls;
#if ANDROID
using SKPlatformView = SkiaSharp.Views.Android.SKGLSurfaceView;
using SKNativePaintGLSurfaceEventArgs = SkiaSharp.Views.Android.SKPaintGLSurfaceEventArgs;
#else
using SKPlatformView = SkiaSharp.Views.iOS.SKGLView;
using SKNativePaintGLSurfaceEventArgs = SkiaSharp.Views.iOS.SKPaintGLSurfaceEventArgs;
#endif
namespace Yang.Maui.Helper.Skia.SKGpuView.Handlers
{
	public partial class SKGLSurfaceViewHandler : ViewHandler<SKMauiView, SKPlatformView>
	{
		private SKTouchHandler touchHandler;

        protected override SKPlatformView CreatePlatformView()
        {
            // create the native view
#if ANDROID
			var view = new SKPlatformView(Context);
#else
            var view = new SKPlatformView();
            SetDisablesUserInteraction(true);
#endif

            touchHandler = new SKTouchHandler(
                args => (VirtualView as ISKGpuView).OnTouch(args),
                (x, y) => GetScaledCoord(x, y));
            return view;
        }

		public GRContext GRContext => PlatformView?.GRContext;

#if __IOS__
		protected void SetDisablesUserInteraction(bool disablesUserInteraction)
		{
			touchHandler.DisablesUserInteraction = disablesUserInteraction;
		}
#endif
        protected override void ConnectHandler(SKPlatformView platformView)
        {
            platformView.PaintSurface += OnPaintSurface;

            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(SKPlatformView platformView)
        {
			if (platformView != null)
			{
                platformView.PaintSurface -= OnPaintSurface;
			}

			// detach, regardless of state
			touchHandler?.Detach(platformView);

			base.DisconnectHandler(platformView);

#if IOS
			Dispose();
#endif
        }

        ISKGpuView virtualView;
        private static void MapHasRenderLoop(SKGLSurfaceViewHandler handler, SKGpuView view)
        {
            handler.virtualView = view as ISKGpuView;
            handler.SetupRenderLoop(false, view);
        }

        private static void MapEnableTouchEvents(SKGLSurfaceViewHandler handler, SKGpuView view)
        {
            handler.touchHandler?.SetEnabled(handler.PlatformView, view.EnableTouchEvents);
        }

        private static void MapEnableTransparent(SKGLSurfaceViewHandler handler, SKGpuView view)
        {
            // Force the opacity to false for consistency with the other platforms
#if ANDROID
            //handler.PlatformView.SetOpaque(!view.EnableTransparent);
#else
            handler.PlatformView.Opaque = !view.EnableTransparent;
#endif

        }

        // the user asked for the current GRContext
        private static void MapGetGRContext(SKGLSurfaceViewHandler handler, SKGpuView view, object arg3)
        {
            var arg = arg3 as GetPropertyValueEventArgs<GRContext>;
			arg.Value = handler.PlatformView?.GRContext;
        }

        // the user asked for the size
        private static void MapGetCanvasSize(SKGLSurfaceViewHandler handler, SKGpuView view, object arg3)
        {
			var arg = arg3 as GetPropertyValueEventArgs<SKSize>;

            arg.Value = handler.PlatformView?.CanvasSize ?? SKSize.Empty;
        }

        // the user asked to repaint
        private static void MapSurfaceInvalidated(SKGLSurfaceViewHandler handler, SKGpuView view, object arg3)
        {
            // if we aren't in a loop, then refresh once
            if (!view.HasRenderLoop)
            {
                handler.SetupRenderLoop(true, view);
            }
        }

        protected partial void SetupRenderLoop(bool oneShot, SKGpuView view);

		private SKPoint GetScaledCoord(double x, double y)
		{
#if __ANDROID__ || __TIZEN__
			// Android and Tizen are the reverse of the other platforms
#elif __IOS__
			x = x * PlatformView.ContentScaleFactor;
			y = y * PlatformView.ContentScaleFactor;
#elif __MACOS__
			x = x * Control.Window.BackingScaleFactor;
			y = y * Control.Window.BackingScaleFactor;
#elif WINDOWS_UWP
			x = x * Control.ContentsScale;
			y = y * Control.ContentsScale;
#elif __WPF__
			// TODO: implement this if it is actually supported
			// WPF does not scale for GL as it is using Windows.Forms
#elif __GTK__
			// TODO: implement this if it is actually supported
			// GTK does not yet support IgnorePixelScaling
#else
#error Missing platform logic
#endif

			return new SKPoint((float)x, (float)y);
		}

		private void OnPaintSurface(object sender, SKNativePaintGLSurfaceEventArgs e)
		{
            // the control is being repainted, let the user know
            virtualView?.OnPaintSurface(new SKPaintGpuSurfaceEventArgs(e.Surface, e.BackendRenderTarget));
		}
	}
}
#endif