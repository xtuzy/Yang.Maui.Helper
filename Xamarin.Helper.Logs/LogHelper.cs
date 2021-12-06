using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Helper.Logs
{
    public delegate void LogEventHandler(string msg);
    /// <summary>
    /// 可以对Log进行配置,默认使用Console打印到输出窗口
    /// </summary>
    public static partial class LogHelper
    {
        public static event LogEventHandler DebugEvent;
        public static event LogEventHandler ErrorEvent;
        public static event LogEventHandler InfoEvent;
        public static event LogEventHandler WarnEvent;
        public static void Debug(string msg)
        {
            if (DebugEvent != null)
                DebugEvent(msg);
            else
                DefaultDebug(msg);
        }

        public static void Debug(string format, params object[] arg)
        {
            Debug(string.Format(format, arg));
        }


        public static void Error(string msg)
        {
            if(ErrorEvent != null)
                ErrorEvent(msg);
            ErrorEvent(msg);
        }

        public static void Error(string format, params object[] arg)
        {
            Error(string.Format(format, arg));
        }

        public static void Info(string msg)
        {
            if(InfoEvent != null)
                InfoEvent(msg);
            InfoEvent(msg);
        }

        public static void Info(string format, params object[] arg)
        {
            Info(string.Format(format, arg));
        }

        public static void Warn(string msg)
        {
            if(WarnEvent != null)
                WarnEvent(msg);
            WarnEvent(msg);
        }

        public static void Warn(string format, params object[] arg)
        {
            Warn(string.Format(format, arg));
        }
    }
}
