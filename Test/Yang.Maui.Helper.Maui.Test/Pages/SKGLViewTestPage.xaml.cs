using SkiaSharp;
using SkiaSharp.Views.Maui;
using Yang.Maui.Helper.Skia.SKGpuView;

namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class SKGLViewTestPage : ContentPage
{
	public SKGLViewTestPage()
	{
		InitializeComponent();
		var glView = new SkiaSharp.Views.Maui.Controls.SKGLView() {
            HeightRequest = 300, 
            WidthRequest = 300, 
            //EnableTransparent = true, 
            //HasRenderLoop = true 
        };

        grid.Add(glView);
        glView.PaintSurface += GlView_PaintSurface;
        RefreshButton.Clicked += (sender, e) => 
        {
            glView.InvalidateSurface();
        };
        
        var gpuView = new SKGpuView()
        {
            HeightRequest = 300,
            WidthRequest = 300,
            EnableTransparent = true, 
            //HasRenderLoop = true 
        };

        rootLayout.Add(gpuView);
        gpuView.PaintSurface += GpuView_PaintSurface;
    }

    private void GpuView_PaintSurface(object sender, SKPaintGpuSurfaceEventArgs e)
    {
        //var size = (sender as SKGpuView).CanvasSize;
        var density = DeviceDisplay.Current.MainDisplayInfo.Density;
        //var viewSize = (sender as SKGpuView).Width;
        e.Surface.Canvas.Clear(SKColors.Red.WithAlpha(200));
    }

    private void GlView_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        //var size = (sender as SKGpuView).CanvasSize;
        var density = DeviceDisplay.Current.MainDisplayInfo.Density;
        //var viewSize = (sender as SKGpuView).Width;
        e.Surface.Canvas.Clear(SKColors.Red.WithAlpha(200));
    }
}