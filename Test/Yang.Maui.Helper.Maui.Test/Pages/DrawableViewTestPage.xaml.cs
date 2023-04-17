using Microsoft.Maui.Controls.Compatibility;
using SkiaSharp;
using SkiaSharp.Views.Windows;
using System.Diagnostics;
using Yang.Maui.Helper.Controls.DrawableView;
using Yang.Maui.Helper.Controls.WrapPanel;

namespace Yang.Maui.Helper.Maui.Test.Pages;

public partial class DrawableViewTestPage : ContentPage
{
	public DrawableViewTestPage()
	{
		InitializeComponent();
        var scrollView = new ScrollView() { Orientation = ScrollOrientation.Vertical, VerticalScrollBarVisibility = ScrollBarVisibility.Always };
        var panel = new VerticalStackLayout() { Margin = new Thickness(20, 20, 20, 20) };
        scrollView.Content = panel;
        RootLayout.Children.Add(scrollView);
        var drawableView = new AutoSizeDrawableView() {  };
        panel.Children.Add(drawableView);
        drawableView.PaintSurface += DrawableView_PaintSurface;
    }

    private void DrawableView_PaintSurface(object sender, Controls.DrawableView.Platform.PlatformDrawEventArgs e)
    {
#if WINDOWS
        var wE = e.PlatformDrawArgs as Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs;
        wE.DrawingSession.Clear(SkiaSharp.Views.Windows.WindowsExtensions.ToColor(SKColors.Red));
#endif
    }

    class AutoSizeDrawableView : DrawableView
    {
        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            Debug.WriteLine($"MeasureOverride : {widthConstraint} {heightConstraint}");
            //if (!double.IsInfinity(widthConstraint))
            //    return new Size(widthConstraint/2, widthConstraint/2);
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }

        public override Size CustomMeasuredSize(double widthConstraint, double heightConstraint)
        {
            Debug.WriteLine($"CustomMeasuredSize: {widthConstraint} {heightConstraint}");
            if (!double.IsInfinity(widthConstraint))
                return new Size(widthConstraint * 0.8, widthConstraint * 3);
            return base.CustomMeasuredSize(widthConstraint, heightConstraint);
        }
    }
}