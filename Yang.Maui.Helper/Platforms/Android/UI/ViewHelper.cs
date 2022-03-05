using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Attribute = Android.Resource.Attribute;
using AndroidX.ConstraintLayout.Widget;
using Yang.Maui.Helper.Devices.Screen;
using Android.Content.Res;
using Android.Graphics.Drawables;

namespace Yang.Maui.Helper.Platforms.Android.UI
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

    public class ViewStateHelper
    {

        /// <summary>
        /// <see href="https://stackoverflow.com/questions/32226232/android-create-selector-programmatically">Android create selector programmatically?</see><br/>
        /// At Android, usually use xml to create Selector to according to state set icon, this method let you can easily use code to set.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static StateListDrawable CreateStateListDrawable((State[], Drawable)[] stateListDrawables)
        {
            StateListDrawable res = new StateListDrawable();
            res.SetExitFadeDuration(400);
            res.SetAlpha(45);
            foreach (var statelist in stateListDrawables)
            {
                res.AddState(statelist.Item1.Select(e => (int)e).ToArray(), statelist.Item2);
            }
            //res.AddState(new int[] { }, new Android.Graphics.Drawables.ColorDrawable(Color.Transparent));
            return res;
        }

        /// <summary>
        /// Some View State For StateListDrawable And ColorStateList.
        /// https://developer.android.com/reference/android/graphics/drawable/StateListDrawable
        /// </summary>
        public enum State
        {
            //AboveAnchor = Android.Resource.Attribute.StateAboveAnchor,

            //Accelerated = Android.Resource.Attribute.StateAccelerated,

            /// <summary>
            /// State value for StateListDrawable, set when a view or its parent has been "activated" meaning the user has currently marked it as being of interest. 
            /// </summary>
            Activated = Attribute.StateActivated,

            /// <summary>
            /// State value for StateListDrawable, set when a view or drawable is considered "active" by its host. 
            /// </summary>
            Active = Attribute.StateActive,

            /// <summary>
            /// State identifier indicating that the object may display a check mark. 
            /// </summary>
            Checkable = Attribute.StateCheckable,

            /// <summary>
            /// State identifier indicating that the object is currently checked. 
            /// </summary>
            Checked = Attribute.StateChecked,

            //DragCanAccept = Android.Resource.Attribute.StateDragCanAccept,

            //DragHovered = Android.Resource.Attribute.StateDragHovered,

            //Empty = Android.Resource.Attribute.StateEmpty,

            /// <summary>
            /// State value for StateListDrawable, set when a view is enabled. 
            /// </summary>
            Enabled = Attribute.StateEnabled,

            //Expanded = Android.Resource.Attribute.StateExpanded,

            /// <summary>
            /// State value for StateListDrawable, set when a view is enabled. 
            /// </summary>
            First = Attribute.StateFirst,

            /// <summary>
            /// State value for StateListDrawable, set when a view has input focus. 
            /// </summary>
            Focused = Attribute.StateFocused,

            //Hovered = Android.Resource.Attribute.StateHovered,

            /// <summary>
            /// State value for StateListDrawable, set when a view or drawable is in the last position in an ordered set. 
            /// </summary>
            Last = Attribute.StateLast,

            //[Register("state_long_pressable")]
            //[Obsolete("deprecated")]
            //public const int StateLongPressable = 16843324;

            /// <summary>
            /// State value for StateListDrawable, set when a view or drawable is in the middle position in an ordered set. 
            /// </summary>
            Middle = Attribute.StateMiddle,

            //Multiline = Android.Resource.Attribute.StateMultiline,

            /// <summary>
            /// State value for StateListDrawable, set when the user is pressing down in a view. 
            /// </summary>
            Pressed = Attribute.StatePressed,

            /// <summary>
            /// State value for StateListDrawable, set when a view (or one of its parents) is currently selected. 
            /// </summary>
            Selected = Attribute.StateSelected,

            /// <summary>
            /// State value for StateListDrawable, set when a view or drawable is considered "single" by its host. 
            /// </summary>
            Single = Attribute.StateSingle,

            /// <summary>
            /// State value for StateListDrawable, set when a view's window has input focus. 
            /// </summary>
            WindowFocused = Attribute.StateWindowFocused,
        }

        public static ColorStateList CreateStateListColor((State[], Color)[] stateListColors)
        {
            var stateCount = stateListColors.Length;
            var states = new int[stateCount][];
            var colors = new int[stateCount];
            for (var index = 0; index < stateCount; index++)
            {
                var state = stateListColors[index];
                states[index] = state.Item1.Select(e => (int)e).ToArray();
                colors[index] = state.Item2;
            }
            return new ColorStateList(states, colors);
        }
    }
}