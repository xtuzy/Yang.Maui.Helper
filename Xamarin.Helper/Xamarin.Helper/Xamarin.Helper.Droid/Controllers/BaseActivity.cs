using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using Xamarin.Helper.Actions;
using Xamarin.Helper.Tools;
using Fragment = AndroidX.Fragment.App.Fragment;
namespace Xamarin.Helper.Controllers
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity : AppCompatActivity, IKeyboardAction
    {
       
        /// <summary>
        /// 为生命周期事件添加弱引用,避免在忘记取消订阅时内存泄漏
        /// </summary>
        public readonly WeakEventManager _eventManager = new WeakEventManager();
        

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
        protected override void OnStart()
        {
            base.OnStart();
            InitSoftKeyboard();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnStartEvent));
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
        protected override void OnStop()
        {
            base.OnStop();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnStopEvent));
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
        protected override void OnPause()
        {
            base.OnPause();
            _eventManager.RaiseEvent(this, EventArgs.Empty, nameof(OnPauseEvent));
        }

        protected void InitSoftKeyboard()
        {
            Action action = () =>
            {
                HideKeyboard(CurrentFocus);//隐藏软键，避免内存泄漏
            };
            // 点击外部隐藏软键盘，提升用户体验
            GetContentView().SetOnClickListener(new ClickAction(action));
        }


        #region KeyboardAction
        /// <summary>
        /// 显示软键盘
        /// </summary>
        /// <param name="view"></param>
        public virtual void ShowKeyboard(View view)
        {
            if (view == null)
            {
                return;
            }
            InputMethodManager manager = (InputMethodManager)view.Context.GetSystemService(Context.InputMethodService);
            if (manager != null)
            {
                manager.ShowSoftInput(view, 0);
            }
        }

        /// <summary>
        /// 隐藏软键盘
        /// </summary>
        /// <param name="view"></param>
        public virtual void HideKeyboard(View view)
        {
            if (view == null)
            {
                return;
            }
            InputMethodManager manager = (InputMethodManager)view.Context.GetSystemService(Context.InputMethodService);
            if (manager != null)
            {
                manager.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.NotAlways);
            }
        }

        /// <summary>
        /// 切换软键盘
        /// </summary>
        /// <param name="view"></param>
        public virtual void ToggleSoftInput(View view)
        {
            if (view == null)
            {
                return;
            }
            InputMethodManager manager = (InputMethodManager)view.Context.GetSystemService(Context.InputMethodService);
            if (manager != null)
            {
                manager.ToggleSoftInput(0, 0);
            }
        }

        #endregion

        public override void Finish()
        {
            // 隐藏软键，避免内存泄漏
            HideKeyboard(CurrentFocus);
            base.Finish();
        }

        //如果当前的 Activity（singleTop 启动模式） 被复用时会回调
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            // 设置为当前的 Intent，避免 Activity 被杀死后重启 Intent 还是最原先的那个
            Intent = intent;
        }

        /// <summary>
        /// 和 setContentView 对应的方法
        /// </summary>
        /// <returns></returns>
        public ViewGroup GetContentView()
        {
            return FindViewById(Window.IdAndroidContent) as ViewGroup;
        }

        #region IActivityAction
        
        public new void StartActivity(System.Type activityName)
        {
            StartActivity(new Intent(this, activityName));
        }

        public override void StartActivity(Intent intent)
        {
            /* if (!(Context is Activity))
             {
                 // 如果当前的上下文不是 Activity，调用 startActivity 必须加入新任务栈的标记，否则会报错：android.util.AndroidRuntimeException
                 // Calling startActivity() from outside of an Activity context requires the FLAG_ACTIVITY_NEW_TASK flag. Is this really what you want?
                 intent.AddFlags(ActivityFlags.NewTask);
             }*/
            base.StartActivity(intent);
        }
        #endregion

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            IList<Fragment> fragments = SupportFragmentManager.Fragments;
            foreach (Fragment fragment in fragments)
            {
                // 这个 Fragment 必须是 BaseFragment 的子类，并且处于可见状态
                if (!(fragment is BaseFragment) || (fragment.Lifecycle.CurrentState != AndroidX.Lifecycle.Lifecycle.State.Resumed))
                {
                    continue;
                }
                // 将按键事件派发给 Fragment 进行处理
                if ((fragment as BaseFragment).DispatchKeyEvent(e))
                {
                    // 如果 Fragment 拦截了这个事件，那么就不交给 Activity 处理
                    return true;
                }
            }
            return base.DispatchKeyEvent(e);
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
            switch(currentNightMode)
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
    }
}