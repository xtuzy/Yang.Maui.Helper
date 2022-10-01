using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.ViewUtils
{
    using Microsoft.UI.Xaml.Media;
    using System.Drawing;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Windows.Graphics.Imaging;

    internal partial class CaptureViewImage
    {
        public static async Task<RenderTargetBitmap> GetImageFormViewAsync(Control element)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(element);
            return renderTargetBitmap;
        }

        /// <summary>
        /// https://stackoverflow.com/a/36140365/13254773
        /// </summary>
        /// <param name="encoder"></param>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        public static async Task<byte[]> ImageSourceToBytesAsync(RenderTargetBitmap imageSource)
        {
            var bytes = (await imageSource.GetPixelsAsync()).ToArray();
            return bytes;
        }
    }
}