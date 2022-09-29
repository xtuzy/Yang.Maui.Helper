using Microsoft.Maui.Storage;
using System;
#if __IOS__ || __MACOS__
using System.IO;
#elif   __ANDROID__
using Android.App;
#elif WINDOWS
#endif
namespace Yang.Maui.Helper.File
{
    /// <summary>
    /// 由于各平台文件系统不同,Xamarin统一了部分访问,但还是让人糊涂.
    /// 此处进行解释,并对另外的存储进行统一管理
    /// </summary>
    public partial class CommonFolder
    {
        /// <summary>
        /// App卸载后依旧能存在的数据.如选择保存的用户头像
        /// </summary>
        public static string PublicPersistentDataFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                throw new NotImplementedException();
#elif __ANDROID__
                throw new NotImplementedException();
#elif WINDOWS
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal);//C:\\Users\\me\\Documents
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// App未卸载时一直存在的公开数据.UWP没有公开与私有之分<br/>
        /// iOS:app/Documents/ 可被iCloud备份.<br/>
        /// Android:/storage/emulated/0/Android/data/com.companyname.app/files.<br/>
        /// UWP:C:\\Users\\xtuzh\\AppData\\Local\\Packages\\ba682461-d557-45c1-b3fe-6b5da537c9d9_500jbr7t2ztpc\\LocalState.<br/>
        /// </summary>
        public static string AppPublicPersistentDataFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif __ANDROID__
                return Application.Context.GetExternalFilesDir(null).AbsolutePath;
#elif WINDOWS
                return FileSystem.AppDataDirectory;
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// App未卸载时一直存在的私有数据.UWP没有公开与私有之分<br/>
        /// iOS:app/Library/ 可被iCloud备份.<br/>
        /// Android:/data/user/0/com.companyname.app/files 在国外可被谷歌云盘备份.<br/>
        /// UWP:C:\\Users\\xtuzh\\AppData\\Local\\Packages\\ba682461-d557-45c1-b3fe-6b5da537c9d9_500jbr7t2ztpc\\LocalState.<br/>
        /// </summary>
        public static string AppPrivatePersistentDataFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library");
#elif __ANDROID__
                return Application.Context.FilesDir.AbsolutePath;
#elif WINDOWS
                return FileSystem.AppDataDirectory;
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// App产生的可以缓存以多次利用的私有数据,例如显示的用户头像
        /// iOS:app/library/Caches/
        /// Android:/data/user/0/com.companyname.app/cache
        /// UWP:C:\\Users\\xtuzh\\AppData\\Local\\Packages\\ba682461-d557-45c1-b3fe-6b5da537c9d9_500jbr7t2ztpc\\LocalCache
        /// </summary>
        public static string AppPrivateCachesFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", "Caches");
#elif __ANDROID__
                return Application.Context.CacheDir.AbsolutePath;
#elif WINDOWS
                return FileSystem.CacheDirectory;
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// App产生的私有临时数据,如编辑头像
        /// iOS:app/temp/
        /// Android:/data/user/0/com.companyname.app/cache/temp. 添加了temp文件夹区分
        /// UWP:C:\\Users\\xtuzh\\AppData\\Local\\Packages\\ba682461-d557-45c1-b3fe-6b5da537c9d9_500jbr7t2ztpc\\TempState
        /// </summary>
        public static string AppPrivateTemporaryDataFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "tmp");
#elif __ANDROID__
                var folderPath = System.IO.Path.Combine(Application.Context.CacheDir.AbsolutePath, "temp");
                if (System.IO.Directory.Exists(folderPath) is false) System.IO.Directory.CreateDirectory(folderPath); //创建文件夹
                return folderPath;
#elif WINDOWS
                return Windows.Storage.ApplicationData.Current.TemporaryFolder.Path;
#else
                throw new NotImplementedException();
#endif
            }
        }
    }
}
