using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;

namespace Yang.Maui.Helper.Image
{
    using Microsoft.UI.Xaml.Controls;
    using System.Runtime.InteropServices.WindowsRuntime;

    public class ImageHelper
    {
        public static async Task<SoftwareBitmap> StreamToSoftwareBitmap(Stream stream)
        {
            Windows.Graphics.Imaging.BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
            return await decoder.GetSoftwareBitmapAsync();
        }

        /// <summary>
        /// https://stackoverflow.com/a/36140365/13254773
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static async Task<byte[]> ImageControlToBytesAsync(Image control)
        {
            var bitmap = new RenderTargetBitmap();
            await bitmap.RenderAsync(control);
            return (await bitmap.GetPixelsAsync()).ToArray();
        }

        /// <summary>
        /// https://stackoverflow.com/a/36140365/13254773
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] WriteableBitmapToBytes(WriteableBitmap bitmap)
        {
            return bitmap.PixelBuffer.ToArray();
        }

        /// <summary>
        /// https://stackoverflow.com/a/36140365/13254773
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static async Task<byte[]> StorageFileToBytesAsync(StorageFile bitmap)
        {
            var stream = await bitmap.OpenAsync(Windows.Storage.FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var pixels = await decoder.GetPixelDataAsync();
            return pixels.DetachPixelData();
        }

        /// <summary>
        /// https://stackoverflow.com/a/36140365/13254773
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static async Task<byte[]> RenderTargetBitmapToBytesAsync(RenderTargetBitmap bitmap)
        {
            return (await bitmap.GetPixelsAsync()).ToArray();
        }
    }
}