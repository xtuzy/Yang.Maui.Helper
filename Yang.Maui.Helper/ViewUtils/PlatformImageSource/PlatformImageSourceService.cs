using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.ViewUtils.PlatformImageSource
{
    public partial class PlatformImageSourceService: ImageSourceService, IImageSourceService<IPlatformImageSource>
    {
        public PlatformImageSourceService()
            : this(null)
        {
        }
        public PlatformImageSourceService(ILogger? logger)
            : base(logger)
        {
        }
    }
}
