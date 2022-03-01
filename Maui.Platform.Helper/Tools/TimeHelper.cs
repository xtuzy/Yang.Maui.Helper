using System;

namespace Maui.Platform.Helper.Tools
{
    public class TimeHelper
    {
        /// <summary>
        /// time form 1970.1.1 start
        /// </summary>
        static public long CurrentSystemTimeMillis
        {
            get
            {
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
                long millis = (long)ts.TotalMilliseconds;
                return millis;
            }
        }
    }
}
