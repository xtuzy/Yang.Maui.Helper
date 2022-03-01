using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using AndroidX.ConstraintLayout.Widget;
using Yang.Maui.Helper.Devices.Screen;

namespace Yang.Maui.Helper.Platforms.Android.Views
{
    

    /// <summary>
    /// 辅助View实现轮廓为圆角.
    /// 使用方法:
    /// view.ClipToOutline = true;
    /// view.OutlineProvider = new ViewOutlineProviderHelper();
    /// 参考 <see cref="https://www.jianshu.com/p/ab42f2198776"/>
    /// </summary>
    /// <seealso cref="Android.Views.ViewOutlineProvider" />
    public class ViewBorderHelper : ViewOutlineProvider
    {
        //默认为10
        public int RoundRectRadius = 2.Dp2Px();
        public override void GetOutline(View view, Outline outline)
        {
            // 设置按钮圆角率
            outline.SetRoundRect(0, 0, view.Width, view.Height, RoundRectRadius);
            // 设置按钮为圆形
            //outline.SetOval(0, 0, view.Width, view.Height);
        }
    }

    public static class ViewBoundsHelper
    {
        /// <summary>
        /// 边框样式:圆角与阴影
        /// </summary>
        /// <param name="view"></param>
        /// <param name="radius"></param>
        /// <param name="elevation"></param>
        public static void SetBoundsStyle(this View view, int radius = 10, int elevation = 20)
        {
            view.OutlineProvider = new ViewBorderHelper() { RoundRectRadius = radius.Dp2Px() };//设置圆角
            view.ClipToOutline = true;
            view.Elevation = elevation.Dp2Px();
        }
    }

}