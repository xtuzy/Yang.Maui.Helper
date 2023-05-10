using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Yang.Maui.Helper.Graphics
{
    /// <summary>
    /// Maui拥有自带的FontManager, 其比Graphics的Font的支持更好, 这个类通过FontManager去获取平台字体, 以在ICanvas上使用.
    /// </summary>
    public class MauiFont : IDisposable
    {
        /// <summary>
        /// 需要通过View去获取Maui的FontManager
        /// </summary>
        View View;
        /// <summary>
        /// Maui.Font不是实际的字体, 我们可以随意的处置它.
        /// </summary>
        public Microsoft.Maui.Font VirtualFont { get; private set; }
#if WINDOWS
        Microsoft.UI.Xaml.Media.FontFamily PlatformFont;
        public Microsoft.UI.Xaml.Media.FontFamily GetPlatformFont()
        {
            if (PlatformFont == null)
            {
                var fontManager = View?.Handler?.MauiContext.Services.GetService<IFontManager>();
                PlatformFont = fontManager?.GetFontFamily(VirtualFont);
            }
            return PlatformFont;
        }
#elif IOS || MACCATALYST
        UIKit.UIFont PlatformFont;
        Dictionary<float, CoreText.CTFont> PlatformFontCache = new Dictionary<float, CoreText.CTFont>();
        float lastRequestSize = 0;
        public UIKit.UIFont GetPlatformFont_UIFont(float defaultFontSize = 0)
        {
            if (PlatformFont == null || lastRequestSize != defaultFontSize)
            {
                var fontManager = View?.Handler?.MauiContext.Services.GetService<IFontManager>();
                PlatformFont = fontManager?.GetFont(VirtualFont, defaultFontSize);//不缓存UIFont, 假定Maui框架自身会缓存
                lastRequestSize = defaultFontSize;
            }
            return PlatformFont;
        }

        public CoreText.CTFont GetPlatformFont_CTFont(float defaultFontSize = 0)
        {
            if (PlatformFont == null)
            {
                var fontManager = View?.Handler?.MauiContext.Services.GetService<IFontManager>();
                PlatformFont = fontManager?.GetFont(VirtualFont, defaultFontSize);//不缓存UIFont, 假定Maui框架自身会缓存
                lastRequestSize = defaultFontSize;
            }

            if (PlatformFontCache.ContainsKey(defaultFontSize))
            {
                return PlatformFontCache[defaultFontSize];
            }
            else
            {
                var newFont = new CoreText.CTFont(PlatformFont?.Name, defaultFontSize, CoreText.CTFontOptions.Default);
                PlatformFontCache.Add(defaultFontSize, newFont);
                return newFont;
            }
        }
#elif ANDROID
        Android.Graphics.Typeface PlatformFont;
        public Android.Graphics.Typeface GetPlatformFont()
        {
            if (PlatformFont == null)
            {
                var fontManager = View?.Handler?.MauiContext.Services.GetService<IFontManager>();
                PlatformFont = fontManager?.GetTypeface(VirtualFont);
            }
            return PlatformFont;
        }
#else
        object PlatformFont;
#endif

        public MauiFont(View view, Microsoft.Maui.Font font)
        {
            this.View = view;
            this.VirtualFont = font;
        }

        public void Dispose()
        {
            View = null;
            PlatformFont = null;//从Maui的FontManager获取的, 是否Dispose不应该这里决定
#if IOS || MACCATALYST
            var keys = PlatformFontCache.Keys;
            foreach ( var key in keys )
            {
                var font = PlatformFontCache[key];
                PlatformFontCache[key] = null;
                font.Dispose();
            }
            PlatformFontCache.Clear();
#endif
        }
    }
}
