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
    /// <summary>
    /// 模仿Android的Lifcycle组件,将生命周期可以四处使用
    /// </summary>
    public enum LifeCycle
    {
        UnKnow,
        OnCreate,
        OnCreateView,
        //OnResume,
        OnStart,
        //OnPause,
        OnStop,
        OnDestroy,
    }

    public class LifeCycleArgs : EventArgs
    {
        LifeCycle Event = LifeCycle.UnKnow;

        public LifeCycleArgs(LifeCycle lifeCycle)
        {
            Event = lifeCycle;
        }
    }
}