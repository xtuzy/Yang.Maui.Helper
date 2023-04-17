using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using CpuView = Yang.Maui.Helper.Controls.DrawableView.IDrawableView;
using CpuViewHandler = Yang.Maui.Helper.Controls.DrawableView.Handlers.DrawableViewHandler;
namespace Yang.Maui.Helper.Controls.DrawableView.Hosting
{
    public static class AppHostBuilderExtension
    {
        public static MauiAppBuilder UseDrawableView(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(CpuView), typeof(CpuViewHandler));
            });

            return builder;
        }
    }
}
