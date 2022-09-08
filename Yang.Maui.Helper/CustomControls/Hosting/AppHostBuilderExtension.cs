using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using CpuView = Yang.Maui.Helper.CustomControls.DrawableView;
using CpuViewHandler = Yang.Maui.Helper.CustomControls.Handlers.DrawableViewHandler;
namespace Yang.Maui.Helper.CustomControls.Hosting
{
    public static class AppHostBuilderExtension
    {
        public static MauiAppBuilder UseDrawableView(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddTransient(typeof(CpuView), typeof(CpuViewHandler));
            });

            return builder;
        }
    }
}
