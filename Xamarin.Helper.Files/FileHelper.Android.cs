#if __ANDROID__
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Text;
using Android.Util;
using Android.Widget;
using System;
using System.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace Xamarin.Helper.Files
{
    /// <summary>
    /// 用于从文件路径的Uri解析文件真实路径名.
    /// 适用于Android9。
    /// </summary>
    public static partial class FileHelper
    {
        /// <summary>
        /// Defines the context,TODO:将其设置为弱应用
        /// </summary>
        [Obsolete("deprecated", true)]
        public static string GetFileRealPathFromUriStr(Context context, string uriPath)
        {
            var uri = Uri.Parse(uriPath);
            return GetFileRealPathFromUri(context, uri);
        }

        /// <summary>
        ///  获取uri对应的路径
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [Obsolete("deprecated", true)]
        public static string GetFileRealPathFromUri(Context context, Uri uri)
        {
            string chooseFilePath = null;
            if ("file".Equals(uri.Scheme.ToLower()))
            {//使用第三方应用打开
                chooseFilePath = uri.Path;
                Toast.MakeText(context, chooseFilePath, ToastLength.Short).Show();
                return chooseFilePath;
            }
            if (Build.VERSION.SdkInt > BuildVersionCodes.Kitkat)
            {//4.4以后
                chooseFilePath = GetPathAfterKitkat(context, uri);
            }
            else
            {//4.4以下下系统调用方法
                chooseFilePath = GetPathBeforeKitkat(context, uri);
            }
            return chooseFilePath;
        }


        /// <summary>
        /// 专为Android4.4设计的从Uri获取文件绝对路径，以前的方法已不好使
        /// 注：Android8中不能读取sdcard;未测试8以下
        /// </summary>
        /// <param name="context">The context<see cref="Context"/></param>
        /// <param name="uri">The uri<see cref="Uri"/></param>
        /// <returns>The <see cref="string"/></returns>
        [Obsolete("deprecated", true)]
        private static string GetPathAfterKitkat(Context context, Uri uri)
        {
            //是否大于Android4.4
            bool isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

            //如果是在文档
            if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
            {
                // 若为外部Sdcard存储文档
                if (IsExternalStorageDocument(uri))
                {
                    //若外部存储可访问，这里对常见的外部存储路径格式进行了适配
                    if (Environment.ExternalStorageState == Environment.MediaMounted || Environment.ExternalStorageState == Environment.MediaMountedReadOnly)
                    {
                        string docId = DocumentsContract.GetDocumentId(uri);
                        string[] split = docId.Split(":");

                        //系统自带的，但sdcard用系统判断的外部路径有些模拟器上会出错，如Android8
                        var path = context.GetExternalFilesDir(null).AbsolutePath + "/" + split[1];//Environment.ExternalStorageDirectory
                        Java.IO.File file = new Java.IO.File(path);
                        if (file.Exists())
                        {
                            return path;
                        }

                        //类似/storage/emulated/0/Android/data/my.app/files
                        path = "/storage/" + split[0] + "/" + split[1];
                        file = new Java.IO.File(path);
                        if (file.Exists())
                        {
                            return path;
                        }

                        //类似/storage/sdcard/Android/data/my.app/files；
                        path = "/storage/sdcard/" + split[1];
                        file = new Java.IO.File(path);
                        if (file.Exists())
                        {

                            return path;
                        }

                        //类似/sdcard/10E7-191B/Android/data/my.app/files
                        path = "/sdcard/" + split[0] + "/" + split[1];
                        file = new Java.IO.File(path);
                        if (file.Exists())
                        {
                            return path;
                        }

                        //这是最开始stackflow上的，不知何意
                        if ("primary".Equals(split[0].ToLower()))
                        {
                            return Environment.ExternalStorageDirectory + "/" + split[1];

                        }

                        file.Dispose();
                        return null;
                    }
                }
                // 若为下载文档
                else if (IsDownloadsDocument(uri))
                {
                    string id = DocumentsContract.GetDocumentId(uri);

                    if (!TextUtils.IsEmpty(id))
                    {
                        if (id.StartsWith("raw:"))
                        {
                            return id.Replace("raw:", "");
                        }
                        try
                        {
                            Uri contenturi = ContentUris.WithAppendedId(
                                    Uri.Parse("content://downloads/public_downloads"), long.Parse(id));
                            return GetDataColumn(context, contenturi, null, null);
                        }
                        catch (Java.Lang.NumberFormatException e)
                        {
                            Log.Error("FileUtils", "Downloads provider returned unexpected uri " + uri.ToString(), e);
                            return null;
                        }
                    }
                }
                // 若为媒体文档
                else if (IsMediaDocument(uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(":");
                    string type = split[0];

                    Uri contentUri = null;
                    if ("image".Equals(type))
                    {
                        contentUri = MediaStore.Images.Media.ExternalContentUri;

                    }
                    else if ("video".Equals(type))
                    {
                        contentUri = MediaStore.Video.Media.ExternalContentUri;

                    }
                    else if ("audio".Equals(type))
                    {
                        contentUri = MediaStore.Audio.Media.ExternalContentUri;

                    }

                    string selection = "_id=?";
                    string[] selectionArgs = new string[] { split[1] };

                    return GetDataColumn(context, contentUri, selection, selectionArgs);

                }

            }
            // MediaStore (and general)
            else if ("content".Equals(uri.Scheme.ToLower()))
            {
                return GetDataColumn(context, uri, null, null);

            }
            // File
            else if ("file".Equals(uri.Scheme))
            {
                return uri.Path;
            }
            return null;
        }


        /// <summary>
        /// The getDataColumn
        /// </summary>
        /// <param name="context">The context<see cref="Context"/></param>
        /// <param name="uri">The uri<see cref="Uri"/></param>
        /// <param name="selection">The selection<see cref="string"/></param>
        /// <param name="selectionArgs">The selectionArgs<see cref="string[]"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string GetDataColumn(Context context, Uri uri, string selection, string[] selectionArgs)
        {
            ICursor cursor = null;
            string column = "_data";
            string[] projection = { column };
            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs,
                        null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int column_index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(column_index);
                }
            }
            finally
            {
                if (cursor != null)
                    cursor.Close();
            }
            return null;
        }

        /// <summary>
        /// The getRealPathFromURI
        /// </summary>
        /// <param name="contentUri">The contentUri<see cref="Uri"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string GetPathBeforeKitkat(Context context, Uri contentUri)
        {
            string res = null;
            string[] proj = { MediaStore.Images.Media.ContentType };//这里ContentType原来是DATA，但Xamarin没有这个名字
            ICursor cursor = context.ContentResolver.Query(contentUri, proj, null, null, null);
            if (null != cursor && cursor.MoveToFirst())
            {
                int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.ContentType);
                res = cursor.GetString(column_index);
                cursor.Close();
            }
            return res;
        }

        /// <summary>
        /// 判断是DownLoads文件夹的的文件吗
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>The Uri to check</param>
        /// <returns><see cref="bool"/>Whether the Uri authority is DownloadsProvider</returns>
        private static bool IsDownloadsDocument(Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }

        /// <summary>
        /// 判断是外部存储文件吗和可访问性
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>The Uri to check</param>
        /// <returns>The <see cref="bool"/>Whether the Uri authority is ExternalStorageProvider</returns>
        private static bool IsExternalStorageDocument(Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }

        /// <summary>
        /// 判断是媒体文件吗
        /// </summary>
        /// <param name="uri">The uri<see cref="Uri"/>The Uri to check.</param>
        /// <returns>The <see cref="bool"/>Whether the Uri authority is MediaProvider.</returns>
        private static bool IsMediaDocument(Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }
    }

    public static partial class FileHelper
    {
        /// <summary>
        /// Assets中的文件只读
        /// <see href="https://www.jianshu.com/p/eb757835b6d9">Assets文件访问</see>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static Stream FromAssets(Context context, string assetName)
        {
            return context.Resources.Assets.Open(assetName);
        }

    }

}
#endif