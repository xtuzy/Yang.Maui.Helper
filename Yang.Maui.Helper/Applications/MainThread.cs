#if __ANDROID__
using Android.OS;
#endif
namespace Yang.Maui.Helper.Applications
{
    public partial class MainThread
    {
#if __ANDROID__
        static volatile Handler handler;
#endif

        /// <summary>
        /// Xamarin.Essential no WPF,so not use it.<br/>
        /// https://github.com/xamarin/Essentials/tree/8657192a8963877e389a533b8feb324af6f89c8b/Xamarin.Essentials/
        /// </summary>
        /// <param name="action"></param>
        public static void InvokeInMainThread(Action action)
        {
#if WINDOWS7_0_OR_GREATER
            //https://stackoverflow.com/questions/69885338/how-to-get-dispatcherqueue-in-winui-3-desktop-using-windows-app-sdk
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            dispatcherQueue.TryEnqueue(()=> { action.Invoke(); });
            // #if WPF
            //                System.Windows.Application.Current.Dispatcher.Invoke(() =>
            //                {
            //                    action.Invoke();
            //                });
            // #elif WINDOWS_UWP
            //            var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView?.CoreWindow?.Dispatcher;

            //            if (dispatcher == null)
            //                throw new InvalidOperationException("Unable to find main thread.");
            //            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => action()).AsTask().WatchForError();
#elif MACOS
            AppKit.NSApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                action.Invoke();
            });
#elif __ANDROID__
            if (handler?.Looper != Android.OS.Looper.MainLooper)
             handler = new Handler(Looper.MainLooper);
            handler.Post(action);
#elif __IOS__
            Foundation.NSRunLoop.Main.BeginInvokeOnMainThread(action.Invoke);
#else
            action.Invoke();
#endif
        }
    }



#if WINDOWS7_0_OR_GREATER
    /// <summary>
    /// https://github.com/xamarin/Essentials/blob/8657192a8963877e389a533b8feb324af6f89c8b/Xamarin.Essentials/MainThread/MainThreadExtensions.uwp.cs
    /// </summary>
    internal static partial class MainThreadExtensions
    {
        internal static void WatchForError(this Windows.Foundation.IAsyncAction self) =>
            self.AsTask().WatchForError();

        internal static void WatchForError<T>(this Windows.Foundation.IAsyncOperation<T> self) =>
            self.AsTask().WatchForError();

        internal static void WatchForError(this Task self)
        {
            var context = SynchronizationContext.Current;
            if (context == null)
                return;

            self.ContinueWith(
                t =>
                {
                    var exception = t.Exception.InnerExceptions.Count > 1 ? t.Exception : t.Exception.InnerException;

                    context.Post(e => { throw (Exception)e; }, exception);
                }, CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.Default);
        }
    }
#endif

}
