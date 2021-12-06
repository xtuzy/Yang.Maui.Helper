using System;

namespace Xamarin.Helper.Tools
{
    /// <summary>
    /// 随机数获取
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// 获得任意随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandom()
        {

#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
            Random random = new Random(Guid.NewGuid().GetHashCode());
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
            return random.Next();
        }

        /// <summary>
        /// 获得某两个数之间的随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandom(int min, int max)
        {

#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
            Random random = new Random(Guid.NewGuid().GetHashCode());
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
            return random.Next(min, max);
        }

        /// <summary>
        /// Gets the double(0-1) random.
        /// </summary>
        /// <returns></returns>
        public static double GetDoubleRandom()
        {

#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
            Random random = new Random(Guid.NewGuid().GetHashCode());
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
            return random.NextDouble();
        }
    }
}