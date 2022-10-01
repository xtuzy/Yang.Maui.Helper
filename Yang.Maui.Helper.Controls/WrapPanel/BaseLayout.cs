using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Controls.WrapPanel
{
    using ElementView = Microsoft.Maui.Controls.View;
    public abstract class BaseLayout : Layout
    {
        internal abstract Size MauiMeasure(Size avaliableSize);

        internal abstract Size MauiLayout(Rect rect);

        public ElementView GetChildAt(int index)
        {
            return Children[index] as ElementView;
        }

        public int ChildCount { get { return (Children?.Count) ?? 0; } }

        /// <summary>
        /// Call this when something has changed which has invalidated the layout of this view. This will schedule a layout pass of the view tree. This should not be called while the view hierarchy is currently in a layout pass (isInLayout(). If layout is happening, the request may be honored at the end of the current layout pass (and then layout will run again) or after the current frame is drawn and the next layout occurs.
        /// Subclasses which override this method should call the superclass method to handle possible request-during-layout errors correctly.
        /// </summary>
        public void RefreshLayout()
        {
            this.InvalidateMeasure();
        }

        protected override ILayoutManager CreateLayoutManager()
        {
            return new WrapPanelManager(this);
        }
    }

    class WrapPanelManager : LayoutManager
    {
        public WrapPanelManager(BaseLayout layout):base(layout)
        {
        }

        public override Size ArrangeChildren(Rect bounds)
        {
            return (Layout as BaseLayout).MauiLayout(bounds);
        }

        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            return (Layout as BaseLayout).MauiMeasure(new Size(widthConstraint, heightConstraint));
        }
    }
}