
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using UIElement = Android.Views.View;
#elif WINDOWS
using Microsoft.UI.Xaml;
#elif __IOS__
using UIElement = UIKit.UIView;
#else
using UIElement = System.Object;
#endif
namespace Yang.Maui.Helper.App
{
    public class UIThread
    {
        public static void Invoke(Action action, UIElement view)
        {
#if WINDOWS
            if (view.DispatcherQueue == null)
            {
                Trace.WriteLine("UIThread.Invoke: ConstraintLayout.DispatcherQueue == null");
            }
            else
            {
                view.DispatcherQueue.TryEnqueue(() =>
                {
                    action.Invoke();
                });
            }
#elif __ANDROID__
            view.Post(() =>
            {
                action.Invoke();
            });
#elif __IOS__
            CoreFoundation.DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                action.Invoke();
            });
#endif
        }
    }
}
