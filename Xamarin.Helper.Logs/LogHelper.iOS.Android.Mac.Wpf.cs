
#if __WPF__
using Console =  System.Diagnostics.Debug;
#else
using System;
#endif
namespace Xamarin.Helper.Logs
{
    /// <summary>
    /// 用于各平台用其可用的默认输出工具
    /// </summary>
    public static partial class LogHelper
    {
        public static void DefaultDebug(string msg)
        {
            Console.WriteLine("[DEBUG] " + msg);
        }
        public static void DefaultError(string msg)
        {
            Console.WriteLine("[ERROR] " + msg);
        }
        public static void DefaultInfo(string msg)
        {
            Console.WriteLine("[INFO] " + msg);
        }

        public static void DefaultWarn(string msg)
        {
            Console.WriteLine("[WARN] " + msg);
        }
    }
}