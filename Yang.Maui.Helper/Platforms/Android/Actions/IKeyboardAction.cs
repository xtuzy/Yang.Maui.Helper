using Android.Views;

namespace Yang.Maui.Helper.Platforms.Android.Actions
{

    /**
     *    author : Android 轮子哥
     *    github : https://github.com/getActivity/AndroidProject
     *    time   : 2020/03/08
     *    desc   : 软键盘相关意图
     */
    /// <summary>
    /// 软键盘相关意图
    /// </summary>
    public interface IKeyboardAction
    {

        /// <summary>
        /// 显示软键盘
        /// </summary>
        /// <param name="view"></param>
        public void ShowKeyboard(View view);

        /// <summary>
        /// 隐藏软键盘
        /// </summary>
        /// <param name="view"></param>
        public void HideKeyboard(View view);

        /// <summary>
        /// 切换软键盘
        /// </summary>
        /// <param name="view"></param>
        public void ToggleSoftInput(View view);
    }
}