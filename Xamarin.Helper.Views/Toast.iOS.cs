#if __IOS__
using CoreFoundation;
using GameController;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
namespace Xamarin.Helper.Views
{
    /// <summary>
    /// 实现类似Android的Toast,但位置在中间
    /// 参考:https://stackoverflow.com/questions/18680891/displaying-a-message-in-ios-which-has-the-same-functionality-as-toast-in-android
    /// </summary>
    public class Toast
    {

        public static void MakeText(UIViewController viewController, string message, ToastLength seconds)
        {
            var alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            alert.View.BackgroundColor = UIColor.Black;
            alert.View.Alpha = 0.6f;
            alert.View.Layer.CornerRadius = 15;

            viewController.PresentViewController(alert, animated: true, null);

            DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, TimeSpan.FromMilliseconds((int)seconds)), () =>
            {
                alert.DismissViewController(animated: true, null);
            });
        }

        public enum ToastLength
        {
            Short = 800,
            Long = 1000
        }
    }
}
#endif