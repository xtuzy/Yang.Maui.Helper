﻿//using SkiaSharp.Views.Maui.Controls.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Yang.Maui.Helper.Controls.DrawableView.Hosting;
using Yang.Maui.Helper.Controls.EnhanceGraphicsViewComponent;
using Yang.Maui.Helper.Controls.ViewPagerComponent;
using Yang.Maui.Helper.Skia.SKGpuView.Hosting;
using Yang.Maui.Helper.Skia.EnhanceSKCanvasViewComponent;
using Yang.Maui.Helper.ViewUtils.PlatformImageSource;
using Yang.Maui.Helper.Controls.ScrollViewExperiment;

namespace Yang.Maui.Helper.Maui.Test
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp(true)
                .UsePlatformImageSource()
                .UseViewPager()
                .UseDrawableView()
                .UseEnhanceGraphicsView()
                .UseEnhanceSKCanvasView()
                .UseSKGpuView(false, true)
                .UseTableView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("YouYuan.ttf", "YouYuan");
                    fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FontAwesome6FreeRegular400");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontAwesome6FreeSolid900");
                });

            return builder.Build();
        }
    }
}