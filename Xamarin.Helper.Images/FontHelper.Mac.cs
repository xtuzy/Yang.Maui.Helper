#if __MACOS__
using CoreGraphics;
using Foundation;

using System;
using AppKit;

using UIImage = AppKit.NSImage;
using UIScreen = AppKit.NSScreen;
using UIGraphics = AppKit.NSGraphics;
using UIColor = AppKit.NSColor;
using UIFont = AppKit.NSFont;

using UIStringAttributes = AppKit.NSStringAttributes;
using Xamarin.Helper.Logs;

namespace Xamarin.Helper.Images
{
    public static partial class FontHelper
    {

        /// <summary>
        /// TODO
        /// 从ttf字体文件获取Icon,大小为设计时取的dp大小,会自动计算符合设备的像素大小
        /// 注意ttf存储在Resources,属性设置BoundResource,还需要在info.plist的Fonts provided by application处添加
        /// 参考:https://blog.csdn.net/ZHFDBK/article/details/105667511
        /// </summary>
        /// <param name="fontName">安装后的字体名,不是字体文件名</param>
        /// <param name="iconName"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        /*public static UIImage GetImage(string fontName, string iconName, float size, UIColor color)
        {
            var scale = UIScreen.MainScreen.BackingScaleFactor;//px/dp比例
            var realSize = size * scale;
            var font = GetFont(fontName, realSize);
            UIGraphics.BeginImageContext(new CGSize(width: realSize, height: realSize));
            var textStr = new NSString(iconName);
            if (textStr.RespondsToSelector(new ObjCRuntime.Selector("drawAtPoint:withAttributes:")))
            {
                textStr.DrawString(CGPoint.Empty, new UIStringAttributes() { Font = font, ForegroundColor = color });
            }
            var image = new UIImage();
            var cgimage = UIGraphics.GetImageFromCurrentImageContext();
            if (cgimage != null)
            {
                image = new UIImage(cgimage.CGImage, realSize);
            }
            UIGraphics.EndImageContext();
            return image;
        }*/

        /// <summary>
        /// 用于Lable
        /// </summary>
        /// <param name="fontName">安装后的字体名,不是字体文件名</param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static UIFont GetFont(string fontName, nfloat size)
        {
            var font = UIFont.FromFontName(fontName, size);
            if (font == null)
            {
                LogHelper.Debug(nameof(FontHelper), nameof(GetFont));

            }
            return font != null ? font : UIFont.SystemFontOfSize(size);
        }
    }
}
#endif