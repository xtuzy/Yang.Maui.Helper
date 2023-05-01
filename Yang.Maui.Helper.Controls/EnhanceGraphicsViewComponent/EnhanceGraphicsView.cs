
namespace Yang.Maui.Helper.Controls.EnhanceGraphicsViewComponent
{
    public class EnhanceGraphicsView : GraphicsView, IView
    {
        public EnhanceGraphicsView()
        {
        }

        public virtual Size CustomMeasuredSize(double widthConstraint, double heightConstraint)
        {
            return Size.Zero;
        }

        Size IView.Measure(double widthConstraint, double heightConstraint) 
        {
            return MeasureOverride(widthConstraint, heightConstraint);
        }
    }
}