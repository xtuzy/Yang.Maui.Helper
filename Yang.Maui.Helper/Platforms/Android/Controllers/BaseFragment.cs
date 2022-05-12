using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using System;
using System.Collections.Generic;
using Yang.Maui.Helper.Logs;
using Yang.Maui.Helper.Platforms.Android.Layouts;
using Yang.Maui.Helper.Tools;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Yang.Maui.Helper.Platforms.Android.Controllers
{
    public class BaseFragment : Fragment, IBarController, IFragmentNavigation
    {
        #region 域和属性
        /// <summary>
        /// 管理事件,这里为何使用弱事件管理,因为外部订阅周期事件时,里面可能存在更长周期的引用,例如引用了更长周期的ViewModel,则会导致Fragment无法回收.
        /// </summary>
        public readonly WeakEventManager _eventManager;

        /// <summary>
        /// RootLayout = ToolBar+ContentView
        /// </summary>
        private ConstraintLayout RootLayout;
        /// <summary>
        /// Fragment的Toolbar,区别于Activity的Toolbar(即SupportActionBar),可以使用它去自定义与Fragment相关的内容(返回按钮,标题,Menu)。<br/>
        /// 其在OnCreateView中创建.,可在OnCreated中获取对象.
        /// </summary>
        public Toolbar ToolBar { get; private set; }
        /// <summary>
        /// 除ToolBar外的区域,可在该Layout内添加控件,其在OnCreateView中创建,可在OnCreated中获取对象.
        /// </summary>
        public ConstraintLayout ContentView { get; private set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 执行事件定义的生命周期,其与接口定义的生命周期互斥
        /// </summary>
        public BaseFragment()
        {
            _eventManager = new WeakEventManager();
        }

        #endregion

        #region 生命周期
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            _eventManager?.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnCreate), nameof(LifeCycleEvent));
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            RootLayout = inflater.Inflate(Resource.Layout.base_fragment, null, false) as ConstraintLayout;
            ToolBar = RootLayout.FindViewById<Toolbar>(Resource.Id.fragment_toolbar);
            ContentView = RootLayout.FindViewById<ConstraintLayout>(Resource.Id.fragment_content);
            _eventManager?.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnCreateView), nameof(LifeCycleEvent));
            return RootLayout;
        }

        public override void OnStart()
        {
            base.OnStart();
            _eventManager?.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnStart), nameof(LifeCycleEvent));
        }

        public override void OnStop()
        {
            base.OnStop();
            _eventManager?.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnStop), nameof(LifeCycleEvent));
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _eventManager?.RaiseEvent(this, new LifeCycleArgs(LifeCycle.OnDestroy), nameof(LifeCycleEvent));
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
        /// 模仿Android的Lifcycle组件,将生命周期可以四处使用<br/>
        /// Itself use WeakReference.
        /// </summary>
        public event EventHandler LifeCycleEvent
        {
            add => _eventManager?.AddEventHandler(value, nameof(LifeCycleEvent));
            remove => _eventManager?.RemoveEventHandler(value, nameof(LifeCycleEvent));
        }

        /// <summary>
        ///  Fragment 按键事件派发(我也不知道哪里复制来的,不懂,先不删)
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
            else
                throw new NotImplementedException("当前仅在TabBar中实现前进");
        }

        /// <summary>
        /// 回退
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>

        public void PopViewController()
        {
            if (Activity is BaseTabBarActivity)
            {
                var activity = Activity as BaseTabBarActivity;
                activity.OnBackPressed();//移除系统栈,销毁Fragment
                //activity.SupportFragmentManager.PopBackStack();
                activity.FragmentsStack.Pop().Dispose();//移除自定义栈,释放资源
            }
            else
                throw new NotImplementedException("当前仅在TabBar中实现回退");
        }

        /// <summary>
        /// 回退到某Fragment,必须在栈内,否则会导致退出程序或其他Bug
        /// </summary>
        /// <param name="toFragment"></param>
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
            else
                throw new NotImplementedException("当前仅在TabBar中实现回退");
        }

        /// <summary>
        /// 回退到Tab的Fragment,Tab的Fragment必须是不在栈内的,Tab的Fragment和Activity一起存亡
        /// </summary>
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
            else if (Activity == null)//Reload 时遇到的
            {
                LogHelper.Debug($"Error: In {this.GetType().Name}, Activity is null");
            }
            else
                throw new NotImplementedException("当前仅在TabBar中实现回退");
        }

        /// <summary>
        /// 隐藏Fragment的ToolBar
        /// </summary>
        /// <returns></returns>
        public bool HidenToolBar()
        {
            if (ToolBar != null)
                ToolBar.Visibility = ViewStates.Gone;
            else return false;
            return true;
        }

        /// <summary>
        /// 显示Fragment的ToolBar
        /// </summary>
        /// <returns></returns>
        public bool ShowToolBar()
        {
            if (ToolBar != null)
                ToolBar.Visibility = ViewStates.Visible;
            else return false;
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
        /// 设置ToolBar在ContentView顶部,但不遮挡
        /// </summary>
        public void SetContentViewAtToolBarBottom()
        {
            if (ToolBar == null)
                return;
            var set = new ConstraintSet();
            ContentView.LayoutParameters =
                new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0);
            set.Clone(RootLayout);
            set.Connect(ContentView.Id, ConstraintSet.Top, ToolBar.Id, ConstraintSet.Bottom);
            set.Connect(ContentView.Id, ConstraintSet.Bottom, RootLayout.Id, ConstraintSet.Bottom);
            set.ApplyTo(RootLayout);
        }

        /// <summary>
        /// 设置ToolBar在顶部遮住ContentView
        /// </summary>
        public void SetContentViewAtToolBarBelow()
        {
            if (ToolBar == null)
                return;
            var set = new ConstraintSet();
            ContentView.LayoutParameters =
                new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            set.Clone(RootLayout);
            set.Clear(ToolBar.Id);
            set.Clear(ContentView.Id);
            set.ApplyTo(RootLayout);
        }
        #endregion

        #region 辅助方法

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            var currentNightMode = newConfig.UiMode & UiMode.NightMask;
            switch (currentNightMode)
            {
                case UiMode.NightNo:
                    // 夜间模式未启用，使用浅色主题
                    _eventManager?.RaiseEvent(this, Yang.Maui.Helper.Tools.Theme.Light, nameof(ThemeChangedEvent));
                    break;
                case UiMode.NightYes:
                    // 夜间模式启用，使用深色主题
                    _eventManager?.RaiseEvent(this, Yang.Maui.Helper.Tools.Theme.Dark, nameof(ThemeChangedEvent));
                    break;
            }
        }

        #endregion

    }
}