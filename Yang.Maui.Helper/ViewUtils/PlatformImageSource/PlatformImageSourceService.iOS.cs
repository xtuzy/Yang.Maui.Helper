using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Yang.Maui.Helper.ViewUtils.PlatformImageSource
{
    public partial class PlatformImageSourceService
    {
        public override Task<IImageSourceServiceResult<UIImage>> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
        {
            var image = imageSource switch
            {
                IPlatformImageSource img => img.Image as UIImage,
                _ => null,
            };

            return image != null
                ? FromResult(new ImageSourceServiceResult(image, () => image.Dispose()))
                : FromResult(null);
        }

        private static Task<IImageSourceServiceResult<UIImage>?> FromResult(ImageSourceServiceResult? result) =>
            Task.FromResult<IImageSourceServiceResult<UIImage>?>(result);
    }
}