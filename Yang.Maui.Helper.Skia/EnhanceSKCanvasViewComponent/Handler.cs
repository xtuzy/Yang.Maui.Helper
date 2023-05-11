using SkiaSharp.Views.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.EnhanceSKCanvasViewComponent
{
    public class EnhanceSKCanvasViewHandler : SKCanvasViewHandler
    {
        public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var custom = (this.VirtualView as EnhanceSKCanvasView).CustomMeasuredSize(widthConstraint, heightConstraint);
            if (custom == Size.Zero)
                return base.GetDesiredSize(widthConstraint, heightConstraint);
            else
                return custom;
        }
    }
}
