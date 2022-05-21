using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.CustomControls.DrawableView
{
    public partial class DrawableViewHandler
    {
        public static IPropertyMapper<IDrawableView, DrawableViewHandler> DrawableViewMapper = new PropertyMapper<IDrawableView, DrawableViewHandler>(ViewMapper);

        public static CommandMapper<IDrawableView, DrawableViewHandler> DrawableViewCommandMapper = new CommandMapper<IDrawableView, DrawableViewHandler>(ViewCommandMapper)
        {
            [nameof(IDrawableView.Invalidate)] = MapInvalidate
        };

        public DrawableViewHandler() : base(DrawableViewMapper, DrawableViewCommandMapper)
        {

        }

        public DrawableViewHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
            : base(mapper ?? DrawableViewMapper, commandMapper ?? DrawableViewCommandMapper)
        {

        }
    }
}
