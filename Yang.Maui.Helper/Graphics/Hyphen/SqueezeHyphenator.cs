using System;
using System.Collections.Generic;

namespace Yang.Maui.Helper.Graphics.Hyphen
{
    public class SqueezeHyphenator : IHyphenator
    {
        private static SqueezeHyphenator squeezeHyphenator = new SqueezeHyphenator();

        private SqueezeHyphenator()
        {
        }

        public static SqueezeHyphenator getInstance()
        {
            return squeezeHyphenator;
        }

        public List<String> hyphenate(String word)
        {

            List<String> broken = new List<String>();
            int len = word.Length - 1, i;

            for (i = 0; i < len; i += 2)
            {
                broken.Add(word.Substring(i, i + 2));
            }

            if (i < len)
            {
                broken.Add(word.Substring(i, word.Length));
            }

            return broken;
        }
    }
}
