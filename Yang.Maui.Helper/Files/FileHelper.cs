
using System;
using System.IO;
using Yang.Maui.Helper.Logs;

namespace Yang.Maui.Helper.Files
{
    public static partial class FileHelper
    {

        /// <summary>
        /// 保存数据流到文件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bitmap"></param>
        /// <param name="fileName">包括后缀</param>
        /// <param name="folderPath">文件夹路径</param>
        public static bool SaveTo(Stream s, string fileName, string folderPath)
        {
            if (s is null) return false;
            var isSaved = true;
            /*首先检测或创建文件夹与文件*/
            if (Directory.Exists(folderPath) is false) Directory.CreateDirectory(folderPath); //创建文件夹
            var filePath = Path.Combine(folderPath, fileName);
            //if (Directory.Exists(folderPath) is false) File.Create(filePath).Dispose();//创建文件

            FileStream stream = null;
            try
            {
                stream = new FileStream(filePath, FileMode.OpenOrCreate);
                s.Seek(0, SeekOrigin.Begin);
                s.CopyTo(stream);
            }
            catch (Exception e)
            {
                LogHelper.Error("{0} {1}", "FileHelper.SaveTo ", "写入stream出错");
                isSaved = false;
            }
            finally
            {
                if (stream != null)
                    try
                    {
                        stream.Close();//关闭流
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("{0} {1}", "FileHelper.SaveTo ", "文件流释放出错");
                        isSaved = false;
                    }
            }
            return isSaved;
        }

        /// <summary>
        /// 保存bytes数据到文件
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fileName"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static bool SaveTo(byte[] s, string fileName, string folderPath)
        {
            if (s is null) return false;
            var isSaved = true;
            /*首先检测或创建文件夹与文件*/
            if (Directory.Exists(folderPath) is false) Directory.CreateDirectory(folderPath); //创建文件夹
            var filePath = Path.Combine(folderPath, fileName);
            //if (Directory.Exists(folderPath) is false) File.Create(filePath).Dispose();//创建文件

            FileStream stream = null;
            try
            {
                stream = new FileStream(filePath, FileMode.OpenOrCreate);
                stream.Write(s, 0, s.Length);
            }
            catch (Exception e)
            {
                LogHelper.Error("{0} {1}", "FileHelper.SaveTo ", "写入bytes出错");
                isSaved = false;
            }
            finally
            {
                if (stream != null)
                    try
                    {
                        stream.Close();//关闭流
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("{0} {1}", "FileHelper.SaveTo ", "文件流释放出错");
                        isSaved = false;
                    }
            }
            return isSaved;
        }

        /// <summary>
        /// From Assets get stream. Android is Assets Foler, iOS is Resources Folder.
        /// <see href="https://www.jianshu.com/p/eb757835b6d9"></see>
        /// </summary>
        /// <param name="assetName">Assets name,need extension name</param>
        /// <param name="activty"> Android need.</param>
        /// <returns></returns>
        public static Stream FromAssets(string assetName, object activity = null)
        {
#if __ANDROID__
            return FromAssets(activity as Activity,assetName);
#elif __IOS__
             return FromAssets(assetName);
#else
            throw new NotImplementedException();
#endif
        }
    }

}