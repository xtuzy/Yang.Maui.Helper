
#if __ANDROID__
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Maui.Platform.Helper.Logs;
using Bitmap = Android.Graphics.Bitmap;

namespace Maui.Platform.Helper.Images
{
    public static partial class FontHelper
    {
        /// <summary>
        /// 从Resources/font获取字体图片(Api>26)
        /// </summary>
        /// <param name="context">Activity或Application</param>
        /// <param name="fontId">Resources.Font.fontName</param>
        /// <param name="iconName">查找FontAwesome图标对应的文字</param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap GetImage(Context context, int fontId, string iconName, int size, Color color)
        {
            using (Bitmap myBitmap = Bitmap.CreateBitmap(size, size, Bitmap.Config.Argb8888))
            {
                using (Canvas myCanvas = new Canvas(myBitmap))
                {
                    Paint paint = new Paint()
                    {
                        AntiAlias = true,
                        SubpixelText = true,
                        Color = Color.White,
                        TextSize = size - 4//按照24作图大小,icon会留有2空白
                    };
                    paint.SetTypeface(GetFont(context, fontId));
                    paint.SetStyle(Paint.Style.Fill);
                    myCanvas.DrawText(iconName, 0, 20, paint);
                }
                return myBitmap;
            }
        }

        /// <summary>
        /// 从Assets获取字体图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fontName"></param>
        /// <param name="iconName"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap GetImage(Context context, string fontName, string iconName, int size, Color color)
        {
            using (Bitmap myBitmap = Bitmap.CreateBitmap(size, size, Bitmap.Config.Argb8888))
            {
                using (Canvas myCanvas = new Canvas(myBitmap))
                {
                    Paint paint = new Paint()
                    {
                        AntiAlias = true,
                        SubpixelText = true,
                        Color = Color.White,
                        TextSize = size - 4//按照24作图大小,icon会留有2空白
                    };
                    paint.SetTypeface(GetFont(context, fontName));
                    paint.SetStyle(Paint.Style.Fill);
                    myCanvas.DrawText(iconName, 0, 20, paint);
                }
                return myBitmap;
            }
        }

        /// <summary>
        /// 从Resources/font获取字体(Api>26)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fontId">存储在Resources/font文件夹下的字体文件Id</param>
        /// <returns></returns>
        public static Typeface GetFont(Context context, int fontId)
        {
            var font = context.Resources.GetFont(fontId);
            if (font == null)
            {
                LogHelper.Debug("{0} {1}",nameof(FontHelper), nameof(GetFont));

            }
            return font != null ? font : Typeface.Default;
        }

        /// <summary>
        /// 从Assets获取字体
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static Typeface GetFont(Context context, string fontName)
        {
            Typeface font = Typeface.CreateFromAsset(context.Assets, fontName);
            if (font == null)
            {
                LogHelper.Debug("{0} {1}",nameof(FontHelper), nameof(GetFont));
            }
            return font != null ? font : Typeface.Default;
        }
    }
}
#endif
