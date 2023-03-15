using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Controls.ViewPagerComponent.Handlers
{
    internal partial class ViewPagerHandler
    {
        public static IPropertyMapper<IViewPager, ViewPagerHandler> Mapper = new PropertyMapper<IViewPager, ViewPagerHandler>(ViewHandler.ViewMapper)
        {
            [nameof(IViewPager.Views)] = MapViews,
        };

        public static CommandMapper<IViewPager, ViewPagerHandler> CommandMapper = new(ViewCommandMapper)
        {
            [nameof(IViewPager.Goto)] = MapGoto
        };

        public ViewPagerHandler() : base(Mapper, CommandMapper)
        {
        }

        public ViewPagerHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }
    }
}
