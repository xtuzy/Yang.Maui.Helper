using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;
using Xamarin.Helper.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
namespace Xamarin.Helper.Controllers
{
    /// <summary>
    /// 三个Tab的Activity
    /// </summary>
    [Activity(Label = "BaseTabBarActivity")]
    public abstract class BaseTabBarActivity : BaseActivity
    {
        /// <summary>
        /// 一般应用主界面为三个Tab,每个Tab为一个Fragment,然后Fragment又可以切换到其它Fragment。
        /// 因此每个Tab后的Fragment需要一个栈,控制Fragment返回,而切换Tab则切换栈。
        /// </summary>
        public readonly Stack<Fragment> FragmentStack = new Stack<Fragment>();

        List<BaseFragment> mFragments = new List<BaseFragment>();
        FrameLayout mframeLayout;
        BottomNavigationView mBottomNavigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var RootView = new BaseView(this);
            RootView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            SetContentView(RootView);
            mframeLayout = new FrameLayout(this) { Id = View.GenerateViewId() };
            mBottomNavigationView = new BottomNavigationView(this) { Id = View.GenerateViewId() };
            InitFragment(mFragments);//创建Fragment
            InitBottomNavigationViewItem(mBottomNavigationView);//给BottomNavigationView提供menu.xml
            //设置首页的Fragment
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(mframeLayout.Id, mFragments[0]);
            transaction.Commit();
        }

        /// <summary>
        /// 创建Fragment,请添加到列表
        /// </summary>
        public abstract void InitFragment(List<BaseFragment> fragments);

        /// <summary>
        /// 给BottomNavigationView提供menu.xml,注意需要重写
        /// </summary>
        public virtual void InitBottomNavigationViewItem(BottomNavigationView bottomNavigationView)
        {
            bottomNavigationView.InflateMenu(Resource.Menu.todolistpage_bottomnavigation_menu);
        }

        protected override void OnStart()
        {
            base.OnStart();
            mBottomNavigationView.ItemSelected += MBottomNavigationView_ItemSelected;
        }

        /// <summary>
        /// 在此处实现fragment的选择逻辑,根据e.P0.ItemId对应于menu的名称来切换fragment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void MBottomNavigationView_ItemSelected(object sender, Google.Android.Material.Navigation.NavigationBarView.ItemSelectedEventArgs e)
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            var id = e.P0.ItemId;
            if (id == Resource.Id.page_1)
            {
                transaction.Replace(mframeLayout.Id, mFragments[0]);
                transaction.Commit();
                FragmentStack.Clear();
                FragmentStack.Push(mFragments[0]);
            }
            else if (id == Resource.Id.page_2)
            {
                transaction.Replace(mframeLayout.Id, mFragments[1]);
                transaction.Commit();
                FragmentStack.Clear();
                FragmentStack.Push(mFragments[1]);
            }
            else if (id == Resource.Id.page_3)
            {
                transaction.Replace(mframeLayout.Id, mFragments[2]);
                transaction.Commit();
                FragmentStack.Clear();
                FragmentStack.Push(mFragments[2]);
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        protected override void OnStop()
        {
            base.OnStop();
            mBottomNavigationView.ItemSelected -= MBottomNavigationView_ItemSelected;
        }
    }
}