using SkiaSharp.Views.Maui.Controls.Hosting;
using Yang.Maui.Helper.Controls.DrawableView.Hosting;
using Yang.Maui.Helper.Controls.ViewPagerComponent;
using Yang.Maui.Helper.Skia.SKGpuView.Hosting;
using Yang.Maui.Helper.ViewUtils.PlatformImageSource;

namespace Yang.Maui.Helper.Maui.Test
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UsePlatformImageSource()
                .UseViewPager()
                .UseDrawableView()
                .UseSKGpuView(false, true)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }
    }
}