using System;
using System.IO;
using UIKit;

namespace Yang.Maui.Helper.ViewUtils
{
    internal partial class CaptureViewImage
    {
        /// <summary>
        /// https://social.msdn.microsoft.com/Forums/en-US/34da10e0-1558-4c4e-a51b-84124e49afd8/how-to-convert-the-all-content-in-scroll-view-to-bitmap-in-xamarin-forum?forum=xamarinforms
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static UIImage GetUIImageFromView(UIView view)
        {
            UIGraphics.BeginImageContext(view.Frame.Size);
            view.DrawViewHierarchy(view.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }

        /// <summary>
        /// https://social.msdn.microsoft.com/Forums/en-US/34da10e0-1558-4c4e-a51b-84124e49afd8/how-to-convert-the-all-content-in-scroll-view-to-bitmap-in-xamarin-forum?forum=xamarinforms
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] UIImage2Bytes(UIImage image)
        {
            using (var imageData = image.AsJPEG(100))
            {
                var bytes = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
                return bytes;
            }
        }

        /// <summary>
        /// https://stackoverflow.com/a/21970657/13254773
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Stream UIImage2Stream(UIImage image)
        {
            return image.AsJPEG(100).AsStream();
        }
    }
}