using Microsoft.Maui.Layouts;
using Microsoft.Maui.Platform;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public partial class TableView : ScrollView
    {
        double InitBoundsHeight;
        double InitBoundsWidth;

        protected ScrollViewContentView ContentView;
        public TableView()
        {
            InitBoundsHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            InitBoundsWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

            this.Orientation = ScrollOrientation.Vertical;
            ContentView = new ScrollViewContentView(this) { BackgroundColor = Colors.Gray };
            Content = ContentView;
            init(TableViewStyle.Plain);
            this.Scrolled += TableView_Scrolled;
        }

        private void TableView_Scrolled(object sender, ScrolledEventArgs e)
        {
            (this as IView).InvalidateMeasure();
        }

        public void AddSubview(View subview)
        {
            ContentView.Add(subview);
        }

        public void InsertSubview(View view, int index) { }

        public partial Size OnMeasure(double widthConstraint, double heightConstraint);

        public partial void OnLayoutSubviews();
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
            Console.WriteLine($"Measure Size={(Layout as ScrollViewContentView).ContentSize}");
            return size;
        }
    }
}
