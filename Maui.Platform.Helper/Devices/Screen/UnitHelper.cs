
#if __ANDROID__
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
#endif

namespace Maui.Platform.Helper.Devices.Screen
{
    /// <summary>
    /// some screen unit convert
    /// </summary>
    public static partial  class UnitHelper
    {
        public static float density = 0;

        /// <summary>
        /// density such as i,1.5,2...
        /// </summary>
        public static float Density
        {
            get
            {
                if(density == 0)
                {
#if __ANDROID__
                    throw new System.InvalidOperationException("Please load ScreenHepler.GetScreenDensity() before use Density.");
#elif WINDOWS || __IOS__ 
                    density =  ScreenHelper.Instance.GetScreenDensity();
#endif
                }
                return density;
            }
        }

        /// <summary>
        /// dp= px / density
        /// </summary>
        /// <param name="pxValus"></param>
        /// <returns></returns>
        public static int Px2Dp(this float pxValus)
        {
            return (int)(pxValus / Density + 0.5);//????????
        }

        /// <summary>
        /// dp= px / density
        /// </summary>
        /// <param name="pxValus"></param>
        /// <returns></returns>
        public static int Px2Dp(this int pxValus)
        {
            return (int)(pxValus / Density);//????????
        }

        /// <summary>
        /// px = dp * density
        /// </summary>
        /// <param name="dipValus"></param>
        /// <returns></returns>
        public static int Dp2Px(this float dipValus)
        {
            //return (int)(dipValus * Density + 0.5f);
            return (int)(dipValus * Density);
        }

        /// <summary>
        /// px = dp * density
        /// </summary>
        /// <param name="dipValus"></param>
        /// <returns></returns>
        public static int Dp2Px(this int dipValus)
        {
            //return (int)(dipValus * Density + 0.5f);
            return (int)(dipValus * Density);
        }
    }
}