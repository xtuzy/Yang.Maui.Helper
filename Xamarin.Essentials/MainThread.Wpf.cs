using System;
using System.Threading;

namespace Xamarin.Essentials
{
    public static partial class MainThread
    {
        static void PlatformBeginInvokeOnMainThread(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                action();
            });
        }
        static bool PlatformIsMainThread =>
            Thread.CurrentThread == System.Windows.Application.Current.Dispatcher.Thread;
    }
}
