#if __IOS__
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.SKGpuView.Handlers
{
    public partial class SKMetalViewHandler
    {
        public static PropertyMapper<SKGpuView, SKMetalViewHandler> SKGLViewMapper = new PropertyMapper<SKGpuView, SKMetalViewHandler>(ViewMapper)
        {
            [nameof(SKGpuView.EnableTouchEvents)] = MapEnableTouchEvents,
            [nameof(SKGpuView.HasRenderLoop)] = MapHasRenderLoop,
            [nameof(SKGpuView.EnableTransparent)] = MapEnableTransparent
        };

        public static CommandMapper<SKGpuView, SKMetalViewHandler> SKGLViewCommandMapper = new CommandMapper<SKGpuView, SKMetalViewHandler>(ViewCommandMapper)
        {
            [nameof(ISKGpuView.SurfaceInvalidated)] = MapSurfaceInvalidated,
            [nameof(ISKGpuView.GetCanvasSize)] = MapGetCanvasSize,
            [nameof(ISKGpuView.GetGRContext)] = MapGetGRContext,
        };

        public SKMetalViewHandler() : base(SKGLViewMapper, SKGLViewCommandMapper)
        {

        }

        public SKMetalViewHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
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