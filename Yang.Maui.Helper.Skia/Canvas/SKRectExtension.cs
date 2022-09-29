using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
namespace Yang.Maui.Helper.Skia.Canvas
{
    public static class SKRectExtension
    {
        public static SKRect FormCenter(SKPoint center, float width, float height)
        {
           return  SKRect.Create(center.X - width / 2, center.Y - height / 2, width, height);
        }
    }
}
