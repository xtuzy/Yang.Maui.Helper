#if __ANDROID__
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using AndroidX.Core.Graphics.Drawable;
using Java.IO;
using SkiaSharp;
using SkiaSharp.Views.Android;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using File = Java.IO.File;
using Path = System.IO.Path;
using Android.Views;
using Maui.Platform.Helper.Logs;

namespace Maui.Platform.Helper.Images
{
    /// <summary>
    /// 参考:https://www.jianshu.com/p/1d11522ed35e
    /// </summary>
    public static partial class ImageHelper
    {
        private static readonly string TAG = "ImageHelper";

        public static string SaveTo(Context context, Bitmap bitmap, string fileName, string folderPath)
        {
            if (bitmap == null) return null;

            var isSaved = true;
            //WARN:这里混用了Java.IO和System.IO
            /*首先创建文件夹*/
            var appDir = new File(folderPath);
            if (!appDir.Exists()) appDir.Mkdir(); //创建文件夹
            fileName = fileName + ".png";
            var file = new File(appDir, fileName);
            FileStream stream = null;
            try
            {
                var filePath = Path.Combine(folderPath, fileName);
                stream = new FileStream(filePath, FileMode.Create);
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);//质量压缩在不改变像素的大小的前提下,降低图像的质量,改变存储带线啊哦
            }
            catch (Exception e)
            {
                Toast.MakeText(context, "保存图片失败", ToastLength.Short);
                isSaved = false;
            }
            finally
            {
                if (stream != null)
                    try
                    {
                        stream.Close();
                        //回收
                        bitmap.Recycle();
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error("{0} {1}",TAG, "流释放出错");
                        isSaved = false;
                    }
            }

            if (isSaved)
            {
                return file.AbsolutePath;
            }
            return null;
        }

        /// <summary>
        ///      Google文档:读取位图尺寸和类型
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static BitmapFactory.Options GetInfoFromPath(string imagePath)
        {
            var options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true; //这个选项意味着不加载图像到内存
            BitmapFactory.DecodeFile(imagePath, options);
            return options;
        }

        /// <summary>
        /// 从资源读取图片,未对大图优化
        /// </summary>
        /// <param name="context"></param>
        /// <param name="drawableId"></param>
        /// <returns></returns>
        public static Bitmap FromResources(Context context, int drawableId)
        {
            return BitmapFactory.DecodeResource(context.Resources, drawableId);
        }

        /// <summary>
        /// 从文件读取图片,对大图优化
        /// Google文档:高效加载大型位图
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="reqWidth"></param>
        /// <param name="reqHeight"></param>
        /// <param name="sampleMethod">1为邻近采样(快速),2为线性采样(慢)</param>
        /// <returns></returns>
        public static async Task<Bitmap> FromPathAsyn(string imagePath, int reqWidth, int reqHeight, int sampleMethod)
        {
            //来自Google文档的高效加载大型位图
            // First decode with inJustDecodeBounds=true to check dimensions

            switch (sampleMethod)
            {
                case 1:
                    {
                        var options = new BitmapFactory.Options();
                        options.InJustDecodeBounds = true; //这个选项意味着不加载图像到内存
                        await BitmapFactory.DecodeFileAsync(imagePath, options);
                        // Calculate inSampleSize
                        options.InSampleSize = CalculateInSampleSizeByNearestNeighbour(options, reqWidth, reqHeight);

                        // Decode bitmap with inSampleSize set
                        options.InJustDecodeBounds = false;
                        return await BitmapFactory.DecodeFileAsync(imagePath, options);
                        break;
                    }
                case 2:
                    {
                        Bitmap bitmap = BitmapFactory.DecodeFile(imagePath);
                        Bitmap compress = Bitmap.CreateScaledBitmap(bitmap, bitmap.Width / reqWidth, bitmap.Height / reqHeight, true);
                        return compress;
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        /// <summary>
        /// 从文件加载图片，未对大图优化,可能OOM
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static Bitmap FromPath(string imagePath)
        {
            return BitmapFactory.DecodeFile(imagePath);
        }

        /// <summary>
        /// 参考:<see href="https://social.msdn.microsoft.com/Forums/en-US/3509325a-caf1-44f3-b2f2-00c1f7691648/image-from-url-in-imageview?forum=xamarinandroid">Image from Url in ImageView</see><br/>
        /// 从网络读取图片,未对大图优化
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Bitmap FromUrl(string url)
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);//这里肯定是加载到了内存
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }

        /// <summary>
        /// 从Assets读取文件,未对大图优化
        /// <see href="https://www.jianshu.com/p/eb757835b6d9">Assets文件访问</see>
        /// </summary>
        /// <returns></returns>
        public static Bitmap FromAssets(Context context, string assetName)
        {
            Bitmap bitmap = null;
            using (var stream = context.Resources.Assets.Open(assetName))
            {
                bitmap = BitmapFactory.DecodeStream(stream);
            }
            return bitmap;
        }

        public static Bitmap FromUrl(string url, int reqWidth, int reqHeight)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);//这里肯定是加载到了内存
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    var options = new BitmapFactory.Options();
                    options.InJustDecodeBounds = true; //这个选项意味着不加载图像到内存
                    BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length, options);//计算原图宽高
                    // Calculate inSampleSize
                    options.InSampleSize = CalculateInSampleSizeByNearestNeighbour(options, reqWidth, reqHeight);
                    // Decode bitmap with inSampleSize set
                    options.InJustDecodeBounds = false;
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }


        /// <summary>
        ///     对原来的位图采样:邻近采样
        /// </summary>
        /// <param name="options"></param>
        /// <param name="reqWidth"></param>
        /// <param name="reqHeight"></param>
        /// <returns></returns>
        private static int CalculateInSampleSizeByNearestNeighbour(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            var height = options.OutHeight;
            var width = options.OutWidth;
            var inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {
                var halfHeight = height / 2;
                var halfWidth = width / 2;

                // Calculate the largest inSampleSize value that is a power of 2 and keeps both
                // height and width larger than the requested height and width.
                while (halfHeight / inSampleSize >= reqHeight
                       && halfWidth / inSampleSize >= reqWidth)
                    inSampleSize = inSampleSize * 2;
            }
            return inSampleSize;
        }


#region 转换
        /// <summary>
        /// Bitmap to byte[]
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            using (MemoryStream baos = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, baos);
                return baos.ToArray();
            }
        }

        /// <summary>
        /// <see href="https://stackoverflow.com/questions/11613594/android-how-to-convert-byte-array-to-bitmap"/>
        /// </summary>
        /// <param name="rawBytes"></param>
        /// <param name="bmp"></param>
        public static void BytesToBitmap(byte[] rawBytes, Bitmap bmp)
        {
            BitmapFactory.Options options = new BitmapFactory.Options();
            bmp = BitmapFactory.DecodeByteArray(rawBytes, 0, rawBytes.Length, options); //Convert bytearray to bitmap
        }

        /// <summary>
        /// byte[] to bitmap
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] b)
        {
            if (b.Length != 0)
                return BitmapFactory.DecodeByteArray(b, 0, b.Length);
            else
                return null;
        }

        /// <summary>
        /// Bitmap to Drawable
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Drawable BitmapToDrawable(Context context, Bitmap bitmap)
        {
            BitmapDrawable drawbale = new BitmapDrawable(context.Resources, bitmap);
            return drawbale;
        }

        /// <summary>
        /// 图片加圆角
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="roundPx"></param>
        /// <returns></returns>
        public static Bitmap GetRoundCornerBitmap(Bitmap bitmap, float roundPx)
        {
            int width = bitmap.Width;
            int heigh = bitmap.Height;
            // 创建输出bitmap对象
            Bitmap outmap = Bitmap.CreateBitmap(width, heigh,
                    Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(outmap);
            int color = 0x424242;
            Paint paint = new Paint();
            Rect rect = new Rect(0, 0, width, heigh);
            RectF rectf = new RectF(rect);
            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            paint.Color = new Color(color);
            canvas.DrawRoundRect(rectf, roundPx, roundPx, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(bitmap, rect, rect, paint);
            return outmap;
        }

        /// <summary>
        /// Drawable To Bitmap
        /// </summary>
        /// <param name="drawable"></param>
        /// <returns></returns>
        public static Bitmap DrawableToBitmap(Drawable drawable)
        {
            // 获取 drawable 长宽
            int width = drawable.IntrinsicWidth;
            int heigh = drawable.IntrinsicHeight;

            drawable.SetBounds(0, 0, width, heigh);

            // 获取drawable的颜色格式
            Bitmap.Config config = drawable.Opacity != (int)Format.Opaque ? Bitmap.Config.Argb8888 : Bitmap.Config.Rgb565;
            // 创建bitmap
            Bitmap bitmap = Bitmap.CreateBitmap(width, heigh, config);
            // 创建bitmap画布
            Canvas canvas = new Canvas(bitmap);
            // 将drawable 内容画到画布中
            drawable.Draw(canvas);
            return bitmap;
        }

        /// <summary>
        /// 缩放Bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="heigh"></param>
        /// <returns></returns>
        public static Bitmap GetZoomBitmap(Bitmap bitmap, int width, int heigh)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;
            Matrix matrix = new Matrix();
            float scalewidth = (float)width / w;
            float scaleheigh = (float)heigh / h;
            matrix.PostScale(scalewidth, scaleheigh);
            Bitmap newBmp = Bitmap.CreateBitmap(bitmap, 0, 0, w, h, matrix, true);
            return newBmp;
        }

#endregion 转换

        /// <summary>
        /// <see href="https://www.jianshu.com/p/8c479ed24ca8">参考:如何修改Drawable变色</see>
        /// <para/>
        /// 单独只想改变当前颜色可以传入ColorStateList.ValueOf(Color).<br/>
        /// 改变不同状态的颜色需要创建ColorStateList.<see href="https://www.jianshu.com/p/15383739d083">参考:如何使用ColorStateList</see><br/>
        /// 1.读取定义不同状态颜色的Selector.xml.<br/>
        /// 2.创建<br/>
        ///  ColorStateList colorStateList = new ColorStateList(<br/>
        ///     new int[][]{{-android.R.attr.state_enable},{android.R.attr.state_enable}},<br/>
        ///     new int[] { Color.RED, Color.BLUE });
        /// </summary>
        /// <param name="drawable"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Drawable ChangeDrawableColor(Drawable drawable, ColorStateList colors)
        {
            Drawable wrappedDrawable = DrawableCompat.Wrap(drawable);
            DrawableCompat.SetTintList(wrappedDrawable, colors);
            return wrappedDrawable;
        }

        /// <summary>
        /// 得到bitmap的大小
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static int GetBitmapSize(Bitmap bitmap)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {    //API 19
                return bitmap.AllocationByteCount;
            }
            if (Build.VERSION.SdkInt >= BuildVersionCodes.HoneycombMr1)
            {//API 12
                return bitmap.ByteCount;
            }
            // 在低版本中用一行的字节x高度
            return bitmap.RowBytes * bitmap.Height;
        }


    }

    /// <summary>
    /// 参考:<see href="https://stackoverflow.com/questions/23860511/load-image-from-url-to-imageview-c-sharp">Load image from url to ImageView</see><br/>
    /// 使用:new DownloadImageTask(imgView , progressBar).Execute(uri)
    /// </summary>
    [Obsolete("deprecated", true)]
    public class DownloadImageTaskHelper : AsyncTask
    {
        private ImageView bmImage;
        private ProgressBar progressBar;


        public DownloadImageTaskHelper(ImageView bmImage, ProgressBar progressBar)
        {
            this.bmImage = bmImage;
            this.progressBar = progressBar;
        }

        [Obsolete("deprecated", true)]
        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            bmImage.SetImageBitmap((Bitmap)result);
            if (progressBar != null)
                progressBar.Visibility = ViewStates.Gone;
        }


        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
        {
            var urldisplay = @params[0].ToString();
            Bitmap mIcon11 = null;
            try
            {
                var req = WebRequest.Create(urldisplay);
                var response = req.GetResponse();
                var stream = response.GetResponseStream();

                mIcon11 = BitmapFactory.DecodeStream(stream);
            }
            catch (Exception e)
            {

            }
            return mIcon11;
        }
    }
}
#endif