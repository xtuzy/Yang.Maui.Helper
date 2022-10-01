using Microsoft.Maui.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.ViewUtils.PlatformImageSource
{
    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder UsePlatformImageSource(this MauiAppBuilder builder) =>
             builder
                 .ConfigureImageSources(sources =>
                 {
                     sources.AddService<IPlatformImageSource, PlatformImageSourceService>();
                 });
    }
}