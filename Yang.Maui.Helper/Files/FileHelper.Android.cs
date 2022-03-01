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
        /// from Assets get MemoryStream
        /// <see href="https://www.jianshu.com/p/eb757835b6d9">Assets name,need extension name</see>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assetName"></param>
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
        /// from Assets get stream
        /// <see href="https://www.jianshu.com/p/eb757835b6d9"></see>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static Stream FromAssets(Activity context, string assetName)
        {
            return context.Assets.Open(assetName);
        }

    }

}