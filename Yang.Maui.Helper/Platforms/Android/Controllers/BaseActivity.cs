using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using Yang.Maui.Helper.Platforms.Android.Actions;
using Yang.Maui.Helper.Tools;
using Fragment = AndroidX.Fragment.App.Fragment;
namespace Yang.Maui.Helper.Platforms.Android.Controllers
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity : AppCompatActivity, IKeyboardAction
    {
        #region 域和属性
        /// <summary>
        /// 为生命周期事件添加弱引用,避免在忘记取消订阅时内存泄漏.
        /// 这里为何使用弱事件管理,因为外部订阅周期事件时,里面可能存在更长周期的引用,例如引用了更长周期的ViewModel,则会导致Activity无法回收.
        /// </summary>
        public readonly WeakEventManager _eventManager = new WeakEventManager();


        #endregion

        #region 构造函数
        #endregion
        


        #region 生命周期
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _eventManager.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnCreate), nameof(LifeCycleEvent));
        }

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            _eventManager.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnCreateView), nameof(LifeCycleEvent));
            return base.OnCreateView(parent, name, context, attrs);
        }

        protected override void OnStart()
        {
            base.OnStart();
            InitSoftKeyboard();
            _eventManager.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnStart), nameof(LifeCycleEvent));
        }

        protected override void OnStop()
        {
            base.OnStop();
            _eventManager.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnStop), nameof(LifeCycleEvent));
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            _eventManager.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnDestroy), nameof(LifeCycleEvent));
        }

        #endregion

        #region 事件

        /// <summary>
        /// 模仿Android的Lifcycle组件,将生命周期可以四处使用<br/>
        /// Itself use WeakReference.
        /// </summary>
        public event EventHandler LifeCycleEvent
        {
            add => _eventManager.AddEventHandler(value, nameof(LifeCycleEvent));
            remove => _eventManager.RemoveEventHandler(value, nameof(LifeCycleEvent));
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

        #endregion

        #region 功能




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



        #endregion



        #region 辅助方法

        protected void InitSoftKeyboard()
        {
            Action action = () =>
            {
                HideKeyboard(CurrentFocus);//隐藏软键，避免内存泄漏
            };
            // 点击外部隐藏软键盘，提升用户体验
            GetContentView().SetOnClickListener(new ClickAction(action));
        }

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

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            var currentNightMode = newConfig.UiMode & UiMode.NightMask;
            switch (currentNightMode)
            {
                case UiMode.NightNo:
                    // 夜间模式未启用，使用浅色主题
                    _eventManager.RaiseEvent(this, Yang.Maui.Helper.Tools.Theme.Light, nameof(ThemeChangedEvent));
                    break;
                case UiMode.NightYes:
                    // 夜间模式启用，使用深色主题
                    _eventManager.RaiseEvent(this, Yang.Maui.Helper.Tools.Theme.Dark, nameof(ThemeChangedEvent));
                    break;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Microsoft.Maui.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion
    }
}