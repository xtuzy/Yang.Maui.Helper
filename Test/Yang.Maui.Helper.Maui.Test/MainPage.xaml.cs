using Android.OS;
using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using Yang.Maui.Helper.SkiaExtension;

namespace Yang.Maui.Helper.Maui.Test
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void DrawText_Clicked(object sender, EventArgs e)
        {
            var skCanvasView = new SKCanvasView() { WidthRequest= 500, HeightRequest=1000};
            container.Content = skCanvasView;
            skCanvasView.PaintSurface += (sender, e) =>
            {
                var skCanvas = e.Surface.Canvas;
                var canvas = new AndroidCanvas(skCanvas);
                canvas.DrawColor(SKColors.White);
                canvas.DrawText("Hello", 50, 50, new SKPaint());
            };
        }
    }
}