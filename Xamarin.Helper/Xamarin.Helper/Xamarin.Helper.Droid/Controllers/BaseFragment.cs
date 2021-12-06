using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using System;
using System.Collections.Generic;
using Xamarin.Helper.Tools;
using Xamarin.Helper.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
namespace Xamarin.Helper.Controllers
{
    public abstract class BaseFragment : Fragment
    {
        public readonly WeakEventManager _eventManager = new WeakEventManager();

        /// <summary>
        /// 根布局
        /// </summary>
        BasePage Page;
        public abstract T GetPage<T>();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);;
            return Page;
        }

        /// <summary>
        /// <see cref="OnStart"/> event, you can subscribe event in it.<br/>
        /// Itself use WeakReference.
        /// </summary>
        public event EventHandler OnStartEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(OnStartEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(OnStartEvent));
        }
        public override void OnStart()
        {
            base.OnStart();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnStartEvent));
        }

        /// <summary>
        /// /// <summary>
        /// Xamarin doc advice remove event at OnPause. see https://docs.microsoft.com/en-us/xamarin/android/deploy-test/performance#remove-event-handlers-in-activities
        /// </summary>
        /// </summary>
        public event EventHandler OnPauseEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(OnPauseEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(OnPauseEvent));
        }

        /// <summary>
        /// Xamarin doc advice remove event at OnPause. see https://docs.microsoft.com/en-us/xamarin/android/deploy-test/performance#remove-event-handlers-in-activities
        /// </summary>
        public override void OnPause()
        {
            base.OnPause();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnPauseEvent));
        }

        /// <summary>
        /// <see cref="OnStop"/> event, you can unsubscribe event in it.<br/>
        /// Itself use WeakReference.
        /// </summary>
        public event EventHandler OnStopEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(OnStopEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(OnStopEvent));
        }

        public override void OnStop()
        {
            base.OnStop();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnStopEvent));
        }

        /// <summary>
        ///  Fragment 按键事件派发
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public virtual bool DispatchKeyEvent(KeyEvent e)
        {
            IList<Fragment> fragments = ChildFragmentManager.Fragments;
            foreach (Fragment fragment in fragments)
            {
                // 这个子 Fragment 必须是 BaseFragment 的子类，并且处于可见状态
                if (!(fragment is BaseFragment) || fragment.Lifecycle.CurrentState != AndroidX.Lifecycle.Lifecycle.State.Resumed)
                {
                    continue;
                }
                // 将按键事件派发给子 Fragment 进行处理
                if ((fragment as BaseFragment).DispatchKeyEvent(e))
                {
                    // 如果子 Fragment 拦截了这个事件，那么就不交给父 Fragment 处理
                    return true;
                }
            }
            switch (e.Action)
            {
                case KeyEventActions.Down:
                    return onKeyDown(e.KeyCode, e);
                case KeyEventActions.Up:
                    return onKeyUp(e.KeyCode, e);
                default:
                    return false;
            }
        }
        public virtual bool onKeyDown(Keycode keyCode, KeyEvent e)
        {
            // 默认不拦截按键事件
            return false;
        }

        public virtual bool onKeyUp(Keycode keyCode, KeyEvent e)
        {
            // 默认不拦截按键事件
            return false;
        }

        /// <summary>
        /// 监听Dark和Light主题改变<br/>
        /// 使用这个需要在配置文件中声明android:configChanges="uiMode"<br/>
        /// Event Arg is <see cref="Theme"/>.
        /// </summary>
        public event EventHandler ThemeChangedEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(ThemeChangedEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(ThemeChangedEvent));
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            var currentNightMode = newConfig.UiMode & UiMode.NightMask;
            switch (currentNightMode)
            {
                case UiMode.NightNo:
                    // 夜间模式未启用，使用浅色主题
                    _eventManager.RaiseEvent(this, Xamarin.Helper.Tools.Theme.Light, nameof(ThemeChangedEvent));
                    break;
                case UiMode.NightYes:
                    // 夜间模式启用，使用深色主题
                    _eventManager.RaiseEvent(this, Xamarin.Helper.Tools.Theme.Dark, nameof(ThemeChangedEvent));
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Page = null;
            base.Dispose(disposing);
        }
    }
}