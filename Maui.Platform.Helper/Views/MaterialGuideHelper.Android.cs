#if __ANDROID__
namespace Maui.Platform.Helper.Views
{
    public static class MaterialGuideHelper
    {
        /// <summary>
        /// Sets the device 是手机还是平板,他们的规范有些不同
        /// </summary>
        /// <param name="isphone">if set to <c>true</c> [isphone].</param>
        public static void SetDevice(bool isPhone = true)
        {
            if (isPhone)
            {
                /* TopNavigationBarHeight = 56.Dip2Px();
                 TopNavigationBarTitleTextSize = 20;
                 TopNavigationBarIconSize = 24.Dip2Px();
                 BottomTabBarIconSize = 24.Dip2Px();
                 BottomTabBarIconTopMarginWhenSelected = 8.Dip2Px();
                 BottomTabBarTextSize = 12;*/
            }
            else
            {

            }
        }

        /// <summary>
        /// The navigation bar height,px
        /// </summary>
        public static int TopNavigationBarHeight;
        /// <summary>
        /// The top navigation bar title text size.pt
        /// </summary>
        public static int TopNavigationBarTitleTextSize;
        private static int TopNavigationBarIconSize;
        private static int BottomTabBarIconSize;
        private static int BottomTabBarIconTopMarginWhenSelected;
        /// <summary>
        /// The bottom tab bar text size.pt
        /// </summary>
        private static int BottomTabBarTextSize;
    }
}
#endif