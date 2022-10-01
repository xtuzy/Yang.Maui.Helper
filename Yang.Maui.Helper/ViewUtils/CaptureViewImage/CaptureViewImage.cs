using System;
using System.IO;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.ViewUtils
{
    public partial class CaptureViewImage
    {
        public static async Task<Stream> GetImageFromViewAsync(Microsoft.Maui.Controls.View view)
        {
#if __IOS__
            var v = view.Handler.PlatformView as UIKit.UIView;
            return UIImage2Stream(GetUIImageFromView(v));
#elif ANDROID
            var v = view.Handler.PlatformView as Android.Views.View;
            MemoryStream m = new MemoryStream();
            Bitmap2Stream(GetBitmapFromView(v, v.Width, v.Height),m);
            return m;
#elif WINDOWS
            var v = view.Handler.PlatformView as Microsoft.UI.Xaml.UIElement;
            return new MemoryStream(await ImageSourceToBytesAsync(await GetImageFormViewAsync(v)));
#else
            throw new NotImplementedException(); 
#endif
        }
    }
}