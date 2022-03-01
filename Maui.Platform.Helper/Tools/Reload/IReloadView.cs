using System;
using System.Collections.Generic;
using System.Text;

namespace Maui.Platform.Helper.Tools.Reload
{
    /// <summary>
    /// 这个类似Maui将Native控件转换成统一的IView去管理,以在ReloadPreview中不需要继承Native类就可以使用View的方法,相比delegate更合理.
    /// 
    /// </summary>
    public interface IReloadView
    {
        void OnReloadViewAppearing();
        void OnReloadViewAppeared();
        void OnReloadViewMeasure();
        void OnReloadViewLayout();
        void OnReloadViewDisAppearing();
        void OnReloadViewDisAppeared();
    }
}
