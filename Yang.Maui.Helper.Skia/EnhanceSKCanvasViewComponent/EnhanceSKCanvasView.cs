using SkiaSharp.Views.Maui.Controls;

namespace Yang.Maui.Helper.Skia.EnhanceSKCanvasViewComponent
{
    public class EnhanceSKCanvasView: SKCanvasView
    {
        public EnhanceSKCanvasView()
        {
        }

        public virtual Size CustomMeasuredSize(double widthConstraint, double heightConstraint)
        {
            return Size.Zero;
        }
    }
}