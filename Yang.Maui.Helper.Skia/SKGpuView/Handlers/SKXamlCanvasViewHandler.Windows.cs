using Microsoft.Maui.Handlers;
using SkiaSharp;
using SkiaSharp.Views.Maui.Platform;
using SkiaSharp.Views.Windows;
using Yang.Maui.Helper.Skia.SKGpuView.Platform;

namespace Yang.Maui.Helper.Skia.SKGpuView.Handlers
{
    public partial class SKXamlCanvasViewHandler : ViewHandler<SKGpuView, SKXamlCanvas>
    {
        private SKSizeI lastCanvasSize;
        private SKTouchHandler? touchHandler;

        protected override SKXamlCanvas CreatePlatformView() => new SKXamlCanvas();

        protected override void ConnectHandler(SKXamlCanvas platformView)
        {
            platformView.PaintSurface += OnPaintSurface;

            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(SKXamlCanvas platformView)
        {
            touchHandler?.Detach(platformView);
            touchHandler = null;

            platformView.PaintSurface -= OnPaintSurface;

            base.DisconnectHandler(platformView);
        }

        // Mapper actions / properties

        public static void MapSurfaceInvalidated(SKXamlCanvasViewHandler handler, SKGpuView canvasView, object? args)
        {
            handler.PlatformView?.Invalidate();
        }

        public static void MapIgnorePixelScaling(SKXamlCanvasViewHandler handler, SKGpuView canvasView)
        {
            //handler.PlatformView?.UpdateIgnorePixelScaling(canvasView);
        }

        public static void MapEnableTouchEvents(SKXamlCanvasViewHandler handler, SKGpuView canvasView)
        {
            if (handler.PlatformView == null)
                return;

            handler.touchHandler ??= new SKTouchHandler(
                args => canvasView.OnTouch(args),
                (x, y) => handler.OnGetScaledCoord(x, y));

            handler.touchHandler?.SetEnabled(handler.PlatformView, canvasView.EnableTouchEvents);
        }

        // the user asked for the size
        private static void MapGetCanvasSize(SKXamlCanvasViewHandler handler, SKGpuView view, object arg3)
        {
            var arg = arg3 as GetPropertyValueEventArgs<SKSize>;

            arg.Value = handler.PlatformView?.CanvasSize ?? SKSize.Empty;
        }

        // helper methods

        private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            var newCanvasSize = e.Info.Size;
            if (lastCanvasSize != newCanvasSize)
            {
                lastCanvasSize = newCanvasSize;
                //VirtualView?.OnCanvasSizeChanged(newCanvasSize);
            }

            (VirtualView as ISKGpuView)?.OnPaintSurface(new SKPaintGpuSurfaceEventArgs(e.Surface, null));
        }

        private SKPoint OnGetScaledCoord(double x, double y)
        {
            //if (VirtualView?.IgnorePixelScaling == false && PlatformView != null)
            {
                var scale = PlatformView.Dpi;

                x *= scale;
                y *= scale;
            }

            return new SKPoint((float)x, (float)y);
        }
    }
}
