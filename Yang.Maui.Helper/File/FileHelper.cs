﻿using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.File
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
                //LogHelper.Error("{0} {1}", "FileHelper.SaveTo ", "写入stream出错");
                if (stream != null)
                    stream.Close();//关闭流
                throw;
            }
            if (stream != null)
                stream.Close();//关闭流
            return true;
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
                //LogHelper.Error("{0} {1}", "FileHelper.SaveTo ", "写入bytes出错");
                if (stream != null)
                    stream.Close();//关闭流
                throw;
            }

            if (stream != null)
                stream.Close();//关闭流
            return true;
        }

        /// <summary>
        /// 从Maui项目的Resources/Fonts文件夹读取字体文件, 其读取实际和Raw文件夹一样
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static Task<Stream> LoadMauiFont(string fileFullName)
        {
            return FileSystem.OpenAppPackageFileAsync(fileFullName);
        }
    }
}