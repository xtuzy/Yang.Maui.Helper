using Microsoft.Maui.Storage;
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
        public static string AppLocalCache = FileSystem.CacheDirectory;
        public static string AppLocalState = FileSystem.AppDataDirectory;
        public static string TempState = Windows.Storage.ApplicationData.Current.TemporaryFolder.Path;
        public static string Documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);//C:\\Users\\myAccount\\Documents
    }
}
