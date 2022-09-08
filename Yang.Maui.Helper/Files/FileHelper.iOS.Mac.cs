#if __IOS__ || __MACOS__
using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if __IOS__
using UIKit;
#else
using AppKit;
#endif
namespace Yang.Maui.Helper.Files
{
    public static partial class FileHelper
    {
        /// <summary>
        /// At not Maui project.
        /// From Assets get stream. Android is Assets Foler, iOS is Resources Folder.
        /// </summary>
        /// <param name="name">Assets name,need extension name</param>
        /// <returns></returns>
        public static FileStream FromAssets(string name)
        {
            var path = NSBundle.MainBundle.PathForResource(name, null);
            return File.Open(path, FileMode.Open);
        }
    }
}
#endif