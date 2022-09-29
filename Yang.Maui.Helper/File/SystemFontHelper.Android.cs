using Android.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.File
{
    public class SystemFontHelper
    {
        /// <summary>
        /// 返回所有系统字体的路径
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSystemFontList()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
            {
                return Android.Graphics.Fonts.SystemFonts.AvailableFonts.Select((f)=>f.File.Path);
            }
            else
            {
                //https://stackoverflow.com/questions/3532397/how-to-retrieve-a-list-of-available-installed-fonts-in-android
                String path = "/system/fonts";
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                return files.Select((f) => f.FullName);
            }
        }
    }
}
