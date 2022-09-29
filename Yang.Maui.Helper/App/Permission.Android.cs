using Android.App;
using static Android.Manifest;
using PM = Android.Content.PM;

namespace Yang.Maui.Helper.App
{
    /// <summary>
    /// check permission
    /// </summary>
    public static class Permission
    {
        #region 
        /// <summary>
        /// 
        /// </summary>
        private static readonly string[] PERMISSIONS_STORAGE =
        {
            global::Android.Manifest.Permission.ReadExternalStorage,
            global::Android.Manifest.Permission.WriteExternalStorage,
        };

        /// <summary>
        /// Defines the REQUEST_EXTERNAL_STORAGE
        /// </summary>
        private static readonly int REQUEST_EXTERNAL_STORAGE = 1;

        /// <summary>
        /// check write external storage persmission
        /// </summary>
        /// <param name="activity"></param>
        public static void RequestExternalStoragePermission(Activity activity)
        {
            //check we whether have write permission
            if (AndroidX.Core.App.ActivityCompat.CheckSelfPermission(activity, global::Android.Manifest.Permission.WriteExternalStorage) != PM.Permission.Granted)
            {
                //if not, show dialog
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