using SkiaSharp.Views.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.SKGpuView.Hosting
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder UseSKGpuView(this MauiAppBuilder builder, bool useAndroidSurfaceView = false, bool useiOSMetalView = false)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
#if IOS
                if (useiOSMetalView)
                    handlers.AddHandler(typeof(SKGpuView), typeof(Yang.Maui.Helper.Skia.SKGpuView.Handlers.SKMetalViewHandler));
                else
                    handlers.AddHandler(typeof(SKGpuView), typeof(Yang.Maui.Helper.Skia.SKGpuView.Handlers.SKGLViewHandler));
#elif ANDROID
                if(useAndroidSurfaceView)
                    handlers.AddHandler(typeof(SKGpuView), typeof(Yang.Maui.Helper.Skia.SKGpuView.Handlers.SKGLSurfaceViewHandler));
                else
                    handlers.AddHandler(typeof(SKGpuView), typeof(Yang.Maui.Helper.Skia.SKGpuView.Handlers.SKGLViewHandler));
#elif MACCATALYST
                    handlers.AddHandler(typeof(SKGpuView), typeof(Yang.Maui.Helper.Skia.SKGpuView.Handlers.SKMetalViewHandler));
#elif WINDOWS
                    handlers.AddHandler(typeof(SKGpuView), typeof(Yang.Maui.Helper.Skia.SKGpuView.Handlers.SKXamlCanvasViewHandler));
#endif
            });

            return builder;
        }
    }
}
