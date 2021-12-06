using static Android.Resource;
namespace Xamarin.Helper.Actions
{


    /// <summary>
    /// 定义在resources/values/style里,其对进入和退出组合了resources/anim中的动画
    /// </summary>
    public class AnimAction
    {
        /** 默认动画效果 */
        public static int ANIM_DEFAULT = -1;

        /** 没有动画效果 */
        public static int ANIM_EMPTY = 0;

        /** 缩放动画 */
        public static int ANIM_SCALE = Resource.Style.ScaleAnimStyle;

        /** IOS 动画 */
        public static int ANIM_IOS = Resource.Style.IOSAnimStyle;

        /** 吐司动画 */
        public static int ANIM_TOAST = Style.AnimationToast;

        /** 顶部弹出动画 */
        public static int ANIM_TOP = Resource.Style.TopAnimStyle;

        /** 底部弹出动画 */
        public static int ANIM_BOTTOM = Resource.Style.BottomAnimStyle;

        /** 左边弹出动画 */
        public static int ANIM_LEFT = Resource.Style.LeftAnimStyle;

        /** 右边弹出动画 */
        public static int ANIM_RIGHT = Resource.Style.RightAnimStyle;
    }
}