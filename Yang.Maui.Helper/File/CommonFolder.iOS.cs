using Foundation;
using System;
using System.IO;
using System.Linq;

namespace Yang.Maui.Helper.File
{
    /// <summary>
    /// 参考：
    /// 1. https://juejin.cn/post/6844903567195635720
    /// 2. https://blog.csdn.net/lyz0925/article/details/104460366
    /// 3. https://learn.microsoft.com/en-us/xamarin/ios/app-fundamentals/file-system
    /// </summary>
    public partial class CommonFolder
    {
        /// <summary>
        /// 您应该将所有的应用程序数据文件写入到这个目录下。这个目录用于存储用户数据。
        /// 1. 该路径可通过配置UIFileSharingEnabled(Application supports iTunes file sharing)共享文件让用户在iTunes访问
        /// 2. 该路径可通过配置LSSupportsOpeningDocumentsInPlace(Supports opening documents in place)让用户在Files应用中访问
        /// 3. 可被iTunes备份
        /// </summary>
        public static string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        /// <summary>
        /// 可创建子文件夹。可以用来放置您希望被备份但不希望被用户看到的数据。
        /// 1. 可被iTunes备份
        /// 2. 用户不可见
        /// </summary>
        public static string Library = Path.Combine(Documents, "..", "Library");
        /// <summary>
        /// 包含应用程序的偏好设置文件。您不应该直接创建偏好设置文件，而是应该使用NSUserDefaults类来取得和设置应用程序的偏好。
        /// 1. 可被iTunes备份
        /// 2. 用户不可见
        /// </summary>
        public static string LibraryPreferences = Path.Combine(Documents, "..", "Library", "Preferences");
        /// <summary>
        /// 用于存放应用程序专用的支持文件，保存应用程序再次启动过程中需要的信息。
        /// 1. 不被iTunes备份
        /// 2. 用户不可见
        /// </summary>
        public static string LibraryCaches = Path.Combine(Documents, "..", "Library", "Caches");
        /// <summary>
        /// 用于存放临时文件，保存应用程序再次启动过程中不需要的信息。
        /// 1. 不被iTunes备份
        /// </summary>
        public static string Tmp = Path.Combine(Documents, "..", "tmp");
        /// <summary>
        /// 当Documents中有文件不想被Files应用访问时，将这些文件存放在该目录替代Documents目录
        /// 参考：
        /// https://nemecek.be/blog/57/making-files-from-your-app-available-in-the-ios-files-app
        /// https://learn.microsoft.com/en-us/dotnet/api/foundation.nssearchpathdirectory?view=xamarin-ios-sdk-12
        /// https://monobook.org/wiki/Xamarin.Mac%E3%81%A7%E7%89%B9%E6%AE%8A%E3%83%87%E3%82%A3%E3%83%AC%E3%82%AF%E3%83%88%E3%83%AA%E3%81%AE%E3%83%91%E3%82%B9%E3%82%92%E5%8F%96%E5%BE%97%E3%81%99%E3%82%8B
        /// </summary>
        public static string ApplicationSupportDirectory
        {
            get
            {
                var applicationSupportURL = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User).FirstOrDefault();
                var isDir = true;
                if (!NSFileManager.DefaultManager.FileExists(applicationSupportURL.Path, ref isDir))
                {
                    //不存在时先创建
                    try
                    {
                        NSFileManager.DefaultManager.CreateDirectory(applicationSupportURL.Path, true, attributes: null);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }
                }
                return applicationSupportURL.Path;
            }
        }
    }
}
