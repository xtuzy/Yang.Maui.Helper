using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Yang.Maui.Helper.Tools
{
    /// <summary>
    /// 尝试写一个通用字体加载
    /// </summary>
    public class FontLoadHelper
    {
        static List<string> SystemFonts;
        
        public FontLoadHelper()
        {
            if (SystemFonts == null)
                SystemFonts = SKFontManager.Default.FontFamilies.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="font">可以是纯名字,可以是带后缀,可以是路径</param>
        /// <returns></returns>
        async Task<SKTypeface> Load(string font)
        {
            var skFontManager = SKFontManager.Default;
            //路径
            if (font.Contains('/') || font.Contains('\\'))
            {
                //获取名字,看系统字体中是否含有
                var fontName = System.IO.Path.GetFileNameWithoutExtension(font);
                if (SystemFonts.Contains(fontName))
                {
                    return SKTypeface.FromFamilyName(fontName);
                }
                //系统没有那查找是否应用自带
                else
                {
                    return CustomFontManager.Get(font);
                }
            }
            //后缀
            else if (font.Contains(".ttf"))
            {
                var fontName = font.Split('.')[0];
                if (SystemFonts.Contains(fontName))
                {
                    return SKTypeface.FromFamilyName(fontName);
                }
                //系统没有那查找是否应用自带
                else
                {
                    return await CustomFontManager.GetMauiFont(font);
                }
            }
            //纯名字
            else
            {
                var fontName = font;
                if (SystemFonts.Contains(fontName))
                {
                    return SKTypeface.FromFamilyName(fontName);
                }
                //系统没有那查找是否应用自带
                else
                {
                    return await CustomFontManager.GetMauiFont(font + ".ttf");
                }
            }
        }
    }
}
