#if  __MACOS__
using ShimSkiaSharp;
using SkiaSharp;
using SkiaSharp.Views.Mac;
using Svg.Skia;
using System;
using System.IO;
using System.Text;
using System.Xml;
using AppKit;

using SKPaint = SkiaSharp.SKPaint;
using SKPaintStyle = SkiaSharp.SKPaintStyle;
using SKPath = SkiaSharp.SKPath;

using Image = AppKit.NSImage;
namespace Xamarin.Helper.Images
{
    public static partial class SvgHelper
    {
        /// <summary>
        /// 从iOS项目中的Resources文件夹读取彩色Svg,注意该Svg需要在资源中声明
        /// </summary>
        /// <param name="svgName">如Add.svg全称</param>
        /// <param name="width">svg用来做图标,宽高相等,单位dp</param>
        /// <returns></returns>
        public static Image FromResources(string svgName, int width)
        {
            using (var svg = new SKSvg())
            {

                if (svg.Load(svgName) != null)//从Resources加载
                {
                    //scale<0放大,>0缩小
                    return svg.Picture.ToNSImage(svg.Picture.CullRect.Size.ToSizeI());//, svg.Picture.CullRect.Size.Width / width, NSImageOrientation.Up);
                }
            }
            return null;
        }

        /// <summary>
        /// 尝试从drawable.xml读取标准纯色图标.颜色可以被覆盖,如TabBar图标
        /// </summary>
        /// <param name="drawableXmlName">drawable名称,带后缀,请使用AndroidStudio导出,图标请使用正方形绘制</param>
        /// <param name="width">你要的px宽度</param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Image FromResources(string drawableXmlName, int width, SkiaSharp.SKColor color, bool isFill = true)
        {
            int w, h;
            StringBuilder pathDatas = new StringBuilder();
            using (var sr = File.Open(drawableXmlName, FileMode.Open))
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(sr);
                    var root = doc.FirstChild;
                    if (root.Name != "vector")
                        throw new Exception("Is not Drawable?");
                    w = int.Parse(root.Attributes["android:width"].Value.Split("dp")[0]);
                    h = int.Parse(root.Attributes["android:height"].Value.Split("dp")[0]);

                    foreach (XmlLinkedNode child in root.ChildNodes)
                    {
                        if (child.Name is "path")
                        {
                            pathDatas.Append(child.Attributes["android:pathData"].Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            if (pathDatas.Length == 0)
                return null;
            SkiaSharp.SKPath helloPath = SKPath.ParseSvgPathData(pathDatas.ToString());
            SKPaint paint;
            if (isFill)
            {
                paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    IsAntialias = true,
                    Color = color,
                    StrokeWidth = 1
                };
            }
            else
            {
                paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = color,
                    StrokeWidth = 1
                };
            }

            using (var bitmap = new SKBitmap(width, width))
            {
                using (SkiaSharp.SKCanvas canvas = new SkiaSharp.SKCanvas(bitmap))
                {

                    //这里不知道是否会轮廓不圆滑,尺寸是对了,绘制的话小图标性能应该没问题
                    float scale = width / w;
                    //参考:https://mono.github.io/SkiaSharp.Extended/api/svg/svg
                    var matrix = SkiaSharp.SKMatrix.CreateScale(scale, scale);
                    canvas.SetMatrix(matrix);
                    canvas.DrawPath(helloPath, paint);
                }
                return bitmap.ToNSImage();
            }

        }
    }
}
#endif
