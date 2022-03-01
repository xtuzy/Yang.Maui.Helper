#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using PM = Android.Content.PM;
namespace Maui.Platform.Helper.Views
{
    /// <summary>
    /// <see href="https://www.jianshu.com/p/c74286613665">Android弹出、收起软键盘</see><br/>
    /// <see href="https://www.jianshu.com/p/f32707a47e3e">Android软键盘-显示隐藏软键盘</see><br/>
    /// <see href="https://www.jianshu.com/p/89eec61fa699">Android软键盘-弹起时布局向上拉-登录界面</see>
    /// </summary>
    public partial class KeyboardHelper
    {
        /// <summary>
        /// 显示键盘(比如给EditText定义一个外边框,点击外边框也响应键盘)
        /// </summary>
        /// <param name="view"></param>
        /// <param name="editText"></param>
        public static void ShowKeyboard(View view, EditText editText)
        {
            view.PostDelayed(() =>
            {
                editText.RequestFocus();//获取焦点
                InputMethodManager imm = (InputMethodManager)view.Context.GetSystemService(Context.InputMethodService);
                if (imm != null)
                    imm.ShowSoftInput(editText, 0);
            }, 150);
        }

        /// <summary>
        /// 隐藏键盘(用在页面Click事件中)
        /// </summary>
        /// <param name="view">用于Page</param>
        public static void HideKeyboard(View view)
        {
            InputMethodManager imm = (InputMethodManager)view.Context.GetSystemService(Context.InputMethodService);
            if (imm != null)
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }
    }

    /// <summary>
    /// 跟随页面周期
    /// </summary>
    public partial class KeyboardHelper
    {
        private Activity activity;
        private IOnKeyboardStatusChangeListener onKeyBoardStatusChangeListener;
        private int screenHeight;
        // 空白高度 = 屏幕高度 - 当前 Activity 的可见区域的高度
        // 当 blankHeight 不为 0 即为软键盘高度。
        private int blankHeight = 0;
        View Page;
        public KeyboardHelper(Activity activity,View page)
        {
            this.activity = activity;
            screenHeight = activity.Resources.DisplayMetrics.HeightPixels;
            activity.Window.SetSoftInputMode(SoftInput.AdjustResize);
            if (activity.RequestedOrientation != PM.ScreenOrientation.Portrait)
            {
                activity.RequestedOrientation= PM.ScreenOrientation.Portrait;
            }

            Page = page;

            AddKeybaordListner();
        }

        /// <summary>
        /// 在OnCreat中调用
        /// </summary>
        private void AddKeybaordListner()
        {
           Page.ViewTreeObserver.AddOnGlobalLayoutListener(OnGlobalLayoutListener());
        }

        /// <summary>
        /// 在OnDestroy中调用
        /// </summary>
        public void RemoveKeybaordListner()
        {
            Page.ViewTreeObserver.RemoveOnGlobalLayoutListener(OnGlobalLayoutListener());
        }

        ViewTreeObserver.IOnGlobalLayoutListener onGlobalLayoutListener=null;
        private ViewTreeObserver.IOnGlobalLayoutListener OnGlobalLayoutListener()
        {
            if(onGlobalLayoutListener is null)
             onGlobalLayoutListener= new GlobalLayoutListenerForKeyboard(activity, screenHeight, blankHeight, onKeyBoardStatusChangeListener);
            return onGlobalLayoutListener;
        }

        private void setOnKeyBoardStatusChangeListener(
                IOnKeyboardStatusChangeListener onKeyBoardStatusChangeListener)
        {
            this.onKeyBoardStatusChangeListener = onKeyBoardStatusChangeListener;
        }

        public interface IOnKeyboardStatusChangeListener
        {

            void OnKeyboardPop(int keyBoardheight);

            void OnKeyboardClose(int oldKeyBoardheight);
        }

        public class GlobalLayoutListenerForKeyboard : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
        {
            private Activity activity;
            private int screenHeight;
            private int blankHeight;
            IOnKeyboardStatusChangeListener onKeyBoardStatusChangeListener;

            public GlobalLayoutListenerForKeyboard(Activity activity, int screenHeight, int blankHeight, IOnKeyboardStatusChangeListener onKeyBoardStatusChangeListener) : base()
            {
                this.activity = activity;
                this.screenHeight=screenHeight;
                this.blankHeight=blankHeight;
                this.onKeyBoardStatusChangeListener=onKeyBoardStatusChangeListener;
            }

            public GlobalLayoutListenerForKeyboard(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
            {
            }

            public void OnGlobalLayout()
            {
                Rect rect = new Rect();
                activity.Window.DecorView.GetWindowVisibleDisplayFrame(rect);
                int newBlankheight = screenHeight - rect.Bottom;

                if (newBlankheight != blankHeight)
                {
                    if (newBlankheight == 0)
                    {
                        // keyboard close
                        if (onKeyBoardStatusChangeListener != null)
                        {
                            onKeyBoardStatusChangeListener.OnKeyboardClose(blankHeight);
                        }
                    }
                    else
                    {
                        // keyboard pop
                        if (onKeyBoardStatusChangeListener != null)
                        {
                            onKeyBoardStatusChangeListener.OnKeyboardPop(newBlankheight);
                        }
                    }
                }
                blankHeight = newBlankheight;
            }
        }
    }
}

#endif
