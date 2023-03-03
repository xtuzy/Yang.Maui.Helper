using Android.Views;

namespace Yang.Maui.Helper.Touch
{
    /// <summary>
    /// 参考https://www.jianshu.com/p/e3730b0906cc
    /// </summary>
    public class PerformTouch
    {
        public static void Click(View view, bool longClick = false)
        {
            if (longClick)
            {
                view.PerformLongClick();
            }
            else
                view.PerformClick();
        }

        /// <summary>
        /// 使用<see cref="MotionEvent.Obtain"/>生成事件
        /// </summary>
        /// <param name="view"></param>
        /// <param name="motionEvent"></param>
        public static void Touch(View view, MotionEvent motionEvent)
        {
            view.DispatchTouchEvent(motionEvent);
        }
    }
}
