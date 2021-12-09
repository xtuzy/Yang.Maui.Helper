using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamarin.Helper.Views
{
    /// <summary>
    /// 参考: https://www.mvvmcross.com/documentation/fundamentals/viewmodel-lifecycle
    /// </summary>
    public interface IBaseView
    {
        bool CustomMeasure { get; }
        bool CustomLayout { get; }
        void ViewAppearing();
        void ViewAppeared();
        void ViewDraw(Canvas canvas);
        bool ViewTouchEvent(MotionEvent e);
        void ViewDisAppearing();
        void ViewDisAppeared();

        void Refresh();
    }
}