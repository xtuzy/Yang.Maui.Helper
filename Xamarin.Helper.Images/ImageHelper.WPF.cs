#if __WPF__ 
using System;
using System.IO;
using System.Threading.Tasks;


namespace Xamarin.Helper.Images
{
    public static class ImageHelper
    {
       /* public static async Task<SoftwareBitmap> StreamToSoftwareBitmap(Stream stream)
        {
            Windows.Graphics.Imaging.BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
            return await decoder.GetSoftwareBitmapAsync();
        }*/
    }
}
#endif
