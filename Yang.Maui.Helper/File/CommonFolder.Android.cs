using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.File
{
    /// <summary>
    /// 参考：
    /// https://github.com/JustDo23/FastDemo/blob/master/note/Android%20%E6%96%87%E4%BB%B6%E5%AD%98%E5%82%A8.md
    /// </summary>
    public partial class CommonFolder
    {
        /// <summary>
        /// /data/data/myapp/cache
        /// </summary>
        public static string AppCacheDir = Application.Context.CacheDir.AbsolutePath;
        /// <summary>
        /// /data/data/myapp/files
        /// </summary>
        public static string AppFilesDir = Application.Context.FilesDir.AbsolutePath;
        /// <summary>
        /// 1. 不需要权限
        /// 2. 私有文件
        /// /storage/emulated/0/Android/data/myapp/cache/
        /// </summary>
        public static string AppExternalCacheDir = Application.Context.GetExternalCacheDirs().FirstOrDefault().AbsolutePath;
        /// <summary>
        /// 1. 不需要权限
        /// 2. 私有文件
        /// /storage/emulated/0/Android/data/myapp/files/
        /// </summary>
        public static string AppExternalFilesDir = Application.Context.GetExternalFilesDir(null).AbsolutePath;
        /// <summary>
        /// 模仿iOS创建的临时文件夹
        /// </summary>
        public static string Tmp
        {
            get
            {
                var folderPath = System.IO.Path.Combine(Application.Context.CacheDir.AbsolutePath, "temp");
                if (System.IO.Directory.Exists(folderPath) is false) System.IO.Directory.CreateDirectory(folderPath); //创建文件夹
                return folderPath;
            }
        }
    }
}
