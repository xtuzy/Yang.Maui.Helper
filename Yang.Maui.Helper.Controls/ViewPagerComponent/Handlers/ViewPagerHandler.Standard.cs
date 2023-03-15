#if NET && !__IOS__ && !__ANDROID__ && !WINDOWS
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Controls.ViewPagerComponent.Handlers
{
    internal partial class ViewPagerHandler : ViewHandler<IViewPager, object>
    {
        protected override object CreatePlatformView() => throw new NotImplementedException();

        private static void MapViews(ViewPagerHandler arg1, IViewPager arg2)
        {
            throw new NotImplementedException();
        }

        private static void MapGoto(ViewPagerHandler arg1, IViewPager arg2, object arg3)
        {
            throw new NotImplementedException();
        }
    }
}
#endif