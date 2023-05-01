using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Controls.EnhanceGraphicsViewComponent
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder UseEnhanceGraphicsView(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(EnhanceGraphicsView), typeof(EnhanceGraphicsViewHandler));
            });

            return builder;
        }
    }
}
