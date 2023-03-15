using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yang.Maui.Helper.Controls.ViewPagerComponent.Handlers;

namespace Yang.Maui.Helper.Controls.ViewPagerComponent
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder UseViewPager(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(IViewPager), typeof(ViewPagerHandler));
            });

            return builder;
        }
    }
}
