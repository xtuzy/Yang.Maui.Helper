using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Skia.Canvas
{
    /// <summary>
    /// 参考Android和Flutter,方便转换
    /// </summary>
    public static class SKSizeExtension
    {
        /// <summary>
        /// Creates a square Size whose width and height are twice the given dimension.
        /// https://api.flutter.dev/flutter/dart-ui/Size-class.html
        /// </summary>
        /// <returns></returns>
        public static SKSize fromRadius(float radius)
        {
            var w = 2 * radius;
            return new SKSize(w, w);
        }

        /// <summary>
        /// Creates a square Size whose width and height are the given dimension.
        /// https://api.flutter.dev/flutter/dart-ui/Size-class.html
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public static SKSize square(float dimension)
        {
            return new SKSize(dimension, dimension);
        }
    }
}
