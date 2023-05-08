using Microsoft.Maui.Layouts;
using Microsoft.Maui.Platform;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public partial class TableView : ScrollView
    {
        

        protected ScrollViewContentView ContentView;
        public TableView()
        {

            this.Orientation = ScrollOrientation.Vertical;
            ContentView = new ScrollViewContentView(this) { };
            Content = ContentView;
            Init(TableViewStyle.Plain);
            this.Scrolled += TableView_Scrolled;
        }
        double lastScrollY;
        double scrollOffset = 0;
        private void TableView_Scrolled(object sender, ScrolledEventArgs e)
        {
            scrollOffset = e.ScrollY - lastScrollY;
            lastScrollY = e.ScrollY;
            (this as IView).InvalidateMeasure();
        }

        public void AddSubview(View subview)
        {
            ContentView.Add(subview);
        }

        public void InsertSubview(View view, int index) { }

        public partial Size OnMeasure(double widthConstraint, double heightConstraint);

        public partial void OnLayoutSubviews();

        /// <summary>
        /// TableView的高度应该是有限制的, 而它的内容可以是无限高度, 因此提前在这里获取这个值.
        /// </summary>
        double initTableViewHeightConstraintWhenMeasure;
        double initTableViewWidthConstraintWhenMeasure;
        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            initTableViewWidthConstraintWhenMeasure = widthConstraint ;
            initTableViewHeightConstraintWhenMeasure = heightConstraint;
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }
    }

    public class ScrollViewContentView : Layout
    {
        internal Size ContentSize;
        TableView container;
        public ScrollViewContentView(TableView container)
        {
            this.container = container;
        }

        protected override ILayoutManager CreateLayoutManager()
        {
           return new M(this, container);
        }

        internal void SetContentSize(Size size)
        {
            ContentSize = size;
        }
    }

    class M : LayoutManager
    {
        TableView container;
        public M(Microsoft.Maui.ILayout layout, TableView container) : base(layout)
        {
            this.container = container;
        }

        public override Size ArrangeChildren(Rect bounds)
        {
            container.OnLayoutSubviews();
            return bounds.Size;
        }

        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            var size = container.OnMeasure(widthConstraint, heightConstraint);
            Console.WriteLine($"Measure Size={size}");
            return size;
        }
    }
}
