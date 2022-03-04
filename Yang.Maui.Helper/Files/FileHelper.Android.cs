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

namespace Yang.Maui.Helper.Files
{

    public static partial class FileHelper
    {
        /// <summary>
        /// From Assets get MemoryStream
        /// <see href="https://www.jianshu.com/p/eb757835b6d9">Assets name,need extension name</see>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assetName">Assets name,need extension name</param>
        /// <returns></returns>
        public static MemoryStream ReadMemoryStreamFromAssets(Activity context, string assetName)
        {
            var ms = new MemoryStream();
            using (var s = context.Assets.Open(assetName))
            {
                s.CopyTo(ms);
                return ms;
            }
        }

        /// <summary>
        /// From Assets get stream. Android is Assets Foler, iOS is Resources Folder.
        /// <see href="https://www.jianshu.com/p/eb757835b6d9"></see>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assetName">Assets name,need extension name</param>
        /// <returns></returns>
        public static Stream FromAssets(Activity context, string assetName)
        {
            return context.Assets.Open(assetName);
        }

    }

}