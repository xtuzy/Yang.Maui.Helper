using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Navigation;
using System;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
namespace Yang.Maui.Helper.Platforms.Android.Controllers
{
    /// <summary>
    /// 包含TabBar的Activity.上限为四个Tab.<br/>
    /// 其实现的App结构为:<br/>
    /// App<br/>
    /// SplashActivity<br/>
    /// TabBarActivity<br/>
    /// (TabBar)Fragment-Fragment-Fragment<br/>
    /// Fragment-Fragment-Fragment<br/>
    /// <br/>
    /// How to use:<br/>
    /// You need override InitTabFragments, InitBottomTabBar,BottomTabBar_ItemSelected,
    /// You not need set SetContentView, because have use a template.
    /// </summary>
    [Activity(Label = "BaseTabBarActivity")]
    public abstract class BaseTabBarActivity : BaseActivity,IBarController
    {
        /// <summary>
        /// 一般应用主界面为三个Tab,每个Tab为一个Fragment,然后Fragment又可以切换到其它Fragment。
        /// 因此每个Tab后的Fragment需要一个栈,控制Fragment返回,而切换Tab则切换栈。
        /// </summary>
        public readonly Stack<BaseFragment> FragmentsStack = new Stack<BaseFragment>();

        /// <summary>
        /// 存储Tab界面的三或四个Fragment,这些不添加到返回栈
        /// </summary>
        public (BaseFragment, BaseFragment, BaseFragment, BaseFragment, BaseFragment) TabFragments;
        
        public int CurrentFragmentIndex;

        /// <summary>
        /// ToolBar在OnCreate时产生
        /// </summary>
        public AndroidX.AppCompat.Widget.Toolbar ToolBar { get; private set; }

        /// <summary>
        /// 在OnCreate时产生
        /// </summary>
        public FrameLayout FragmentContainer { get; private set; }

        /// <summary>
        /// 在OnCreate时产生
        /// </summary>
        public BottomNavigationView BottomTabBar { get; private set; }

        #region 生命周期
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.base_tabbar_activity);
            ToolBar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.activity_toolbar);
            FragmentContainer = FindViewById<FrameLayout>(Resource.Id.fragment_container);
            BottomTabBar = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_bar);
            InitTabFragments();//创建Fragment
            InitBottomTabBar(BottomTabBar);//给BottomNavigationView提供menu.xml
            //设置首页的Fragment
            ShowTabFragment(TabFragments.Item1);
        }

        protected override void OnStart()
        {
            base.OnStart();
            BottomTabBar.ItemSelected += BottomTabBar_ItemSelected;
        }

        protected override void OnStop()
        {
            base.OnStop();
            BottomTabBar.ItemSelected -= BottomTabBar_ItemSelected;
        }

        public override void OnBackPressed()
        {
            if(SupportFragmentManager.BackStackEntryCount == 1)//如果Fragment栈中只剩一个,那么这个返回之后就到了TabBar的Fragment,此时需要显示ToolBar
            {
                ShowToolBar();
                ShowBottomTabBar();
            }
            base.OnBackPressed();
        }

        public new void Dispose()
        {
            ToolBar = null;
            FragmentContainer = null;
            BottomTabBar = null;
            base.Dispose();
        }

        #endregion

        /// <summary>
        /// 创建主界面的三或四个Fragment,请给TabFragment
        /// </summary>
        public abstract void InitTabFragments();

        /// <summary>
        /// 给BottomNavigationView提供menu.xml,注意需要重写
        /// </summary>
        public virtual void InitBottomTabBar(BottomNavigationView bottomNavigationView)
        {
            bottomNavigationView.InflateMenu(Resource.Menu.todolistpage_bottomnavigation_menu);
        }

        /// <summary>
        /// 在此处实现fragment的选择逻辑,根据e.P0.ItemId对应于menu的名称来切换fragment,请不要这些Fragment添加到返回栈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void BottomTabBar_ItemSelected(object sender, NavigationBarView.ItemSelectedEventArgs e)
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            var id = e.P0.ItemId;
            if (id == Resource.Id.page_1)
            {
                transaction.Replace(FragmentContainer.Id, TabFragments.Item1);
                transaction.Commit();
                FragmentsStack.Clear();
                FragmentsStack.Push(TabFragments.Item1);
            }
            else if (id == Resource.Id.page_2)
            {
                transaction.Replace(FragmentContainer.Id, TabFragments.Item2);
                transaction.Commit();
                FragmentsStack.Clear();
                FragmentsStack.Push(TabFragments.Item2);
            }
            else if (id == Resource.Id.page_3)
            {
                transaction.Replace(FragmentContainer.Id, TabFragments.Item3);
                transaction.Commit();
                FragmentsStack.Clear();
                FragmentsStack.Push(TabFragments.Item3);
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public void ShowTabFragment(BaseFragment toFragment)
        {
            if (toFragment == TabFragments.Item1)
                CurrentFragmentIndex = 1;
            else if (toFragment == TabFragments.Item2)
                CurrentFragmentIndex = 2;
            else if (toFragment == TabFragments.Item3)
                CurrentFragmentIndex = 3;
            else if (toFragment == TabFragments.Item4)
                CurrentFragmentIndex = 4;
            else
            {
                throw new NotImplementedException("TabFragments中不包含此Fragment");
            }
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(FragmentContainer.Id, toFragment);
            transaction.Commit();
        }

        #region IBarController

        public bool HidenBottomTabBar()
        {
            if(BottomTabBar != null)
            {
                BottomTabBar.Visibility = ViewStates.Gone;
                return true;
            }else
                return false;
            
        }

        public bool HidenToolBar()
        {
            if (ToolBar != null)
            {
                //ToolBar.Visibility = ViewStates.Gone;
                SupportActionBar?.Hide();
                return true;
            }
            else
                return false;
        }

        public bool ShowBottomTabBar()
        {
            if (BottomTabBar != null)
            {
                BottomTabBar.Visibility = ViewStates.Visible;
                return true;
            }
            else
                return false;
        }

        public bool ShowToolBar()
        {
            if (ToolBar != null)
            {
                ToolBar.Visibility = ViewStates.Visible;
                SupportActionBar?.Show();
                return true;
            }
            else
                return false;
        }

        #endregion
    }
}