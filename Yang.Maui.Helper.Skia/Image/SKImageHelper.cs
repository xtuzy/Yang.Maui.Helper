using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace Yang.Maui.Helper.Skia.Image
{
    public static partial class SKImageHelper
    {
        /// <summary>
        /// 利用Skiasharp从网络加载图片,直接解码,适合小图
        /// 参考<see href="https://stackoverflow.com/questions/63201621/skiasharp-loan-image-from-url-and-draw-on-canvas-in-xamarin-forms">Skiasharp loan image from URL and draw on Canvas in Xamarin Forms</see>
        /// </summary>
        /// <param name="url"></param>
        public static SKBitmap FromUrl(string url)
        {
            SKBitmap resourceBitmap = null;
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            request.Timeout = 2000; // miliseconds

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK) //Make sure the URL is not empty and the image is there
                {
                    // download the bytes
                    byte[] stream = null;
                    using (var webClient = new WebClient())
                    {
                        stream = webClient.DownloadData(url);
                    }

                    // decode the bitmap stream
                    resourceBitmap = SKBitmap.Decode(stream);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                // Don't forget to close your response.
                if (response != null)
                {
                    response.Close();
                }
            }
            return resourceBitmap;
        }

        /// <summary>
        /// <see cref="FromUrl(string)"/>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static SKBitmap FromUrl(string url, int w, int h)
        {
            using (var bitmap = new SKBitmap(w, h))
            {
                using (var canvas = new SKCanvas(bitmap))
                {
                    using (var image = FromUrl(url))
                    {
                        if (image != null)
                        {
                            var resizedBitmap = image.Resize(bitmap.Info, SKFilterQuality.High); //Resize to the canvas
                            canvas.DrawBitmap(resizedBitmap, 0, 0);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return bitmap;
            }
        }


        /// <summary>
        /// 从流加载图片,内存优化
        /// <see href="https://github.com/mono/SkiaSharp/issues/236">How to decode and scale down a png image?</see>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="requestWidth"></param>
        /// <param name="requestHeight"></param>
        /// <returns></returns>
        public static SKBitmap FromStream(Stream stream, int requestWidth, int requestHeight)
        {
            try
            {
                SKCodec codec = SKCodec.Create(stream);
                SKImageInfo info = codec.Info;
                var scale = info.Width / requestWidth <= info.Height / requestHeight ? info.Width / requestWidth : info.Height / requestHeight;
                var size = codec.GetScaledDimensions(scale);
                var bitmap = SKBitmap.Decode(codec, new SKImageInfo(size.Width, size.Height));
                return bitmap;
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Load PlatformImage ERROR:" + Ex.Message);
            }
            return null;
        }
    }
}
