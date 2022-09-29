using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using Yang.Maui.Helper.Skia.Canvas;

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
            var skCanvasView = new SKCanvasView() { WidthRequest = 500, HeightRequest = 1000 };
            container.Content = skCanvasView;
            skCanvasView.PaintSurface += (sender, e) =>
            {
                var skCanvas = e.Surface.Canvas;
                var canvas = new AndroidCanvas(skCanvas);
                canvas.DrawColor(SKColors.White);
                canvas.DrawRect(new SKRect(100, 100, 200, 200), new SKPaint() { Color = SKColors.AliceBlue });
                canvas.DrawTextAtVerticalCenter("Hello", 100, 100, 200, new SKPaint() { TextSize = 50 });
            };
        }
    }
}