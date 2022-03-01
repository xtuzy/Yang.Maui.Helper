using System;
using System.Diagnostics;
using System.Text;

namespace Maui.Platform.Helper.Tools
{
    /// <summary>
    /// 输出窗口记录执行过程中的时间点,类似于打点记录.
    /// 用在测试执行代码块使用时间.
    /// </summary>
    public static class RecordTimeHelper
    {
        static Stopwatch timer;
        static StringBuilder result = new StringBuilder();
        /// <summary>
        /// 开始计时
        /// </summary>
        public static void StartRecordTime()
        {
            /// <summary>
            /// 性能计时器
            /// </summary>
            timer = new Stopwatch();
            timer.Start();
        }
        /// <summary>
        /// 记录时间(ms)
        /// </summary>
        /// <param name="message"></param>
        public static void RecordTime(string message)
        {
            if(timer != null)
                result.AppendLine("RecordTime:" + timer.ElapsedMilliseconds + " RecordMessage:" + message);
        }
        /// <summary>
        /// 停止计时
        /// </summary>
        public static void StopRecordTime()
        {
            if (timer != null)
            {
                timer.Stop();
                Debug.WriteLine(result.ToString());
            }
                
        }
        /// <summary>
        /// 记录数据
        /// </summary>
        /// <param name="message"></param>
        public static void RecordDate(string message)
        {
            if (timer != null)
                result.AppendLine("RecordDate:" + DateTime.Now + " RecordMessage:" + message);
        }
    }
}
