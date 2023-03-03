using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Image
{
    public interface IPlatformImageImageSource : IImageSource
    {
#if IOS || MACCATALYST
        UIKit.UIImage PlatformImage { get; }
#elif WINDOWS
        Microsoft.UI.Xaml.Media.ImageSource PlatformImage { get; }
#elif ANDROID
        Android.Graphics.Bitmap PlatformImage { get; }
#else
#endif
    }

    public partial class PlatformImageSourceService : ImageSourceService, IImageSourceService,
        IImageSourceService<IPlatformImageImageSource>
    {
        public PlatformImageSourceService()
            : this(null)
        {
        }
        public PlatformImageSourceService(ILogger? logger)
            : base(logger)
        {
        }

#if ANDROID
        public override Task<IImageSourceServiceResult<Android.Graphics.Drawables.Drawable>?> GetDrawableAsync(IImageSource imageSource, Android.Content.Context context, CancellationToken cancellationToken = default)
        {
            var bitmap = imageSource switch
            {
                IPlatformImageImageSource img => img.PlatformImage,
                _ => null,
            };

            return bitmap != null
                ? FromResult(new ImageSourceServiceResult(new Android.Graphics.Drawables.BitmapDrawable(context.Resources, bitmap), () => bitmap.Dispose()))
                : FromResult(null);
        }

        private static Task<IImageSourceServiceResult<Android.Graphics.Drawables.Drawable>?> FromResult(ImageSourceServiceResult? result) =>
            Task.FromResult<IImageSourceServiceResult<Android.Graphics.Drawables.Drawable>?>(result);
#elif IOS || MACCATALYST
        public override Task<IImageSourceServiceResult<UIKit.UIImage>?> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
        {
            var image = imageSource switch
            {
                IPlatformImageImageSource img => img.PlatformImage,
                _ => null,
            };

            return image != null
                ? FromResult(new ImageSourceServiceResult(image, () => image.Dispose()))
                : FromResult(null);
        }

        private static Task<IImageSourceServiceResult<UIKit.UIImage>?> FromResult(ImageSourceServiceResult? result) =>
            Task.FromResult<IImageSourceServiceResult<UIKit.UIImage>?>(result);
#elif WINDOWS
        public override Task<IImageSourceServiceResult<Microsoft.UI.Xaml.Media.ImageSource>?> GetImageSourceAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
        {
            var bitmap = imageSource switch
            {
                IPlatformImageImageSource img => img.PlatformImage,
                _ => null,
            };

            return bitmap != null
                ? FromResult(new ImageSourceServiceResult(bitmap))
                : FromResult(null);
        }

        private static Task<IImageSourceServiceResult<Microsoft.UI.Xaml.Media.ImageSource>?> FromResult(ImageSourceServiceResult? result) =>
            Task.FromResult<IImageSourceServiceResult<Microsoft.UI.Xaml.Media.ImageSource>?>(result);
#else
#endif
    }

    public sealed partial class PlatformImageImageSource : ImageSource, IPlatformImageImageSource
    {
        public static readonly BindableProperty PlatformImageProperty =
#if IOS || MACCATALYST
            BindableProperty.Create(nameof(PlatformImage), typeof(UIKit.UIImage), typeof(PlatformImageImageSource), default(UIKit.UIImage));
#elif WINDOWS
            BindableProperty.Create(nameof(PlatformImage), typeof(Microsoft.UI.Xaml.Media.ImageSource), typeof(PlatformImageImageSource), default(Microsoft.UI.Xaml.Media.ImageSource));
#elif ANDROID
            BindableProperty.Create(nameof(PlatformImage), typeof(Android.Graphics.Bitmap), typeof(PlatformImageImageSource), default(Android.Graphics.Bitmap));
#else
            BindableProperty.Create(nameof(PlatformImage), typeof(object), typeof(PlatformImageImageSource), default(object));
#endif
#if IOS || MACCATALYST
        public UIKit.UIImage PlatformImage
        {
            get { return (UIKit.UIImage)GetValue(PlatformImageProperty); }
            set { SetValue(PlatformImageProperty, value); }
        }

#elif WINDOWS
        public Microsoft.UI.Xaml.Media.ImageSource PlatformImage
                {
            get { return (Microsoft.UI.Xaml.Media.ImageSource)GetValue(PlatformImageProperty); }
            set { SetValue(PlatformImageProperty, value); }
        }

#elif ANDROID
        public Android.Graphics.Bitmap PlatformImage
        {
            get { return (Android.Graphics.Bitmap)GetValue(PlatformImageProperty); }
            set { SetValue(PlatformImageProperty, value); }
        }

#else
        public object PlatformImage
        {
            get { return (object)GetValue(PlatformImageProperty); }
            set { SetValue(PlatformImageProperty, value); }
        }
#endif
        public override Task<bool> Cancel()
        {
            return Task.FromResult(false);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == PlatformImageProperty.PropertyName)
                OnSourceChanged();
            base.OnPropertyChanged(propertyName);
        }
    }


    public static class AppHostBuilderExtensions
    {
        public static MauiAppBuilder UsePlatformImageSource(this MauiAppBuilder builder) =>
            builder
                .ConfigureImageSources(sources =>
                {
                    sources.AddService<IPlatformImageImageSource, PlatformImageSourceService>();
                });
    }
}
