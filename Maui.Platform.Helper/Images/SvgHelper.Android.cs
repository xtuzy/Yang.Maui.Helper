#if __ANDROID__
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using ShimSkiaSharp;
using SkiaSharp;
using SkiaSharp.Views.Android;
using Svg.Skia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SKPaint = SkiaSharp.SKPaint;
using SKPaintStyle = SkiaSharp.SKPaintStyle;
using SKPath = SkiaSharp.SKPath;

namespace Maui.Platform.Helper.Images
{
    public static partial class SvgHelper
    {
        /// <summary>
        /// 从Android的Assets文件夹读取彩色Svg
        /// </summary>
        /// <param name="context"></param>
        /// <param name="svgName">如Add.svg全称</param>
        /// <param name="width">svg用来做图标,宽高相等,单位为px,需要按照dp计算时请乘以密度传入</param>
        /// <returns></returns>
        public static Bitmap FromAssets(Context context, string svgName, int width)
        {
            using (var svg = new SKSvg())
            {
                AssetManager assets = context.Assets;
                using (StreamReader sr = new StreamReader(assets.Open(svgName)))
                {
                    if (svg.Load(sr.BaseStream) != null)
                    {
                        using (var bitmap = new SKBitmap(width, width))
                        {
                            using (SkiaSharp.SKCanvas canvas = new SkiaSharp.SKCanvas(bitmap))
                            {
                                //这里不知道是否会轮廓不圆滑,尺寸是对了,绘制的话小图标性能应该没问题
                                float scale = width / svg.Picture.CullRect.Width;
                                //参考:https://mono.github.io/SkiaSharp.Extended/api/svg/svg
                                var matrix = SkiaSharp.SKMatrix.CreateScale(scale, scale);
                                canvas.DrawPicture(svg.Picture, ref matrix);
                            }
                            return bitmap.ToBitmap();
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 尝试从drawable.xml读取标准纯色图标,注意测试是否能用.颜色设置在设置了ColorStateList时会被覆盖失效
        /// AndroidStudio导出的图标标准:<br/>
        /// 1. 不含<?xml version="1.0" encoding="utf-8"?><br/>
        /// 2. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="drawableXmlName">drawable名称,带后缀,请使用AndroidStudio导出,图标请使用正方形绘制</param>
        /// <param name="width">你要的px宽度</param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap FromAssets(Context context, string drawableXmlName, int width, SkiaSharp.SKColor color, bool isFill = true, int stroke = 1)
        {
            int w, h;
            StringBuilder pathDatas = new StringBuilder();
            AssetManager assets = context.Assets;
            using (StreamReader sr = new StreamReader(assets.Open(drawableXmlName)))
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(sr);
                    var root = doc.DocumentElement;
                    if(root.Name != "vector")
                        throw new Exception("Is not Drawable?");
                    w = int.Parse(root.Attributes["android:width"].Value.Split("dp")[0]);
                    h = int.Parse(root.Attributes["android:height"].Value.Split("dp")[0]);

                    foreach(XmlLinkedNode child in root.ChildNodes)
                    {
                        if(child.Name is "path")
                        {
                            pathDatas.Append(child.Attributes["android:pathData"].Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw  new Exception(ex.Message);
                }
            }

            if(pathDatas.Length == 0)
                return null;
            SkiaSharp.SKPath helloPath = SKPath.ParseSvgPathData(pathDatas.ToString());
            SKPaint paint;
            if (isFill)
            {
                paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    IsAntialias=true,
                    Color = color,
                    StrokeWidth = stroke
                };
            }
            else
            {
                paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = color,
                    StrokeWidth = stroke
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
                return bitmap.ToBitmap();
            }
            
        }
    }
}
#endif