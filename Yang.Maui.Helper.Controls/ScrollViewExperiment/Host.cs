using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Controls.ScrollViewExperiment
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder UseTableView(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler(typeof(TableView), typeof(MyScrollViewHandler));
#endif
                });

            return builder;
        }
    }
}
