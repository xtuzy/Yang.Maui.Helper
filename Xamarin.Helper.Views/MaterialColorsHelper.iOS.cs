#if __IOS__
using UIKit;

namespace Xamarin.Helper.Views
{
    /// <summary>
    /// </summary>
    public static class MaterialColorsHelperExtension
    {

        public static UIColor ToUIColor(this RGB rgb)
        {
            return UIColor.FromRGB(rgb.R, rgb.G, rgb.B);
        }

    }
}
#endif