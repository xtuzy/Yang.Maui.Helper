using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using System;
using System.Collections.Generic;
using Xamarin.Helper.Tools;
using Xamarin.Helper.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Xamarin.Helper.Layouts.Constraint;
namespace Xamarin.Helper.Controllers
{
    public abstract class BaseFragment : Fragment, IBarController,IFragmentNavigation
    {
        public readonly WeakEventManager _eventManager;
        WeakReference<IReloadableFragment> _fragmentManager;
        private ConstraintLayout RootLayout;

        protected BaseFragment()
        {
            _eventManager = new WeakEventManager();
        }

        protected BaseFragment(IReloadableFragment reloadableFragment)
        {
            _fragmentManager = new WeakReference<IReloadableFragment> (reloadableFragment);
        }


        /// <summary>
        /// Fragment的Toolbar,区别于Activity的Toolbar(即SupportActionBar),可以使用它去自定义与Fragment相关的内容(返回按钮,标题,Menu)。
        /// </summary>
        public Toolbar ToolBar { get; private set; }
        public ConstraintLayout ContentView { get; private set; }

        #region 生命周期
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            RootLayout = inflater.Inflate(Resource.Layout.base_fragment, null, false) as ConstraintLayout;
            ToolBar = RootLayout.FindViewById<Toolbar>(Resource.Id.fragment_toolbar);
            ContentView = RootLayout.FindViewById<ConstraintLayout>(Resource.Id.fragment_content);
            return RootLayout;
        }

        public override void OnStart()
        {
            base.OnStart();
            _eventManager?.RaiseEvent(this, EventArgs.Empty, nameof(OnStartEvent));
            GetReloadableFragment(_fragmentManager)?.OnStart();
        }

        public override void OnPause()
        {
            base.OnPause();
            _eventManager?.RaiseEvent(this, EventArgs.Empty, nameof(OnPauseEvent));
            GetReloadableFragment(_fragmentManager)?.OnPause();
        }

        public override void OnStop()
        {
            base.OnStop();
            _eventManager?.RaiseEvent(this, EventArgs.Empty, nameof(OnStopEvent));
            GetReloadableFragment(_fragmentManager)?.OnStop();
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            GetReloadableFragment(_fragmentManager)?.OnSDestroy();
        }

        public new void Dispose()
        {
            ToolBar = null;
            ContentView = null;
            base.Dispose();
        }

        #endregion

        #region 事件

        /// <summary>
        /// <see cref="OnStart"/> event, you can subscribe event in it.<br/>
        /// Itself use WeakReference.
        /// </summary>
        public event EventHandler OnStartEvent
        {
            add => _eventManager?.AddEventHandler(value, nameof(OnStartEvent));
            remove => _eventManager?.RemoveEventHandler(value, nameof(OnStartEvent));
        }


        /// <summary>
        /// /// <summary>
        /// Xamarin doc advice remove event at OnPause. see https://docs.microsoft.com/en-us/xamarin/android/deploy-test/performance#remove-event-handlers-in-activities
        /// </summary>
        /// </summary>
        public event EventHandler OnPauseEvent
        {
            add => _eventManager?.AddEventHandler(value, nameof(OnPauseEvent));
            remove => _eventManager?.RemoveEventHandler(value, nameof(OnPauseEvent));
        }

        /// <summary>
        /// Xamarin doc advice remove event at OnPause. see https://docs.microsoft.com/en-us/xamarin/android/deploy-test/performance#remove-event-handlers-in-activities
        /// </summary>


        /// <summary>
        /// <see cref="OnStop"/> event, you can unsubscribe event in it.<br/>
        /// Itself use WeakReference.
        /// </summary>
        public event EventHandler OnStopEvent
        {
            add => _eventManager?.AddEventHandler(value, nameof(OnStopEvent));
            remove => _eventManager?.RemoveEventHandler(value, nameof(OnStopEvent));
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
            add => _eventManager?.AddEventHandler(value, nameof(ThemeChangedEvent));
            remove => _eventManager?.RemoveEventHandler(value, nameof(ThemeChangedEvent));
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            var currentNightMode = newConfig.UiMode & UiMode.NightMask;
            switch (currentNightMode)
            {
                case UiMode.NightNo:
                    // 夜间模式未启用，使用浅色主题
                    _eventManager?.RaiseEvent(this, Xamarin.Helper.Tools.Theme.Light, nameof(ThemeChangedEvent));
                    break;
                case UiMode.NightYes:
                    // 夜间模式启用，使用深色主题
                    _eventManager?.RaiseEvent(this, Xamarin.Helper.Tools.Theme.Dark, nameof(ThemeChangedEvent));
                    break;
            }
        }
        #endregion

        #region 功能
        public void PushViewController(BaseFragment toFragment)
        {
            if (Activity is BaseTabBarActivity)
            {
                var activity = Activity as BaseTabBarActivity;
                var transaction = Activity.SupportFragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.fragment_container, toFragment);
                transaction.AddToBackStack(Activity.GetType().Name);//TODO:验证子类获取的名称是否正确
                activity.FragmentsStack.Push(toFragment);
                //transaction.Hide(this);//使用Add时需要隐藏
                transaction.Commit();
            }
        }


        public void PopViewController()
        {
            if (Activity is BaseTabBarActivity)
            {
                var activity = Activity as BaseTabBarActivity;
                activity.OnBackPressed();//移除系统栈,销毁Fragment
                //activity.SupportFragmentManager.PopBackStack();
                activity.FragmentsStack.Pop().Dispose();//移除自定义栈,释放资源
            }
        }

        public void PopToViewController(BaseFragment toFragment)
        {
            if (Activity is BaseTabBarActivity)
            {
                var activity = Activity as BaseTabBarActivity;

                if (this == toFragment)
                {

                }
                else
                {
                    activity.OnBackPressed();//移除系统栈,销毁Fragment
                    //activity.SupportFragmentManager.PopBackStack();
                    activity.FragmentsStack.Pop().Dispose();//移除自定义栈,释放资源
                    activity.FragmentsStack.TryPeek(out var fragment);
                    fragment?.PopToViewController(toFragment);
                }
            }
        }

        public void PopToRootViewController()
        {
            if (Activity is BaseTabBarActivity)
            {
                var activity = Activity as BaseTabBarActivity;

                if (activity.FragmentsStack.Count == 0)
                {

                }
                else
                {
                    activity.OnBackPressed();//移除系统栈,销毁Fragment
                    //activity.SupportFragmentManager.PopBackStack();
                    activity.FragmentsStack.Pop().Dispose();//移除自定义栈,释放资源
                    activity.FragmentsStack.TryPeek(out var fragment);
                    fragment?.PopToRootViewController();
                }
            }
        }

        /// <summary>
        /// 隐藏Activity的TabBar,如果所属Activity是TabBarActivity，则可以隐藏.<br/>
        /// 对应iOS的ViewController.HidesBottomBarWhenPushed = true;
        /// </summary>
        public bool HidenBottomTabBar()
        {
            if (Activity is IBarController)
            {
                var activity = Activity as IBarController;
                activity.HidenBottomTabBar();
                return true;
            }
            return false;
        }

        /// <summary>
        /// ToolBar.Visibility=Gone
        /// </summary>
        /// <returns></returns>
        public bool HidenToolBar()
        {
            ToolBar.Visibility = ViewStates.Gone;
            return true;
        }

        public bool ShowToolBar()
        {
            ToolBar.Visibility = ViewStates.Visible;
            return true;
        }
        /// <summary>
        /// 显示Activity的TabBar
        /// </summary>
        /// <returns></returns>
        public bool ShowBottomTabBar()
        {
            if (Activity is IBarController)
            {
                var activity = Activity as IBarController;
                activity.ShowBottomTabBar();
                return true;
            }
            return false;
        }

        /// <summary>
        /// ToolBar在ContentView顶部,不遮挡
        /// </summary>
        public void SetContentViewAtToolBarBottom()
        {
            var set = new ConstraintSet();
            ContentView.LayoutParameters =
                new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0);
            set.Clone(RootLayout);
            set.AddConnect(ContentView, NSLayoutAttribute.Top, ToolBar, NSLayoutAttribute.Bottom)
            .AddConnect(ContentView, NSLayoutAttribute.Bottom, RootLayout, NSLayoutAttribute.Bottom);
            set.ApplyTo(RootLayout);
        }

        /// <summary>
        /// ToolBar会遮住ContentView
        /// </summary>
        public void SetContentViewAtToolBarBelow()
        {
            var set = new ConstraintSet();
            ContentView.LayoutParameters =
                new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            set.Clone(RootLayout);
            set.Clear(ToolBar.Id);
            set.Clear(ContentView.Id);
            set.ApplyTo(RootLayout);
        }
        #endregion

        IReloadableFragment? GetReloadableFragment(WeakReference<IReloadableFragment> weakReference)
        {
            IReloadableFragment fragment = null;
            weakReference?.TryGetTarget(out fragment);
            if (fragment == null)
                return null;
            else
                return fragment;
        }
    }
}