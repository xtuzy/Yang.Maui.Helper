#if ANDROID
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.SKGpuView.Handlers
{
    public partial class SKGLSurfaceViewHandler
    {
        public static PropertyMapper<SKGpuView, SKGLSurfaceViewHandler> SKGLViewMapper = new PropertyMapper<SKGpuView, SKGLSurfaceViewHandler>(ViewMapper)
        {
            [nameof(SKGpuView.EnableTouchEvents)] = MapEnableTouchEvents,
            [nameof(SKGpuView.HasRenderLoop)] = MapHasRenderLoop,
            [nameof(SKGpuView.EnableTransparent)] = MapEnableTransparent
        };

        public static CommandMapper<SKGpuView, SKGLSurfaceViewHandler> SKGLViewCommandMapper = new CommandMapper<SKGpuView, SKGLSurfaceViewHandler>(ViewCommandMapper)
        {
            [nameof(ISKGpuView.SurfaceInvalidated)] = MapSurfaceInvalidated,
            [nameof(ISKGpuView.GetCanvasSize)] = MapGetCanvasSize,
            [nameof(ISKGpuView.GetGRContext)] = MapGetGRContext,
        };

        public SKGLSurfaceViewHandler() : base(SKGLViewMapper, SKGLViewCommandMapper)
        {

        }

        public SKGLSurfaceViewHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
            : base(mapper ?? SKGLViewMapper, commandMapper ?? SKGLViewCommandMapper)
        {

        }

        public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var custom = (this.VirtualView as SKGpuView).CustomMeasuredSize(widthConstraint, heightConstraint);
            if (custom == Size.Zero)
                return base.GetDesiredSize(widthConstraint, heightConstraint);
            else
                return custom;
        }
    }
}
#endif