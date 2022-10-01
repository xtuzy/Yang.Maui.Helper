using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.ViewUtils.PlatformImageSource
{
    public partial class PlatformImageSourceService
    {
        public override Task<IImageSourceServiceResult<Microsoft.UI.Xaml.Media.ImageSource>> GetImageSourceAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
        {
            var image = imageSource switch
            {
                IPlatformImageSource img => img.Image as Microsoft.UI.Xaml.Media.ImageSource,
                _ => null,
            };

            return image != null
                ? FromResult(new ImageSourceServiceResult(image))
                : FromResult(null);
        }

        private static Task<IImageSourceServiceResult<Microsoft.UI.Xaml.Media.ImageSource>?> FromResult(ImageSourceServiceResult? result) =>
            Task.FromResult<IImageSourceServiceResult<Microsoft.UI.Xaml.Media.ImageSource>?>(result);
    }
}
