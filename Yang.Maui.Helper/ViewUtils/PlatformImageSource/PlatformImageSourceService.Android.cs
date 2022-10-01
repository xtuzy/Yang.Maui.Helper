using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Microsoft.Maui;
using System.Threading;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.ViewUtils.PlatformImageSource
{
    public partial class PlatformImageSourceService
    {
        public override Task<IImageSourceServiceResult<Drawable>> GetDrawableAsync(IImageSource imageSource, Context context, CancellationToken cancellationToken = default)
        {
            var image = imageSource switch
            {
                IPlatformImageSource img => img.Image as Bitmap,
                _ => null,
            };

            return image != null
                ? FromResult(new ImageSourceServiceResult(new BitmapDrawable(context.Resources, image), () => image.Dispose()))
                : FromResult(null);
        }

        private static Task<IImageSourceServiceResult<Drawable>?> FromResult(ImageSourceServiceResult? result) =>
            Task.FromResult<IImageSourceServiceResult<Drawable>?>(result);
    }
}