using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Xamarin.Essentials
{
    public static partial class Screenshot
    {
        static bool PlatformIsCaptureSupported =>
            true;

        static async Task<ScreenshotResult> PlatformCaptureAsync()
        {
            using (var bitmap = CaptureScreen(0, 0, (int)DeviceDisplay.MainDisplayInfo.Width, (int)DeviceDisplay.MainDisplayInfo.Height))
            {
                var pixels = BitmapToBytes(bitmap);
                return new ScreenshotResult((int)DeviceDisplay.MainDisplayInfo.Width, (int)DeviceDisplay.MainDisplayInfo.Height, pixels);
            }
        }


        /// <summary>
        /// 屏幕截图
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
         static Bitmap CaptureScreen(int x, int y, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(x, y, 0, 0, new Size(width, height));
            //BitmapToBitmapSource方法，请参阅：https://huchengv5.gitee.io/post/Bitmap%E4%B8%8EBitmapSource%E7%9A%84%E4%BA%92%E8%BD%AC.html
            return bitmap;
        }

        /// <summary>
        /// 以当前dpi放大比例截屏
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
         static Bitmap CaptureScreenWithCurrentDpi(int x, int y, int width, int height)
        {
            //GetDpiRatio方法，请参阅：https://huchengv5.gitee.io/post/WPF-%E5%A6%82%E4%BD%95%E8%8E%B7%E5%8F%96%E7%B3%BB%E7%BB%9FDPI.html
            var ratio = DeviceDisplay.MainDisplayInfo.Density;
            return CaptureScreen(x, y, width / (int)ratio, height / (int)ratio);
        }

        static byte[] BitmapToBytes(Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, Bitmap.RawFormat);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }
    }

    public partial class ScreenshotResult
    {
        readonly byte[] bytes;
        public ScreenshotResult(int width, int height, byte[] pixels)
        {
            Width = width;
            Height = height;
            bytes = pixels;
        }

        internal async Task<Stream> PlatformOpenReadAsync(ScreenshotFormat format)
        {
            return new MemoryStream(bytes);
        }
    }
}
