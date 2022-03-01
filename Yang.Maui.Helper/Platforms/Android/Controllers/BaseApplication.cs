using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yang.Maui.Helper.Platforms.Android.Controllers
{
    public partial class BaseApplication : Application
    {
        const string TAG = "BaseApplication";

        public static BaseApplication Current;

        /// <summary>
        /// 用来注册服务,包括ViewModel.Service
        /// </summary>
        public IServiceProvider Services;

        /// <summary>
        /// 替代Ioc来暂时存储ViewModel
        /// </summary>

        public Dictionary<string,object> ViewModels = new Dictionary<string, object>();

        public BaseApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }

        public override void OnCreate()
        {
            Current = this;
            base.OnCreate();
        }

    }
}