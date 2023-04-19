#if ANDROID || IOS
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.SKGpuView.Handlers
{
    public partial class SKGLViewHandler
    {
        public static PropertyMapper<SKGpuView, SKGLViewHandler> SKGLViewMapper = new PropertyMapper<SKGpuView, SKGLViewHandler>(ViewMapper)
        {
            [nameof(SKGpuView.EnableTouchEvents)] = MapEnableTouchEvents,
            [nameof(SKGpuView.HasRenderLoop)] = MapHasRenderLoop,
            [nameof(SKGpuView.EnableTransparent)] = MapEnableTransparent
        };

        public static CommandMapper<SKGpuView, SKGLViewHandler> SKGLViewCommandMapper = new CommandMapper<SKGpuView, SKGLViewHandler>(ViewCommandMapper)
        {
            [nameof(ISKGpuView.SurfaceInvalidated)] = MapSurfaceInvalidated,
            [nameof(ISKGpuView.GetCanvasSize)] = MapGetCanvasSize,
            [nameof(ISKGpuView.GetGRContext)] = MapGetGRContext,
        };

        public SKGLViewHandler() : base(SKGLViewMapper, SKGLViewCommandMapper)
        {

        }

        public SKGLViewHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
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