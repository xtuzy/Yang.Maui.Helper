namespace Yang.Maui.Helper.Graphics.Hyphen
{
    internal static class StringExtensions
    {
        public static int codePointAt(this string str, int index)
        {
            return char.ConvertToUtf32(str, index);
        }

        /// <summary>
        /// java的是截取位置, C#截取长度, Java转C#时需要转换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static string SubstringLikeJava(this string str, int startIndex, int endIndex)
        {
            return str.Substring(startIndex, endIndex - startIndex);
        }
    }
}
