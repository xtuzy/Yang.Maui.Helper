using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.EnhanceSKCanvasViewComponent
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder UseEnhanceSKCanvasView(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(EnhanceSKCanvasView), typeof(EnhanceSKCanvasViewHandler));
            });

            return builder;
        }
    }
}
