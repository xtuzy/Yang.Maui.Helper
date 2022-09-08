using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Yang.Maui.Helper.Files;

namespace Yang.Maui.Helper.Tools
{
    /// <summary>
    /// 管理Skia读取的自定义字体(非系统字体),用于缓存.
    /// 如果需要系统字体,可以使用SKFontManager.
    /// 参考:<see href="https://github.com/mono/SkiaSharp/issues/1471"/>
    /// </summary>
    public class CustomFontManager
    {
        private static readonly ConcurrentDictionary<string, WeakReference<SKTypeface>> cache =
            new ConcurrentDictionary<string, WeakReference<SKTypeface>>();
        
        /// <summary>
        /// 从具体的路径读取字体
        /// </summary>
        /// <param name="path">具体的路径</param>
        /// <returns></returns>
        public static SKTypeface Get(string path)
        {
            path = path.ToLowerInvariant();

            if (cache.TryGetValue(path, out var weak) && weak.TryGetTarget(out var tf))
                return tf;

            tf = SKTypeface.FromFile(path);
            cache[path] = new WeakReference<SKTypeface>(tf);

            return tf;
        }

        /// <summary>
        /// 从Maui的Fonts文件夹获取字体
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<SKTypeface> GetMauiFont(string fontFullName)
        {
            if (cache.TryGetValue(fontFullName, out var weak) && weak.TryGetTarget(out var tf))
                return tf;

            tf = SKTypeface.FromStream(await FileHelper.LoadMauiFont(fontFullName));
            cache[fontFullName] = new WeakReference<SKTypeface>(tf);

            return tf;
        }
    }
}
