#if IOS || MACCATALYST
using SkiaSharp.Views.iOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Yang.Maui.Helper.Skia.Image
{
    public static partial class ImageHelper
    {
        /// <summary>
        /// 通过Skiasharp从网络获取图像
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static UIImage FromUrl(string url)
        {
            using (var image = SKImageHelper.FromUrl(url))
            {
                if (image != null)
                {
#if __IOS__
                    return image.ToUIImage();
#else
                    return image.ToNSImage();
#endif
                }

                return null;
            }
        }

        /// <summary>
        /// 通过Skiasharp从网络获取图像
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestWidth"></param>
        /// <param name="requestHeight"></param>
        /// <returns></returns>
        public static UIImage FromUrl(string url, int requestWidth, int requestHeight)
        {
            using (var image = SKImageHelper.FromUrl(url, requestWidth, requestHeight))
            {
                if (image != null)
                {
#if __IOS__
                    return image.ToUIImage();
#else
                    return image.ToNSImage();
#endif
                }
                return null;
            }
        }
    }
}
#endif