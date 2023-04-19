using SkiaSharp;
using SkiaSharp.Views.Maui;
using Yang.Maui.Helper.Skia.SKGpuView;

namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class SKGLViewTestPage : ContentPage
{
	public SKGLViewTestPage()
	{
		InitializeComponent();
		var glView = new SKGpuView() { HeightRequest = 300, WidthRequest = 300, EnableTransparent = false };

        grid.Add(glView);
        glView.PaintSurface += GlView_PaintSurface;
        RefreshButton.Clicked += (sender, e) => 
        {
            glView.InvalidateSurface();
        };
        grid.Add(new Label() { Text = "Hello", FontSize = 40, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center });
        rootLayout.Add(new SkiaTextDrawDemo());
    }

    private void GlView_PaintSurface(object sender, SKPaintGpuSurfaceEventArgs e)
    {
        e.Surface.Canvas.Clear(SKColors.Red.WithAlpha(200));
    }
}