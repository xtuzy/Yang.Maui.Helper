using System;
#if __IOS__ || __MACOS__
using System.IO;
#elif   __ANDROID__
using Android.App;
#elif __WPF__
#endif
namespace Xamarin.Helper.Files
{
    /// <summary>
    /// 由于各平台文件系统不同,Xamarin统一了部分访问,但还是让人糊涂.
    /// 此处进行解释,并对另外的存储进行统一管理
    /// </summary>
    public class FileStoreFolderService : IFileStoreFolderService
    {

        public string PublicPersistentDataFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                throw new NotImplementedException();
#elif __ANDROID__
                throw new NotImplementedException();
#elif __UWP__
                throw new NotImplementedException();
#elif __WPF__ || __CONSOLE__
               return Environment.GetFolderPath(Environment.SpecialFolder.Personal);//C:\\Users\\me\\Documents
#endif
            }
        }


        public string AppPublicPersistentDataFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#elif __ANDROID__
                return Application.Context.GetExternalFilesDir(null).AbsolutePath;
#elif __UWP__
                return Xamarin.Essentials.FileSystem.AppDataDirectory;
#elif __WPF__ || __CONSOLE__
                throw new NotImplementedException();
#endif
            }
        }


        public string AppPrivatePersistentDataFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library");
#elif __ANDROID__
                return Application.Context.FilesDir.AbsolutePath;
#elif __UWP__
                return Xamarin.Essentials.FileSystem.AppDataDirectory;
#elif __WPF__ || __CONSOLE__
                throw new NotImplementedException();
#endif
            }
        }


        public string AppPrivateCachesFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", "Caches");
#elif __ANDROID__
                return Application.Context.CacheDir.AbsolutePath;
#elif __UWP__
                return Xamarin.Essentials.FileSystem.CacheDirectory;
#elif __WPF__ || __CONSOLE__
                throw new NotImplementedException();
#endif
            }
        }


        public string AppPrivateTemporaryDataFolder
        {
            get
            {
#if __IOS__ || __MACOS__
                return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "tmp");
#elif __ANDROID__
                var folderPath = System.IO.Path.Combine(Application.Context.CacheDir.AbsolutePath, "temp");
                if (System.IO.Directory.Exists(folderPath) is false) System.IO.Directory.CreateDirectory(folderPath); //创建文件夹
                return folderPath;
#elif __UWP__
                return Windows.Storage.ApplicationData.Current.TemporaryFolder.Path;
#elif __WPF__ || __CONSOLE__
                throw new NotImplementedException();
#endif
            }
        }
    }
}
