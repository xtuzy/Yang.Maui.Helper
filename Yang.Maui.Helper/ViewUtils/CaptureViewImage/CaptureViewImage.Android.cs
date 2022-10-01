using Android.Graphics;

namespace Yang.Maui.Helper.ViewUtils
{
    using Android.Graphics.Drawables;
    using Android.Views;
    using Android.Widget;
    using Microsoft.Maui.Graphics;
    using System.IO;
    using Color = Android.Graphics.Color;

    internal partial class CaptureViewImage
    {
        /// <summary>
        /// https://stackoverflow.com/questions/3609297/android-total-height-of-scrollview
        /// </summary>
        /// <param name="scrollView"></param>
        /// <returns></returns>
        public static Size GetScrollViewTotalSize(ScrollView scrollView)
        {
            var view = scrollView.GetChildAt(0);
            return new Size(view.Width, view.Height);
        }

        /// <summary>
        /// Copy From:https://stackoverflow.com/questions/8738448/how-to-convert-all-content-in-a-scrollview-to-a-bitmap/19383606#19383606
        /// 也可参考：https://social.msdn.microsoft.com/Forums/en-US/34da10e0-1558-4c4e-a51b-84124e49afd8/how-to-convert-the-all-content-in-scroll-view-to-bitmap-in-xamarin-forum?forum=xamarinforms
        /// </summary>
        /// <param name="view"></param>
        /// <param name="totalViewHeight"></param>
        /// <param name="totalViewWidth"></param>
        /// <returns></returns>
        public static Bitmap GetBitmapFromView(View view, int totalViewHeight, int totalViewWidth)
        {
            Bitmap returnedBitmap = Bitmap.CreateBitmap(totalViewWidth, totalViewHeight, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(returnedBitmap);
            Drawable bgDrawable = view.Background;
            if (bgDrawable != null)
                bgDrawable.Draw(canvas);
            else
                canvas.DrawColor(Color.White);
            view.Draw(canvas);
            return returnedBitmap;
        }

        public static byte[] Bitmap2Bytes(Bitmap bitmap)
        {
            using (MemoryStream baos = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, baos);
                return baos.ToArray();
            }
        }

        public static void Bitmap2Stream(Bitmap bitmap, Stream stream)
        {
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
        }
    }
}