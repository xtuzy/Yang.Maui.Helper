
using Xamarin.Essentials;

namespace Xamarin.Helper.Devices
{
    /// <summary>
    /// 可跨平台的单位转换
    /// </summary>
    public static partial  class UnitHelper
    {
        static float density = 0;
        /// <summary>
        /// 屏幕密度,即Dpi
        /// </summary>
        public static float Density
        {
            get
            {
                if(density == 0)
                  density =  (float)DeviceDisplay.MainDisplayInfo.Density;
                return density;
            }
        }

        /// <summary>
        /// 将px值转换成dpi或者dp值，跨平台
        /// </summary>
        /// <param name="pxValus"></param>
        /// <returns></returns>
        public static int Px2Dp(this float pxValus)
        {
            return (int)(pxValus / Density + 0.5);//往大的算
        }

        public static int Px2Dp(this int pxValus)
        {
            return (int)(pxValus / Density);//往大的算
        }

        /// <summary>
        /// 将dip和dp转化成px,跨平台
        /// </summary>
        /// <param name="dipValus"></param>
        /// <returns></returns>
        public static int Dp2Px(this float dipValus)
        {
            //return (int)(dipValus * Density + 0.5f);
            return (int)(dipValus * Density);
        }

        public static int Dp2Px(this int dipValus)
        {
            //return (int)(dipValus * Density + 0.5f);
            return (int)(dipValus * Density);
        }
    }
}