#if __ANDROID__
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using static Android.Manifest;
using PM = Android.Content.PM;

namespace Xamarin.Helper.Applications
{
    /// <summary>
    /// 应用级别实用工具合集
    /// </summary>
    public static class AppHelper
    {
        #region 权限
        /// <summary>
        /// 存储权限
        /// </summary>
        private static readonly string[] PERMISSIONS_STORAGE = {
        Permission.ReadExternalStorage,
        Permission.WriteExternalStorage,
        };

        /// <summary>
        /// Defines the REQUEST_EXTERNAL_STORAGE
        /// </summary>
        private static readonly int REQUEST_EXTERNAL_STORAGE = 1;

        /// <summary>
        /// 检查应用程序是否有权写入设备存储
        /// 如果应用没有权限，则系统将提示用户授予权限
        /// </summary>
        /// <param name="activity"></param>
        public static void RequestExternalStoragePermission(Activity activity)
        {
            // 检查我们是否具有写权限
            if (AndroidX.Core.App.ActivityCompat.CheckSelfPermission(activity, Permission.WriteExternalStorage) != PM.Permission.Granted)
            {
                // 我们没有权限，所以提示用户
                AndroidX.Core.App.ActivityCompat.RequestPermissions(
                    activity,
                    PERMISSIONS_STORAGE,
                    REQUEST_EXTERNAL_STORAGE
                );
            }
        }

        #endregion
    }
}
#endif
