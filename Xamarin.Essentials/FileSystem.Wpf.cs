using System;
using System.Threading.Tasks;
using IO = System.IO;

namespace Xamarin.Essentials
{
    /// <summary>
    /// copy from 
    /// https://github.com/xamarin/Essentials/tree/aab19bb928332e737c6957e8df45c1c7f2e722cb/Xamarin.Essentials
    /// </summary>
    public static partial class FileSystem
    {
        static string PlatformCacheDirectory
            => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        static string PlatformAppDataDirectory
            => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        static Task<IO.Stream> PlatformOpenAppPackageFileAsync(string filename)
             => Task.FromResult<IO.Stream>(IO.File.OpenRead(IO.Path.Combine(Environment.CurrentDirectory, filename)));
    }

    public partial class FileBase
    {
        internal void PlatformInit(FileBase file)
        {
            ContentType = PlatformGetContentType(file.FullPath);
        }

        static string PlatformGetContentType(string extension)
        {
            //https://stackoverflow.com/questions/34131326/using-mimemapping-in-asp-net-core
           // return System.Web.MimeMapping.GetMimeMapping(extension);
            return MimeTypes.GetMimeType(extension);
        }

        internal virtual Task<IO.Stream> PlatformOpenReadAsync()
        {
            var stream = IO.File.OpenRead(FullPath);
            return Task.FromResult<IO.Stream>(stream);
        }
    }
}
