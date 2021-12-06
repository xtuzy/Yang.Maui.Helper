#if __ANDROID__
using Android.Graphics;

namespace Xamarin.Helper.Views
{
    /// <summary>
    /// </summary>
    public static class MaterialColorsHelperExtension
    {
        public static Color ToUIColor(this RGB rgb)
        {
            return new Color(rgb.R, rgb.G, rgb.B);
        }
        public static Color ToUIColor(this RGB rgb, int a)
        {
            return new Color(rgb.R, rgb.G, rgb.B, a);
        }
    }
}
#endif
