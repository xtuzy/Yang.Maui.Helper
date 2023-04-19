#if WINDOWS
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.SKGpuView.Handlers
{
    public partial class SKXamlCanvasViewHandler
    {
        public static PropertyMapper<SKGpuView, SKXamlCanvasViewHandler> SKGLViewMapper = new PropertyMapper<SKGpuView, SKXamlCanvasViewHandler>(ViewMapper)
        {
            [nameof(SKGpuView.EnableTouchEvents)] = MapEnableTouchEvents,
            //[nameof(SKGpuView.HasRenderLoop)] = MapHasRenderLoop,
            //[nameof(SKGpuView.EnableTransparent)] = MapEnableTransparent
        };

        public static CommandMapper<SKGpuView, SKXamlCanvasViewHandler> SKGLViewCommandMapper = new CommandMapper<SKGpuView, SKXamlCanvasViewHandler>(ViewCommandMapper)
        {
            [nameof(ISKGpuView.SurfaceInvalidated)] = MapSurfaceInvalidated,
            [nameof(ISKGpuView.GetCanvasSize)] = MapGetCanvasSize,
            //[nameof(ISKGLView.GetGRContext)] = MapGetGRContext,
        };

        public SKXamlCanvasViewHandler() : base(SKGLViewMapper, SKGLViewCommandMapper)
        {

        }

        public SKXamlCanvasViewHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
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