namespace Yang.Maui.Helper.Files
{
    /// <summary>
    /// 由于各平台文件系统不同,Xamarin统一了部分访问,但还是让人糊涂.
    /// 此处进行解释,并对另外的存储进行统一管理
    /// </summary>
    public interface IFileStoreFolderService
    {
        /// <summary>
        /// App卸载后依旧能存在的数据.如选择保存的用户头像
        /// </summary>
        string PublicPersistentDataFolder { get; }

        /// <summary>
        /// App未卸载时一直存在的公开数据.UWP没有公开与私有之分<br/>
        /// iOS:app/Documents/ 可被iCloud备份.<br/>
        /// Android:/storage/emulated/0/Android/data/com.companyname.app/files.<br/>
        /// UWP:C:\\Users\\xtuzh\\AppData\\Local\\Packages\\ba682461-d557-45c1-b3fe-6b5da537c9d9_500jbr7t2ztpc\\LocalState.<br/>
        /// </summary>
        string AppPublicPersistentDataFolder { get; }

        /// <summary>
        /// App未卸载时一直存在的私有数据.UWP没有公开与私有之分<br/>
        /// iOS:app/Library/ 可被iCloud备份.<br/>
        /// Android:/data/user/0/com.companyname.app/files 在国外可被谷歌云盘备份.<br/>
        /// UWP:C:\\Users\\xtuzh\\AppData\\Local\\Packages\\ba682461-d557-45c1-b3fe-6b5da537c9d9_500jbr7t2ztpc\\LocalState.<br/>
        /// </summary>
        string AppPrivatePersistentDataFolder { get; }

        /// <summary>
        /// App产生的可以缓存以多次利用的私有数据,例如显示的用户头像
        /// iOS:app/library/Caches/
        /// Android:/data/user/0/com.companyname.app/cache
        /// UWP:C:\\Users\\xtuzh\\AppData\\Local\\Packages\\ba682461-d557-45c1-b3fe-6b5da537c9d9_500jbr7t2ztpc\\LocalCache
        /// </summary>
        string AppPrivateCachesFolder { get; }

        /// <summary>
        /// App产生的私有临时数据,如编辑头像
        /// iOS:app/temp/
        /// Android:/data/user/0/com.companyname.app/cache/temp. 添加了temp文件夹区分
        /// UWP:C:\\Users\\xtuzh\\AppData\\Local\\Packages\\ba682461-d557-45c1-b3fe-6b5da537c9d9_500jbr7t2ztpc\\TempState
        /// </summary>
        string AppPrivateTemporaryDataFolder { get; }
    }
}
